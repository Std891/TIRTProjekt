using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;

namespace TeleinfTraffic
{
    public class GeneratorDisplay : Panel
    {
        private readonly int sizeHeight = 250;
        private readonly int sizeWidth = 800;
        private int _tickCount = 0;
        public List<PointXY> generatorPoints = new List<PointXY>();
        private System.Threading.Timer _timer;

        private bool _emulationStarted;
        private bool _startedAtLeastOnce;
        private bool _emulatorShown;
        public int index;
        public int creationTick = 0;
        private Form1 mainForm;
        public GeneratorFlow flow;


        private String[] distributions = new string[5];
        private Panel _contents;
        private Panel _settingsPanel;
        private Panel _flowDisplayPanel;
        private Chart _chart;

        private ComboBox _tBPacketsCombo;
        private ComboBox _pIPCombo;
        private ComboBox _tBPtCombo;

        private Label _tbpacketsChartLabel;
        private Label _plChartLabel;
        private Label _tbpackagesChartLabel;

        private Label _tBPacketsComboLabel;
        private Label _pIPComboLabel;
        private Label _tBPtComboLabel;


        private Button _startStopButton;
        private Button _pauseButton;
        
        private Label _indexLabel;

        private TextBox _tBPBox_first;
        private TextBox _tBPBox_second;
        private TextBox _pIPBox_first;
        private TextBox _pIPBox_second;
        private TextBox _tBPtBox_first;
        private TextBox _tBPtBox_second;

        private Label _tBPBox_first_Label;
        private Label _tBPBox_second_Label;
        private Label _pIPBox_first_Label;
        private Label _pIPBox_second_Label;
        private Label _tBPtBox_first_Label;
        private Label _tBPtBox_second_Label;

        public List<StatisticsPoint> TbpStatistics = new List<StatisticsPoint>();
        public List<StatisticsPoint> PipStatistics = new List<StatisticsPoint>();
        public List<StatisticsPoint> TbptStatistics = new List<StatisticsPoint>();

        public GeneratorDisplay(Form1 form, int ind, GeneratorFlow f, int tick)
        {
            mainForm = form;
            index = ind;
            flow = f;
            creationTick = tick;
            InitializeComponents();
            
        }

        private void InitializeComponents()
        {
            Size = new System.Drawing.Size(sizeWidth, sizeHeight-1);

            InitializeControls();

            _emulationStarted = false;

            _startedAtLeastOnce = false;


            _tBPacketsCombo.SelectedIndex = 0;
            _pIPCombo.SelectedIndex = 0;
            _tBPtCombo.SelectedIndex = 0;
            
        }

        private void InitializeControls()
        {
            distributions[0] = "Poisson";
            distributions[1] = "Wykładniczy";
            distributions[2] = "Normalny";
            distributions[3] = "Pareto";
            distributions[4] = "Erlanga";

            this.BackColor = Color.Black;

            // _contents panel
            _contents = new Panel();
            _contents.Location = new Point(1, 1);
            _contents.BackColor = Color.White;
            _contents.Size = new Size(sizeWidth - 2, sizeHeight - 3);
            _contents.Parent = this;

            // settings panel
            _settingsPanel = new Panel();
            _settingsPanel.Location = new Point(0, 0);
            _settingsPanel.Size = new Size(sizeWidth - 2 - 200, _contents.Size.Height);
            _settingsPanel.Parent = _contents;

            _indexLabel = new Label();
            _indexLabel.Location = new Point(_contents.Width - 40, 0);
            _indexLabel.Font = new Font("Microsoft Sans Serif", 15);
            _indexLabel.TextAlign = ContentAlignment.MiddleRight;
            _indexLabel.Size = new Size(30, 40);
            _indexLabel.Text = (Convert.ToInt32(index) + 1).ToString();
            _indexLabel.Parent = _contents;

            // button
            _startStopButton = new Button();
            _startStopButton.Text = "Start";
            _startStopButton.TextAlign = ContentAlignment.MiddleCenter;
            _startStopButton.Location = new Point(630, Height/2-19);
            _startStopButton.Size = new Size(100, 30);
            _startStopButton.Click += new System.EventHandler(Start_stopButtonClick);
            _startStopButton.Parent = _contents;

            _pauseButton = new Button();
            _pauseButton.Text = "Wstrzymaj";
            _pauseButton.TextAlign = ContentAlignment.MiddleCenter;
            _pauseButton.Location = new Point(630, _startStopButton.Location.Y+50);
            _pauseButton.Size = new Size(100,30);
            _pauseButton.Click += PauseButtonClick;
            _pauseButton.Parent = _contents;
            _pauseButton.Visible = false;



            // dystrybucje
            _tBPacketsCombo = new ComboBox();
            _tBPacketsCombo.Location = new Point(80, 60);
            _tBPacketsCombo.Size = new Size(200, 20);
            _tBPacketsCombo.Items.AddRange(distributions);
            _tBPacketsCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _tBPacketsCombo.SelectedIndexChanged += comboBox_tbpackets_SelectedIndexChanged;
            _tBPacketsCombo.Parent = _settingsPanel;

            _pIPCombo = new ComboBox();
            _pIPCombo.Location = new Point(80, 110);
            _pIPCombo.Size = new Size(200, 20);
            _pIPCombo.Items.AddRange(distributions);
            _pIPCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _pIPCombo.SelectedIndexChanged += comboBox_pl_SelectedIndexChanged;
            _pIPCombo.Parent = _settingsPanel;

            _tBPtCombo = new ComboBox();
            _tBPtCombo.Location = new Point(80, 160);
            _tBPtCombo.Size = new Size(200, 20);
            _tBPtCombo.Items.AddRange(distributions);
            _tBPtCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            _tBPtCombo.SelectedIndexChanged += comboBox_tbpackages_SelectedIndexChanged;
            _tBPtCombo.Parent = _settingsPanel;

            // labele dystrybucji
            _tBPacketsComboLabel = new Label();
            _tBPacketsComboLabel.Location = new Point(80, 40);
            _tBPacketsComboLabel.Size = new Size(200, 20);
            _tBPacketsComboLabel.Text = "Rozkład czasu pomiędzy pakietami";
            _tBPacketsComboLabel.Parent = _settingsPanel;

            _pIPComboLabel = new Label();
            _pIPComboLabel.Location = new Point(80, 90);
            _pIPComboLabel.Size = new Size(200, 20);
            _pIPComboLabel.Text = "Rozkład długości paczek";
            _pIPComboLabel.Parent = _settingsPanel;

            _tBPtComboLabel = new Label();
            _tBPtComboLabel.Location = new Point(80, 140);
            _tBPtComboLabel.Size = new Size(200, 20);
            _tBPtComboLabel.Text = "Rozkład czasu pomiędzy paczkami";
            _tBPtComboLabel.Parent = _settingsPanel;

            // parametry dystrybucji
            _tBPBox_first = new TextBox();
            _tBPBox_first.Location = new Point(310, 60);
            _tBPBox_first.Size = new Size(50, 20);
            _tBPBox_first.Parent = _settingsPanel;

            _tBPBox_second = new TextBox();
            _tBPBox_second.Location = new Point(380, 60);
            _tBPBox_second.Size = new Size(50, 20);
            _tBPBox_second.Parent = _settingsPanel;

            _pIPBox_first = new TextBox();
            _pIPBox_first.Location = new Point(310, 110);
            _pIPBox_first.Size = new Size(50, 20);
            _pIPBox_first.Parent = _settingsPanel;

            _pIPBox_second = new TextBox();
            _pIPBox_second.Location = new Point(380, 110);
            _pIPBox_second.Size = new Size(50, 20);
            _pIPBox_second.Parent = _settingsPanel;

            _tBPtBox_first = new TextBox();
            _tBPtBox_first.Location = new Point(310, 160);
            _tBPtBox_first.Size = new Size(50, 20);
            _tBPtBox_first.Parent = _settingsPanel;

            _tBPtBox_second = new TextBox();
            _tBPtBox_second.Location = new Point(380, 160);
            _tBPtBox_second.Size = new Size(50, 20);
            _tBPtBox_second.Parent = _settingsPanel;

            // labele parametrów
            _tBPBox_first_Label = new Label();
            _tBPBox_first_Label.Location = new Point(310, 40);
            _tBPBox_first_Label.Size = new Size(50, 20);
            _tBPBox_first_Label.Text = "(first)";
            _tBPBox_first_Label.Parent = _settingsPanel;

            _tBPBox_second_Label = new Label();
            _tBPBox_second_Label.Location = new Point(380, 40);
            _tBPBox_second_Label.Size = new Size(70, 20);
            _tBPBox_second_Label.Text = "(second)";
            _tBPBox_second_Label.Parent = _settingsPanel;

            _pIPBox_first_Label = new Label();
            _pIPBox_first_Label.Location = new Point(310, 90);
            _pIPBox_first_Label.Size = new Size(50, 20);
            _pIPBox_first_Label.Text = "(first)";
            _pIPBox_first_Label.Parent = _settingsPanel;

            _pIPBox_second_Label = new Label();
            _pIPBox_second_Label.Location = new Point(380, 90);
            _pIPBox_second_Label.Size = new Size(70, 20);
            _pIPBox_second_Label.Text = "(second)";
            _pIPBox_second_Label.Parent = _settingsPanel;

            _tBPtBox_first_Label = new Label();
            _tBPtBox_first_Label.Location = new Point(310, 140);
            _tBPtBox_first_Label.Size = new Size(50, 20);
            _tBPtBox_first_Label.Text = "(first)";
            _tBPtBox_first_Label.Parent = _settingsPanel;

            _tBPtBox_second_Label = new Label();
            _tBPtBox_second_Label.Location = new Point(380, 140);
            _tBPtBox_second_Label.Size = new Size(70, 20);
            _tBPtBox_second_Label.Text = "(second)";
            _tBPtBox_second_Label.Parent = _settingsPanel;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
            // flow display
            _flowDisplayPanel = new Panel();
            _flowDisplayPanel.Location = new Point(0, 0);
            _flowDisplayPanel.Size = new Size(sizeWidth - 2 - 200, sizeHeight - 2);
            _flowDisplayPanel.Parent = _contents;
            _flowDisplayPanel.AutoScroll = true;
            _flowDisplayPanel.Visible = false;

            // rysunek
            _chart = new Chart();
            _chart.Location = new Point(0, 0);
            _chart.Size = new Size(200, _flowDisplayPanel.Height);
            _chart.Parent = _flowDisplayPanel;

            ChartArea ca = new ChartArea();
            ca.Name = "FlowArea";

            Series ss = new Series();
            ss.Name = "FlowSeries";
            ss.ChartArea = "FlowArea";
            ss.ChartType = SeriesChartType.Column;
           

            _chart.ChartAreas.Add(ca);
            _chart.Series.Add(ss);

            _chart.ChartAreas["FlowArea"].Position.Width = 100;
            _chart.ChartAreas["FlowArea"].Position.Height = 100;
            _chart.Series["FlowSeries"]["PointWidth"] = "1";
            _chart.Series["FlowSeries"]["PixelPointWidth"] = "8";
        }

        private void EmulatorDisplayButtonClick(object sender, EventArgs e)
        {
            if (_emulatorShown)
                HideEmulator();
            else
                ShowEmulator();
            
        }

        private void ShowEmulator()
        {
            if (_emulatorShown)
                return;

            if (_flowDisplayPanel.InvokeRequired)
            {
                _flowDisplayPanel.Invoke(new Action(() =>
                {
                    _flowDisplayPanel.Visible = true;
                }));
            }
            else
            {
                _flowDisplayPanel.Visible = true;
            }

            if (_settingsPanel.InvokeRequired)
            {
                _settingsPanel.Invoke(new Action(() =>
                {
                    _settingsPanel.Visible = false;
                }));
            }
            else
            {
                _settingsPanel.Visible = false;
            }
            _pauseButton.Visible = true;
            _emulatorShown = true;
        }

        private void HideEmulator()
        {
            if (!_emulatorShown)
                return;

            if (_flowDisplayPanel.InvokeRequired)
            {
                _flowDisplayPanel.Invoke(new Action(() =>
                {
                    _flowDisplayPanel.Visible = false;
                }));
            }
            else
            {
                _flowDisplayPanel.Visible = false;
            }

            if (_settingsPanel.InvokeRequired)
            {
                _settingsPanel.Invoke(new Action(() =>
                {
                    _settingsPanel.Visible = true;
                }));
            }
            else
            {
                _settingsPanel.Visible = true;
            }
            _pauseButton.Visible = false;
            _emulatorShown = false;
        }

        public void randomizeDistributions()
        {
            
        }

        public void setTimer(System.Threading.Timer t)
        {
            _timer = t;
        }

        public void StartEvent()
        {
            flow.updateParameters(
                _tBPacketsCombo.SelectedIndex,
                _tBPBox_first.Text,
                _tBPBox_second.Text,
                _pIPCombo.SelectedIndex,
                _pIPBox_first.Text,
                _pIPBox_second.Text,
                _tBPtCombo.SelectedIndex,
                _tBPtBox_first.Text,
                _tBPtBox_second.Text);

            startEmulation();
            if (!_startedAtLeastOnce)
            {
                mainForm.StartFlow(index);
                _startedAtLeastOnce = true;
            }
            else
            {
                _timer.Change(1000, 1);
            }
        }

        public void StopEvent()
        {
            flow.needStop();
            if (_startStopButton.InvokeRequired)
            {
                _startStopButton.Invoke(new Action(() => { _startStopButton.Text = "Start"; }));
            }
            else
                _startStopButton.Text = "Start";

            stopEmulation();


            /* if (!mainForm.displayBoxes.Exists(x=>x._emulationStarted=true))
             {
                 mainForm.areStarted = false;
                 mainForm.button1.Text = "Rozpocznij Wszystkie";
             }*/
        }

        public void timerTick(object state)
        {
            flow.GeneratorTick(this);
        }

        public void DisplayTick(int tickNumber, List<PointXY> points, int packetsCount, int packagesCount)
        {
            int diff = tickNumber - _tickCount;
            _tickCount = tickNumber;
            if (_chart.InvokeRequired)
            {
                _chart.Invoke(new Action(() => {
                    for (int i = 0; i < points.Count; i++)
                    {

                            

                        _chart.Series["FlowSeries"].Points.AddXY(points[i].X, points[i].Y);
                    }

                    _chart.ClientSize = new Size(_chart.ClientSize.Width + 10 * diff, _chart.ClientSize.Height);
                }));
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    _chart.Series["FlowSeries"].Points.AddXY(points[i].X, points[i].Y);
                }

                _chart.ClientSize = new Size(_chart.ClientSize.Width + 10 * diff, _chart.ClientSize.Height);
            }

            if (_flowDisplayPanel.InvokeRequired)
            {
                _flowDisplayPanel.Invoke(new Action(() => { _flowDisplayPanel.AutoScrollPosition = new Point(_chart.Width, _chart.Height); }));
            }
            else
                _flowDisplayPanel.AutoScrollPosition = new Point(_chart.Width, _chart.Height);
        }

        private void ShowStatisticsButtonClick(object sender, EventArgs e)
        {
            
        }

        private void Start_stopButtonClick(object sender, EventArgs e)
        {
            if (!_emulationStarted)
            {
                StartEvent();
            }
            else
            {
                StopEvent();
            }
        }

        private void PauseButtonClick(object sender, EventArgs e)
        {
            if (_emulationStarted)
            {
                _pauseButton.Text = "Wznów";
                _emulationStarted = false;

                if (_timer != null)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
            else
            {
                _pauseButton.Text = "Wstrzymaj";
                _emulationStarted = true;
                _timer.Change(1000, 1);
            }
        }

        public void startEmulation()
        {

            if (_startStopButton.InvokeRequired)
            {
                _startStopButton.Invoke(new Action(() =>
                {
                    _startStopButton.Text = "Stop";
                }));
            }
            else
            {
                _startStopButton.Text = "Stop";
            }

            _emulationStarted = true;

            ShowEmulator();

            flow.StartEmulation();

        }

        public void stopEmulation()
        {
            if (_startStopButton.InvokeRequired)
            {
                _startStopButton.Invoke(new Action(() =>
                {
                    _startStopButton.Text = "Start";
                }));
            }
            else
            {
                _startStopButton.Text = "Start";
            }
            _emulationStarted = false;

            if (_timer != null)
            {
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            HideEmulator();

        }

        private void textBox_KeyPress_double(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
               (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox_KeyPress_integer(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void comboBox_tbpackets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                _tBPBox_first_Label.Text = "Średnia";
                _tBPBox_first.Text = "4";
                _tBPBox_first.KeyPress += textBox_KeyPress_integer;

                _tBPBox_second_Label.Visible = false;
                _tBPBox_second.Visible = false;
            }
            else 
            if (((ComboBox) sender).SelectedIndex == 1)
            {
                _tBPBox_first_Label.Text = "Lambda";
                _tBPBox_first.Text = "4";
                _tBPBox_first.KeyPress += textBox_KeyPress_integer;

                _tBPBox_second_Label.Visible = false;
                _tBPBox_second.Visible = false;
            }
            else
    if (((ComboBox)sender).SelectedIndex == 2)
            {
                _tBPBox_first_Label.Text = "Średnia";
                _tBPBox_first.Text = "4";
                _tBPBox_first.KeyPress += textBox_KeyPress_double;

                _tBPBox_second_Label.Visible = true;
                _tBPBox_second.Visible = true;

                _tBPBox_second_Label.Text = "Odchylenie";
                _tBPBox_second.Text = "2";
                _tBPBox_second.KeyPress += textBox_KeyPress_double;
            }
            else
    if (((ComboBox)sender).SelectedIndex == 3)
            {
                _tBPBox_first_Label.Text = "Skala";
                _tBPBox_first.Text = "4";
                _tBPBox_first.KeyPress += textBox_KeyPress_double;

                _tBPBox_second_Label.Visible = true;
                _tBPBox_second.Visible = true;

                _tBPBox_second_Label.Text = "Kształt";
                _tBPBox_second.Text = "2";
                _tBPBox_second.KeyPress += textBox_KeyPress_double;
            }
            else
    if (((ComboBox)sender).SelectedIndex == 4)
            {
                _tBPBox_first_Label.Text = "Częstość";
                _tBPBox_first.Text = "4";
                _tBPBox_first.KeyPress += textBox_KeyPress_integer;

                _tBPBox_second_Label.Visible = true;
                _tBPBox_second.Visible = true;

                _tBPBox_second_Label.Text = "Kształt";
                _tBPBox_second.Text = "2";
                _tBPBox_second.KeyPress += textBox_KeyPress_double;
            }
        }

        private void comboBox_pl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0)
            {
                _pIPBox_first_Label.Text = "Średnia";
                _pIPBox_first.Text = "30";
                _pIPBox_first.KeyPress += textBox_KeyPress_integer;

                _pIPBox_second_Label.Visible = false;
                _pIPBox_second.Visible = false;
            }
            else if (((ComboBox) sender).SelectedIndex == 1)
            {
                _pIPBox_first_Label.Text = "Lambda";
                _pIPBox_first.Text = "30";
                _pIPBox_first.KeyPress += textBox_KeyPress_integer;

                _pIPBox_second_Label.Visible = false;
                _pIPBox_second.Visible = false;
            }
            else
               if (((ComboBox)sender).SelectedIndex == 2)
            {
                _pIPBox_first_Label.Text = "Średnia";
                _pIPBox_first.Text = "30";
                _pIPBox_first.KeyPress += textBox_KeyPress_double;

                _pIPBox_second_Label.Visible = true;
                _pIPBox_second.Visible = true;

                _pIPBox_second_Label.Text = "Odchylenie";
                _pIPBox_second.Text = "5";
                _pIPBox_second.KeyPress += textBox_KeyPress_double;
            }
            else
               if (((ComboBox)sender).SelectedIndex == 3)
            {
                _pIPBox_first_Label.Text = "Skala";
                _pIPBox_first.Text = "25";
                _pIPBox_first.KeyPress += textBox_KeyPress_double;

                _pIPBox_second_Label.Visible = true;
                _pIPBox_second.Visible = true;

                _pIPBox_second_Label.Text = "Kształt";
                _pIPBox_second.Text = "4";
                _pIPBox_second.KeyPress += textBox_KeyPress_double;
            }
            else
               if (((ComboBox)sender).SelectedIndex == 4)
            {
                _pIPBox_first_Label.Text = "Częstość";
                _pIPBox_first.Text = "55";
                _pIPBox_first.KeyPress += textBox_KeyPress_integer;

                _pIPBox_second_Label.Visible = true;
                _pIPBox_second.Visible = true;

                _pIPBox_second_Label.Text = "Kształt";
                _pIPBox_second.Text = "4";
                _pIPBox_second.KeyPress += textBox_KeyPress_double;
            }
        }

        private void comboBox_tbpackages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 0 )
            {
                _tBPtBox_first_Label.Text = "Średnia";
                _tBPtBox_first.Text = "10";
                _tBPtBox_first.KeyPress += textBox_KeyPress_integer;

                _tBPtBox_second_Label.Visible = false;
                _tBPtBox_second.Visible = false;
            }
            else if(((ComboBox)sender).SelectedIndex == 1)
            {
                _tBPtBox_first_Label.Text = "Lambda";
                _tBPtBox_first.Text = "10";
                _tBPtBox_first.KeyPress += textBox_KeyPress_integer;

                _tBPtBox_second_Label.Visible = false;
                _tBPtBox_second.Visible = false;
            }
            else
                if (((ComboBox)sender).SelectedIndex == 2)
            {
                _tBPtBox_first_Label.Text = "Średnia";
                _tBPtBox_first.Text = "10";
                _tBPtBox_first.KeyPress += textBox_KeyPress_double;

                _tBPtBox_second_Label.Visible = true;
                _tBPtBox_second.Visible = true;

                _tBPtBox_second_Label.Text = "Odchylenie";
                _tBPtBox_second.Text = "3";
                _tBPtBox_second.KeyPress += textBox_KeyPress_double;
            }
            else
                if (((ComboBox)sender).SelectedIndex == 3)
            {
                _tBPtBox_first_Label.Text = "Skala";
                _tBPtBox_first.Text = "10";
                _tBPtBox_first.KeyPress += textBox_KeyPress_double;

                _tBPtBox_second_Label.Visible = true;
                _tBPtBox_second.Visible = true;

                _tBPtBox_second_Label.Text = "Kształt";
                _tBPtBox_second.Text = "5";
                _tBPtBox_second.KeyPress += textBox_KeyPress_double;
            }
            else
                if (((ComboBox)sender).SelectedIndex == 4)
            {
                _tBPtBox_first_Label.Text = "Częstość";
                _tBPtBox_first.Text = "10";
                _tBPtBox_first.KeyPress += textBox_KeyPress_integer;

                _tBPtBox_second_Label.Visible = true;
                _tBPtBox_second.Visible = true;

                _tBPtBox_second_Label.Text = "Kształt";
                _tBPtBox_second.Text = "2";
                _tBPtBox_second.KeyPress += textBox_KeyPress_double;
            }
        }

        public bool isEmulationStarted()
        {
            return _emulationStarted;
        }

        public void DeleteDisplay()
        {
            stopEmulation();

            foreach (Control c in Controls)
                c.Dispose();

            this.Dispose();
        }
    }
}
