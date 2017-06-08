using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;
using System.Diagnostics;

namespace TeleinfTraffic
{
    public class StatDisplay : Panel
    {
        private readonly int sizeHeight = 300;
        private readonly int sizeWidth = 800;

        private Chart chart1;
        private Chart chart2;
        private Chart chart3;
        private Panel _contents;

        private Label _indexLabel;
        private int index;
        private FormStats mainForm;

        public StatDisplay(FormStats form, int id, List<StatisticsPoint> pip, List<StatisticsPoint> tbp, List<StatisticsPoint> tbpt)
        {
            mainForm = form;
            index = id;

            InitializeComponents(pip, tbp, tbpt);
        }
        public StatDisplay(FormStats form, List<StatisticsPoint> pip, List<StatisticsPoint> tbp, List<StatisticsPoint> tbpt)
        {
            mainForm = form;

            InitializeComponentsSumm(pip, tbp, tbpt);
        }
        private void InitializeComponents(List<StatisticsPoint> pip, List<StatisticsPoint> tbp, List<StatisticsPoint> tbpt)
        {
            Size = new Size(sizeWidth, sizeHeight);

            double sum = 0;
            int count = 0;
            double sumDev = 0;
            double average = 0;
            double stDev = 0;

            InitializeControls();
            foreach (var pipv in pip)
            {
                sum += pipv.Position * pipv.Count;
                count += (int)pipv.Count;
                sumDev += (pipv.Position * pipv.Position) * pipv.Count;
                chart1.Series["StatSeries1"].Points.AddXY(pipv.position, pipv.Count);


            }

            average = sum / count;
            stDev = Math.Sqrt(sumDev / (count - 1) - (sum / count) * (sum / count));
            chart1.Titles.Add("Śr: " + Math.Round(average,2) + " | Dev: " + Math.Round(stDev,2));
            sum = 0;
            count = 0;
            sumDev = 0;
            average = 0;
            stDev = 0;

            foreach (var tbpv in tbp)
            {
                sum += tbpv.Position * tbpv.Count;
                count += (int)tbpv.Count;
                sumDev += (tbpv.Position * tbpv.Position) * tbpv.Count;
                chart2.Series["StatSeries2"].Points.AddXY(tbpv.position, tbpv.Count);

            }
            average = sum / count;
            stDev = Math.Sqrt(sumDev / (count - 1) - (sum / count) * (sum / count));
            chart2.Titles.Add("Śr: " + Math.Round(average,2) + " | Dev: " + Math.Round(stDev,2));
            sum = 0;
            count = 0;
            sumDev = 0;
            average = 0;
            stDev = 0;


            foreach (var tbptv in tbpt)
            {
                sum += tbptv.Position * tbptv.Count;
                count += (int)tbptv.Count;
                sumDev += (tbptv.Position * tbptv.Position) * tbptv.Count;
                chart3.Series["StatSeries3"].Points.AddXY(tbptv.position, tbptv.Count);
            }
            average = sum / count;
            stDev = Math.Sqrt(sumDev / (count - 1) - (sum / count) * (sum / count));
            chart3.Titles.Add("Śr: " + Math.Round(average,2) + " | Dev: " + Math.Round(stDev,2));
        }

        private void InitializeComponentsSumm(List<StatisticsPoint> pip, List<StatisticsPoint> tbp, List<StatisticsPoint> tbpt)
        {
            Size = new Size(sizeWidth, sizeHeight);

            double sum = 0;
            int count = 0;
            double sumDev = 0;
            double average = 0;
            double stDev = 0;

            InitializeControlsSumm();
            foreach (var pipv in pip)
            {
                sum += pipv.Position * pipv.Count;
                count += (int)pipv.Count;
                sumDev += (pipv.Position * pipv.Position) * pipv.Count;
                chart1.Series["StatSeries1"].Points.AddXY(pipv.position, pipv.Count);


            }

            average = sum / count;
            stDev = Math.Sqrt(sumDev / (count - 1) - (sum / count) * (sum / count));
            chart1.Titles.Add("Śr: " + Math.Round(average,2) + " | Dev: " + Math.Round(stDev,2));
            sum = 0;
            count = 0;
            sumDev = 0;
            average = 0;
            stDev = 0;

            foreach (var tbpv in tbp)
            {
                sum += tbpv.Position * tbpv.Count;
                count += (int)tbpv.Count;
                sumDev += (tbpv.Position * tbpv.Position) * tbpv.Count;
                chart2.Series["StatSeries2"].Points.AddXY(tbpv.position, tbpv.Count);

            }
            average = sum / count;
            stDev = Math.Sqrt(sumDev / (count - 1) - (sum / count) * (sum / count));
            chart2.Titles.Add("Śr: " + Math.Round(average,2) + " | Dev: " + Math.Round(stDev,2));
            sum = 0;
            count = 0;
            sumDev = 0;
            average = 0;
            stDev = 0;


            foreach (var tbptv in tbpt)
            {
                sum += tbptv.Position * tbptv.Count;
                count += (int)tbptv.Count;
                sumDev += (tbptv.Position * tbptv.Position) * tbptv.Count;
                chart3.Series["StatSeries3"].Points.AddXY(tbptv.position, tbptv.Count);
            }
            average = sum / count;
            stDev = Math.Sqrt(sumDev / (count - 1) - (sum / count) * (sum / count));
            chart3.Titles.Add("Śr: " + Math.Round(average,2) + " | Dev: " + Math.Round(stDev,2));
        }

        private void InitializeControls()
        {

            _contents = new Panel();
            _contents.Location = new Point(1, 1);
            _contents.BackColor = Color.White;
            _contents.Size = new Size(sizeWidth - 2, sizeHeight - 3);
            _contents.Parent = this;
            _contents.Visible = true;

            _indexLabel = new Label();
            _indexLabel.Location = new Point(_contents.Width - 40, 10);
            _indexLabel.Font = new Font("Microsoft Sans Serif", 15);
            _indexLabel.TextAlign = ContentAlignment.MiddleRight;
            _indexLabel.Size = new Size(30, 40);
            _indexLabel.Text = (Convert.ToInt32(index) + 1).ToString();
            _indexLabel.Parent = _contents;
            _indexLabel.Visible = true;

            chart1 = new Chart();
            chart1.Location = new Point(15,25);
            chart1.Size = new Size(240,240);
            chart1.Parent = _contents;
            ChartArea ca1 = new ChartArea();
            ca1.CursorX.IsUserSelectionEnabled = true;
            ca1.Name = "StatArea1";
            Series ss1 = new Series();
            ss1.Name = "StatSeries1";
            ss1.ChartArea = "StatArea1";
            ss1.ChartType=SeriesChartType.Column;
            chart1.ChartAreas.Add(ca1);
            chart1.Series.Add(ss1);
            chart1.Titles.Add("Packets in Packages");


            chart2 = new Chart();
            chart2.Location = new Point(280,25);
            chart2.Size = new Size(240,240);
            chart2.Parent = _contents;
            ChartArea ca2 = new ChartArea();
            ca2.Name = "StatArea2";
            ca2.CursorX.IsUserSelectionEnabled = true;
            Series ss2 = new Series();
            ss2.Name = "StatSeries2";
            ss2.ChartArea = "StatArea2";
            ss2.ChartType = SeriesChartType.Column;
            chart2.ChartAreas.Add(ca2);
            chart2.Series.Add(ss2);
            chart2.Titles.Add("Time between Packages");


            chart3 = new Chart();
            chart3.Location = new Point(545,25);
            chart3.Size = new Size(240,240);
            chart3.Parent = _contents;
            ChartArea ca3 = new ChartArea();
            ca3.Name = "StatArea3";
            ca3.CursorX.IsUserSelectionEnabled = true;
            Series ss3 = new Series();
            ss3.Name = "StatSeries3";
            ss3.ChartArea = "StatArea3";
            ss3.ChartType = SeriesChartType.Column;
            chart3.ChartAreas.Add(ca3);
            chart3.Series.Add(ss3);
            chart3.Titles.Add("Time between Packets");


            _indexLabel.BringToFront();
        }
        private void InitializeControlsSumm()
        {

            _contents = new Panel();
            _contents.Location = new Point(1, 1);
            _contents.BackColor = Color.White;
            _contents.Size = new Size(sizeWidth - 2, sizeHeight - 3);
            _contents.Parent = this;
            _contents.Visible = true;

            _indexLabel = new Label();
            _indexLabel.Location = new Point(_contents.Width - 70, 10);
            _indexLabel.Font = new Font("Microsoft Sans Serif", 13);
            _indexLabel.TextAlign = ContentAlignment.MiddleRight;
            _indexLabel.Size = new Size(60, 30);
            _indexLabel.Text = "Suma";
            _indexLabel.Parent = _contents;
            _indexLabel.Visible = true;

            chart1 = new Chart();
            chart1.Location = new Point(15, 25);
            chart1.Size = new Size(240, 240);
            chart1.Parent = _contents;
            ChartArea ca1 = new ChartArea();
            ca1.CursorX.IsUserSelectionEnabled = true;
            ca1.Name = "StatArea1";
            Series ss1 = new Series();
            ss1.Name = "StatSeries1";
            ss1.ChartArea = "StatArea1";
            ss1.ChartType = SeriesChartType.Column;
            chart1.ChartAreas.Add(ca1);
            chart1.Series.Add(ss1);
            chart1.Titles.Add("Packets in Packages");


            chart2 = new Chart();
            chart2.Location = new Point(280, 25);
            chart2.Size = new Size(240, 240);
            chart2.Parent = _contents;
            ChartArea ca2 = new ChartArea();
            ca2.Name = "StatArea2";
            ca2.CursorX.IsUserSelectionEnabled = true;
            Series ss2 = new Series();
            ss2.Name = "StatSeries2";
            ss2.ChartArea = "StatArea2";
            ss2.ChartType = SeriesChartType.Column;
            chart2.ChartAreas.Add(ca2);
            chart2.Series.Add(ss2);
            chart2.Titles.Add("Time between Packages");


            chart3 = new Chart();
            chart3.Location = new Point(545, 25);
            chart3.Size = new Size(240, 240);
            chart3.Parent = _contents;
            ChartArea ca3 = new ChartArea();
            ca3.Name = "StatArea3";
            ca3.CursorX.IsUserSelectionEnabled = true;
            Series ss3 = new Series();
            ss3.Name = "StatSeries3";
            ss3.ChartArea = "StatArea3";
            ss3.ChartType = SeriesChartType.Column;
            chart3.ChartAreas.Add(ca3);
            chart3.Series.Add(ss3);
            chart3.Titles.Add("Time between Packets");


            _indexLabel.BringToFront();
        }

        private double CalculateMean(DataPointCollection list)
        {
            double sum = 0;
            int count = 0;

            foreach (var point in list)
            {
                sum += point.XValue *point.YValues.First();
                count += (int)point.YValues.First();
            }

            return sum/count;

        }

     /*   private double CalculateStDev()
        {
            
        }
        */
    }
}
