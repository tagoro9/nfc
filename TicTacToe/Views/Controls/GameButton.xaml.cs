using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TicTacToe
{
    public partial class GameButton : UserControl
    {
        private string state = "Default";

        public GameButton()
        {
            InitializeComponent();
        }

        public Boolean HasState()
        {
            return state != "Default";
        }

        public string State
        {
            get { return state; }
            set
            {
                if (!HasState())
                {
                    state = value;
                    VisualStateManager.GoToState(this, state, true);
                }
            }
        }
    }
}
