using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThreadState = System.Threading.ThreadState;

namespace TeleinfTraffic
{

    public partial class Form1 : Form
    {

        public int overallTick = 0;
        public bool areStarted = false;
        private int nextFlowNumber = 0;
        public List<GeneratorDisplay> displayBoxes = new List<GeneratorDisplay>();
        private List<Thread> threads = new List<Thread>();
        private List<System.Threading.Timer> timers = new List<System.Threading.Timer>();
        private bool _emulationStarted = false;
        private int boxY = 100;
        private int boxX = 50;
        private String[] distributions = new string[5];

        private List<PointXY> CombinedPoints = new List<PointXY>();
        private double maxValue = 0;
        public Form1()
        {
            InitializeComponent();

            distributions[0] = "Poisson";
            distributions[1] = "Wykładniczy";
            distributions[2] = "Normalny";
            distributions[3] = "Pareto";
            distributions[4] = "Erlang";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Puste;
        }

      /*  private void button1_Click(object sender, EventArgs e)
        {
            if (areStarted)
            {
                foreach (GeneratorDisplay gd in displayBoxes)
                {
                    gd.StopEvent();
                    areStarted = false;
                    button1.Text = "Rozpocznij Wszystkie";
                }

                summarizePoints();
                FormSummaryGraph summaryDisplay = new FormSummaryGraph(maxValue, CombinedPoints);
                summaryDisplay.Visible = true;
                Debug.WriteLine(maxValue +"    -    C:"+CombinedPoints.Count);
            }
            else if (!areStarted)
            {
                foreach (GeneratorDisplay gd in displayBoxes)
                {
                    gd.StartEvent();
                    areStarted = true;
                    button1.Text = "Zatrzymaj Wszystkie";
                }
            }

        }*/

        private void addGenerator_Click(object sender, EventArgs e)
        {
            // Tworzenie pojedynczego generatora.
            // createFlowsButton_Click
            GeneratorFlow gen = new GeneratorFlow(this);

            GeneratorDisplay gd = new GeneratorDisplay(this, nextFlowNumber, gen,overallTick);
            gd.Location = new Point(boxX,boxY);
            gd.Parent = panel1;
            displayBoxes.Add(gd);

            boxY += gd.Height + 20;
            ManualResetEvent mre = new ManualResetEvent(false);
            Thread thread = new Thread(this.CreateFlow);
            thread.Name = nextFlowNumber.ToString();
            threads.Add(thread);

            areStarted = false;
          //  button1.Text = "Rozpocznij Wszystkie";
            nextFlowNumber++;
        }

        private void CreateFlow(object index)
        {

            System.Threading.Timer timer = new System.Threading.Timer(displayBoxes.Find(fb => fb.index == Convert.ToInt32(index)).timerTick, null, 1000, 1);

            displayBoxes.Find(fb => fb.index == Convert.ToInt32(index)).setTimer(timer);
            displayBoxes.Find(fb => fb.index == Convert.ToInt32(index)).startEmulation();

            timers.Add(timer);
        }

        public void StartFlow(int index)
        {
            Thread thread = threads.Find(t => t.Name == index.ToString());
            threads.Find(t => t.Name == index.ToString()).Start(thread.Name);

        //    button1.Text = "Zatrzymaj Wszystkie";
            _emulationStarted = true;
        }

        private void StartAllFlows()
        {
            foreach (Thread t in threads)
            {
                if (t.ThreadState != ThreadState.Running)
                {
                    displayBoxes.Find(fb => fb.index == Convert.ToInt32(t.Name)).StartEvent();
                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            FormStats stat = new FormStats(displayBoxes);
            stat.Visible = true;
        }

        private void buttonUsun_Click(object sender, EventArgs e)
        {
            if (displayBoxes.Count > 0) { 
            GeneratorDisplay gd = displayBoxes[displayBoxes.Count-1];
            displayBoxes.RemoveAt(displayBoxes.Count-1);

            nextFlowNumber = nextFlowNumber - 1;
            boxY -= gd.Height + 20;

            int gdIndex = gd.index;
            threads.Remove(threads.Find(t => t.Name == gdIndex.ToString()));

            gd.DeleteDisplay();
            }
        }

        private void summarizePoints()
        {
            CombinedPoints = new List<PointXY>();
            CombinedPoints.Add(new PointXY(0,0));
            foreach (var db in displayBoxes)
            {
              //  Debug.WriteLine("CreationTick:" + db.creationTick);
                double lastAdded = 0;
                double cTick = db.creationTick;
                foreach (var pt in db.generatorPoints)
                {
                    if (pt != null) { 
                    double x = pt.X + cTick;
                    double y = pt.Y;

                  
                    if (CombinedPoints.Find(t => t.X == x) == null)
                    {
                        CombinedPoints.Add(new PointXY(x,y));
                        lastAdded = x;
                    }
                    else
                    {
                        List<PointXY> MatchingPoints = CombinedPoints.FindAll(t => t.X == x);

                        foreach (var mp in MatchingPoints)
                        {
                            mp.Y += y;
                        }
                     }
                    }

                }   
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            summarizePoints();
            FormSummaryGraph summaryDisplay = new FormSummaryGraph(maxValue, CombinedPoints);
            summaryDisplay.Visible = true;
           // Debug.WriteLine(maxValue + "    -    C:" + CombinedPoints.Count);
        }
    }
}
