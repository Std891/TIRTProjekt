using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleinfTraffic
{
    public class StatisticsPoint
    {
        public double position;
        private int _count;

        public StatisticsPoint(double p, int c)
        {
            position = p;
            _count = c;
        }

        public double Position
        {
            get { return position; }
            set { position = value; }
        }


        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public void CountUp()
        {
            _count++;
        }
    }


}
