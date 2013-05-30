using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBCSApp.Entities
{
    public class NfcLogItem
    {

        public static string ROOT_ICONS = "/Assets/Icons/";
        public static string NFC_ICON = ROOT_ICONS + "nfc.png";
        public static string CROSS_ICON = ROOT_ICONS + "crosshair.png";
        public static string INFO_ICON = ROOT_ICONS + "information.png";

        public DateTime DateAndTime { get; set; }
        public string Event { get; set; }
        public string Icon { get; set; }

        public NfcLogItem(string ev, string icon, DateTime dt)
        {
            Event = ev;
            Icon = icon;
            DateAndTime = dt;
        }
    }
}
