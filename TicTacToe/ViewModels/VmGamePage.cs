namespace TicTacToe.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using TicTacToe.Services.Dispatcher;
    using TicTacToe.Services.Navigation;
    using TicTacToe.ViewModels.Base;
    using Windows.Networking.Proximity;
    using Windows.Networking.Sockets;
    using TicTacToe.Models;
    using Windows.Storage.Streams;

    /// <summary>
    /// MainPage ViewModel.
    /// </summary>
    public class VMGamePage : VMBase
    {
        //Services variables.
        private INavigationService navService;
        private IDispatcherService dispatcherService;

        //Commands variables.
        private DelegateCommand<bool> setAsBusyCommand;
        private DelegateCommand<int> tappedElementCommand;
        private DelegateCommand resetGameCommand;

        //Private variables
        private StreamSocket socket;
        private DataWriter dataWriter;
        private DataReader dataReader;
        private Boolean gameStarted = false;
        private Boolean isInitiator = false;
        private Boolean myTurn = false;
        private String movement = "";
        private String logInfo;

        //Position states
        private TTCGame gameArray;
        public TTCGame GameArray
        {
            get { return gameArray; }
        }

        public String LogInfo
        {
            get { return logInfo; }
            set
            {
                logInfo = value;
                RaisePropertyChanged();
            }
        }

        public Boolean GameStarted
        {
            get { return gameStarted; }
            set
            {
                gameStarted = value;
                dispatcherService.CallDispatcher(() => {
                    resetGameCommand.RaiseCanExecuteChanged();
                    tappedElementCommand.RaiseCanExecuteChanged();                
                });
            }
        }

        public Boolean MyTurn
        {
            get { return myTurn; }
            set
            {
                myTurn = value;
                dispatcherService.CallDispatcher(() =>
                {
                    resetGameCommand.RaiseCanExecuteChanged();
                    tappedElementCommand.RaiseCanExecuteChanged();
                });
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="navService">Navigation service instance resolved by DI.</param>
        /// <param name="dispatcherService">Dispatcher service instance resolved by DI.</param>
        public VMGamePage(INavigationService navService, IDispatcherService dispatcherService)
        {
            this.navService = navService;
            this.dispatcherService = dispatcherService;

            this.setAsBusyCommand = new DelegateCommand<bool>(SetAsBusyExecte);
            this.tappedElementCommand = new DelegateCommand<int>(TappedElementExecute, TappedElementCanExecute);
            this.resetGameCommand = new DelegateCommand(ResetGameExecute, ResetGameCanExecute);
            //GameArray = new String[] { "", "", "", "", "", "", "", "", "" };
            gameArray = new TTCGame();
            FindPeer();

        }

        private bool ResetGameCanExecute()
        {
            if (gameStarted && myTurn)
            {
                return true;
            }
            return false;
        }

        private void ResetBoard()
        {
            gameArray.Reset();
        }

        private async void ResetGameExecute()
        {
            LogMessage("Resetting game");
            dispatcherService.CallDispatcher(() => ResetBoard());
            dataWriter.WriteString("R");
            dataWriter.WriteInt16(Convert.ToInt16(0));
            await dataWriter.StoreAsync();
            GameStarted = false;
            isInitiator = false;
            MyTurn = false;
            HandShake();
        }

        private void FindPeer()
        {
            LogMessage("Tap peer to start the game");
            //Get informed when a peer is found
            PeerFinder.TriggeredConnectionStateChanged += TriggeredConnectionStateChanged;
            //Start advertising this app and waiting for peers
            PeerFinder.Start();
        }

        private async void HandShake()
        {
            System.Random rnd = new Random();
            Int32 myDice = rnd.Next(0, 10000);
            LogMessage("My dice: " + myDice.ToString());
            dataWriter.WriteInt32(myDice);
            await dataWriter.StoreAsync();
            var bytesRead = await dataReader.LoadAsync(4);
            if (bytesRead > 0)
            {
                Int32 peerSecret = dataReader.ReadInt32();
                LogMessage("Received dice: " + peerSecret.ToString());
                if (myDice != peerSecret)
                {
                    if (myDice > peerSecret)
                    {
                        LogMessage("You are the master");
                        isInitiator = true;
                        movement = "X";
                    }
                    else
                    {
                        LogMessage("Your peer is the master");
                        movement = "O";
                    }
                    StartGame();
                }
                else
                {
                    LogMessage("You tied, need to hanshake again!");
                    HandShake();
                }
            }

        }

        private void StartGame()
        {
            GameStarted = true;
            if (isInitiator == true)
            {
                MyTurn = true;
                LogMessage("Your turn!");
            }
            else
            {
                WaitForMovement();
            }
        }

        private async void WaitForMovement()
        {
            var bytesRead = await dataReader.LoadAsync(3);
            if (bytesRead > 0)
            {
                string peerMovement = dataReader.ReadString(1);
                int position = dataReader.ReadInt16();
                if (peerMovement == "R")
                {
                    LogMessage("Game has been reset");
                    dispatcherService.CallDispatcher(() => ResetBoard());
                    HandShake();
                }
                else
                {
                    LogMessage("Received movement, your turn!");
                    dispatcherService.CallDispatcher(() =>
                    {
                        GameArray[position] = peerMovement;
                        if (GameArray.IsWinner())
                        {
                            LogMessage("LOSER!");
                            dispatcherService.CallDispatcher(() => {
                                GameStarted = false;
                                MyTurn = false;
                                isInitiator = false;
                            });
                            LogMessage("Starting new game");
                            ResetBoard();
                            HandShake();
                            return;
                        }
                    });
                    MyTurn = true;
                }                    
            }
        }

        private void TriggeredConnectionStateChanged(object sender, TriggeredConnectionStateChangedEventArgs args)
        {
            switch (args.State)
            {
                case TriggeredConnectState.PeerFound:   // Tapped another phone
                    LogMessage("Peer found");
                    break;
                case TriggeredConnectState.Connecting:
                    LogMessage("Connecting to peer");
                    break;
                case TriggeredConnectState.Failed:
                    LogMessage("Connection failed");
                    break;
                case TriggeredConnectState.Completed:   // Connection ready to use
                    // Save socket to fields
                    socket = args.Socket;
                    dataWriter = new DataWriter(socket.OutputStream);
                    dataReader = new DataReader(socket.InputStream);
                    // Listen to incoming socket
                    LogMessage("Connected, ready to play!");
                    HandShake();
                    break;
            }
        }

        public ICommand ResetGameCommand
        {
            get { return this.resetGameCommand; }
        }

        /// <summary>
        /// Command to be binded in UI, set IsBusy to the passed value.
        /// </summary>
        public ICommand SetAsBusyCommand
        {
            get { return this.setAsBusyCommand; }
        }

        public ICommand TappedElementCommand
        {
            get { return this.tappedElementCommand; }
        }

        /// <summary>
        /// Method to be execute by the SetAsBusyCommand.
        /// </summary>
        /// <param name="parameter"></param>
        private void SetAsBusyExecte(bool parameter)
        {
            this.IsBusy = parameter;
        }

        private async void TappedElementExecute(int pos)
        {
            //button.State = movement;
            if (GameArray[pos] == "")
            {
                GameArray[pos] = movement;
                dataWriter.WriteString(movement);

                dataWriter.WriteInt16(Convert.ToInt16(pos));
                await dataWriter.StoreAsync();
                MyTurn = false;
                LogMessage("Sent my movement!");
                LogMessage("Waiting for peer movement!");
                if (GameArray.IsWinner())
                {
                    LogMessage("WINNER!");
                    dispatcherService.CallDispatcher(() =>
                    {
                        GameStarted = false;
                        MyTurn = false;
                        isInitiator = false;
                    });
                    LogMessage("Starting new game");
                    ResetBoard();
                    HandShake();
                    return;
                }
                WaitForMovement();
            }
        }

        private bool TappedElementCanExecute(int pos)
        {
            if (gameStarted && myTurn)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to log a status message on the screen
        /// </summary>
        /// <param name="message"></param>
        private void LogMessage(string message)
        {
            dispatcherService.CallDispatcher(() => { LogInfo = message; });
        }

    }
}
