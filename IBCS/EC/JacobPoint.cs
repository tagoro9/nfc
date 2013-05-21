using System;
using IBCS.Interfaces;
using IBCS.Math;

namespace IBCS.EC
{
    public class JacobPoint
    {
        public static JacobPoint INFINITY = new JacobPoint();
        private BigInt x;
        private BigInt y;
        private BigInt z;

        public BigInt X
        {
            get { return x; }
            set { x = value; }
        }

        public BigInt Y
        {
            get { return y; }
            set { y = value; }
        }

        public BigInt Z
        {
            get { return z; }
            set { z = value; }
        }

        private JacobPoint()
        {
            this.x = BigInt.ONE;
            this.y = BigInt.ONE;
            this.z = BigInt.ZERO;
        }

        public JacobPoint(BigInt x, BigInt y, BigInt z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool IsInfinity()
        {
            return this.z.Equals(BigInt.ZERO);
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((x == null) ? 0 : x.GetHashCode());
            result = prime * result + ((y == null) ? 0 : y.GetHashCode());
            result = prime * result + ((z == null) ? 0 : z.GetHashCode());
            return result;
        }

        public override bool Equals(Object obj)
        { //this could fail
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            //if ( != obj.getClass())
            //    return false;
            JacobPoint other = (JacobPoint)obj;
            if (x == null)
            {
                if (other.x != null)
                    return false;
            }
            else if (!x.Equals(other.x))
                return false;
            if (y == null)
            {
                if (other.y != null)
                    return false;
            }
            else if (!y.Equals(other.y))
                return false;
            if (z == null)
            {
                if (other.z != null)
                    return false;
            }
            else if (!z.Equals(other.z))
                return false;
            return true;
        }

        public override String ToString()
        {
            return "[" + x + "," + y + "," + z + "]";
        }
    }
}
