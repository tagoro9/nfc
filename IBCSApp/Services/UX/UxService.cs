using Coding4Fun.Toolkit.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IBCSApp.Services.UX
{
    public class UxService : IUxService
    {
        public void ShowToastNotification(string title, string message)
        {
            ToastPrompt toast = new ToastPrompt();
            toast.FontSize = 20;
            toast.Title = title;
            toast.Message = message;
            toast.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            //toast.ImageSource = new BitmapImage(new Uri("/Assets/ApplicationIcon.png", UriKind.RelativeOrAbsolute));
            toast.Show();
        }


        public void ShowMessageBox(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK);
        }
    }
}
