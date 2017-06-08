using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TeleinfTraffic
{
    public partial class FormSummaryGraph : Form
    {
        public FormSummaryGraph(double size, List<PointXY> list )
        {
            InitializeComponent(size);

            foreach (var pt in list)
            {
            //    Debug.WriteLine("["+pt.X+ ", "+pt.Y+"]");
                chart1.Series["Series1"].Points.AddXY(pt.X,pt.Y);
            }
            
        }

    }
}
