using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TicTacToe.Models
{
    public class TTCGame : INotifyPropertyChanged
    {

        private string[] gameArray;

        public TTCGame()
        {
            gameArray = new string[] { "", "", "", "", "", "", "", "", "" };
        }

        public string this[int pos]
        {
            get { return gameArray[pos - 1]; }
            set
            {
                gameArray[pos - 1] = value;
                RaisePropertyChanged();
            }

        }

        public void Reset()
        {
            for (int i = 1; i <= gameArray.Length; i++)
            {
                this[i] = "";
            }
        }

        public bool IsWinner()
        {
            if (gameArray[4] != "" && (
                (gameArray[4] == gameArray[3] && gameArray[4] == gameArray[5]) ||
                (gameArray[4] == gameArray[1] && gameArray[4] == gameArray[7]) ||
                (gameArray[4] == gameArray[0] && gameArray[4] == gameArray[8]) ||
                (gameArray[4] == gameArray[2] && gameArray[4] == gameArray[6])
            ))
                return true;
            else if (gameArray[0] != "" && (
                (gameArray[0] == gameArray[2] && gameArray[0] == gameArray[2]) ||
                (gameArray[0] == gameArray[4] && gameArray[0] == gameArray[6])
            ))
                return true;
            else if (gameArray[8] != "" && (
                (gameArray[8] == gameArray[7] && gameArray[8] == gameArray[6]) ||
                (gameArray[8] == gameArray[5] && gameArray[8] == gameArray[2])
            ))
                return true;
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Public method, called from the ViewModel to raise the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Optional, name of the property who update it value.</param>
        public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            var Handler = PropertyChanged;
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
