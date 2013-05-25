using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IBCS.BF
{
    public class SerializedBFCText
    {
        public string UX { get; set; }
        public string UY { get; set; }
        public byte[] V { get; set; }
        public byte[] W { get; set; }
    }
}
