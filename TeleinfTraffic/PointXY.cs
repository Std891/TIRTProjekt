using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleinfTraffic
{
    public class PointXY
    {
        private double _x;
        private double _y;
        public PointXY(double parX, double parY)
        {
            X = parX;
            Y = parY;
        }


        public double X
        {
            get { return _x; }
            set { _x = value; }
        }


        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

    }
}
