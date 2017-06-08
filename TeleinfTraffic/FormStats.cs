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

namespace TeleinfTraffic
{
    public partial class FormStats : Form
    {
        private List<GeneratorDisplay> generators = new List<GeneratorDisplay>();
        private List<StatDisplay> displays = new List<StatDisplay>();
        private List<StatisticsPoint> pipTotal = new List<StatisticsPoint>();
        private List<StatisticsPoint> tbpTotal = new List<StatisticsPoint>();
        private List<StatisticsPoint> tbptTotal = new List<StatisticsPoint>();
        public FormStats(List<GeneratorDisplay> gd)
        {
            int boxY = 330;
            //15

            generators = gd;
            InitializeComponent();
            //Debug.WriteLine("Generatory: "+generators.Count);
            foreach (var g in generators)
            {
                StatDisplay sd = new StatDisplay(this, g.index, g.PipStatistics, g.TbpStatistics, g.TbptStatistics);
                sd.Location = new Point(40, boxY);
                //Debug.WriteLine(sd.Location.X+"     "+sd.Location.Y);
                sd.Parent = panel1;
                boxY = boxY + sd.Height + 15;
                displays.Add(sd);

                SummarizeLists(g.PipStatistics,g.TbpStatistics,g.TbptStatistics);
            }

            StatDisplay summDisplay = new StatDisplay(this, pipTotal, tbpTotal, tbptTotal);
            summDisplay.Location = new Point(40, 15);
            summDisplay.Parent = panel1;
            displays.Add(summDisplay);
        }

        private void SummarizeLists(List<StatisticsPoint> PipStatistics, List<StatisticsPoint> TbpStatistics,
            List<StatisticsPoint> TbptStatistics)
        {
            foreach (var point in PipStatistics)
            {
                if (pipTotal.Find(x => x.position == point.Position) != null)
                {
                    pipTotal.Find(x => x.position == point.Position).Count += point.Count;
                }
                else
                {
                    pipTotal.Add(new StatisticsPoint(point.Position, point.Count));
                }
            }
            foreach (var point in TbpStatistics)
            {
                if (tbpTotal.Find(x => x.position == point.Position) != null)
                {
                    tbpTotal.Find(x => x.position == point.Position).Count += point.Count;
                }
                else
                {
                    tbpTotal.Add(new StatisticsPoint(point.Position, point.Count));
                }
            }
            foreach (var point in TbptStatistics)
            {
                if (tbptTotal.Find(x => x.position == point.Position) != null)
                {
                    tbptTotal.Find(x => x.position == point.Position).Count += point.Count;
                }
                else
                {
                    tbptTotal.Add(new StatisticsPoint(point.Position, point.Count));
                }
            }
        }

    }
}
