using System;
using IBCS.Interfaces;
using IBCS.Math;

namespace IBCS.EC
{
    public class Point
    {
        public static Point INFINITY = new Point();
        private Point()
        {
            this.x = null;
            this.y = null;
        }
        private FieldElement x;
        private FieldElement y;
        public FieldElement X
        {
            get { return x; }
            set { x = value; }
        }
        public FieldElement Y
        {
            get { return y; }
            set { y = value; }
        }

        /**
         * Create a Point.
         * @param x the x-coordinate.
         * @param y the y-coordinate.
        */
        public Point(FieldElement x, FieldElement y)
        {
            this.x = x;
            this.y = y;
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + ((x == null) ? 0 : x.GetHashCode());
            result = prime * result + ((y == null) ? 0 : y.GetHashCode());
            return result;
        }

        public override bool Equals(Object obj)
        { //This could fail!
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (!(obj is Point))
                return false;
            Point other = (Point)obj;
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
            return true;
        }

        public bool IsInfinity()
        {
            return this.x == null && this.y == null;
        }

        public override String ToString()
        {
            if (this.IsInfinity())
                return "(null,null)";
            return "(" + x + "," + y + ")";
        }

        /**
         * Return a string representation of the point "(x,y)".x,y are the given radix.
         */
        public String ToString(int radix)
        {
            if (this.IsInfinity())
                return "(null,null)";
            return "(" + x.ToString(radix) + "," + y.ToString(radix) + ")";
        }
    }
}
