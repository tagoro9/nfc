using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IBCS.BF.Key
{
    public class SerializedPrivateKey
    {
        public String KeyX { get; set; }
        public String KeyY { get; set; }
        public String PairingCofactor { get; set; }
        public String PairingGroupOrder { get; set; }
        public String CurveField { get; set; }
        public String CurveA { get; set; }
        public String CurveB { get; set; }
        public String PX { get; set; }
        public String PY { get; set; }
        public String PPubX { get; set; }
        public String PPubY { get; set; }
    }
}
