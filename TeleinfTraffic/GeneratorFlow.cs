using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeleinfTraffic
{
    public class GeneratorFlow
    {
        public Form1 mainForm;

        private int _tick = 0;
        private int _port = 0;
        private System.Threading.Timer _externalTimer; //Czy trzeba??
        public bool _isExternalTimerRunning = false; //Czy Trzeba??



        private Status _genStatus = Status.WaitingForPackage;

        private List<Package> _packages = new List<Package>();
        private bool needToStop = false;
        private double _timeBetweenPackages = 5;
        private double _packetsInPackage = 0;
        private double _timeBetweenPackets = 0;

        // Parametry do ustawienia w interfejsie/do zadania w momencie rozpoczęcia generacji:
        //  Czas między paczkami - TBP
        private int _tBP_Distribution;
        private string _tBP_first;
        private string _tBP_second;
        //  Pakiety w paczkach - PIP
        private int _pIP_Distribution;
        private string _pIP_first;
        private string _pIP_second;
        //  Czas między pakietami - TBPt
        private int _tBPt_Distribution;
        private string _tBPt_first;
        private string _tBPt_second;

        private String[] distributions = new string[5];
        public GeneratorFlow(Form1 mf)
        {
            mainForm = mf;

            distributions[0] = "Poisson";
            distributions[1] = "Wykładniczy";
            distributions[2] = "Normalny";
            distributions[3] = "Pareto";
            distributions[4] = "Erlang";
        }

        public void updateParameters(int type1, string param1_1, string param1_2, int type2, string param2_1, string param2_2, int type3, string param3_1, string param3_2)
        {
            _tBP_Distribution = type1;
            _tBP_first = param1_1;
            _tBP_second = param1_2;

            _pIP_Distribution = type2;
            _pIP_first= param2_1;
            _pIP_second = param2_2;

            _tBPt_Distribution = type3;
            _tBPt_first = param3_1;
            _tBPt_second = param3_2;
        }

        public void GeneratorTick(GeneratorDisplay gd)
        {
            List<PointXY> points = new List<PointXY>();
            _tick++;
            mainForm.overallTick++;
            

            if (_genStatus == Status.WaitingForPackage)
            {
                
                if (_timeBetweenPackages > 0)
                {
                    _timeBetweenPackages--;
                    points.Add(new PointXY(_tick, 0));
                }
                else
                {
                    
                    _packages.Add(new Package());
                    _packetsInPackage = GeneratePIP(); // GeneratePIP

                    if (gd.PipStatistics.Find(x => x.position == _packetsInPackage) != null)
                    {
                        gd.PipStatistics.Find(x => x.position == _packetsInPackage).CountUp();
                    }
                    else
                    {
                        gd.PipStatistics.Add(new StatisticsPoint(_packetsInPackage, 1));
                    }

                    points.Add(new PointXY(_tick, 0));
                    DisplayTick(gd, _tick, points);
                    foreach (var pt in points)
                    {
                        gd.generatorPoints.Add(pt);
                    }
                    points.Clear();

                    _genStatus = Status.WaitingForPacket;
                }
            }
            else if (_genStatus == Status.WaitingForPacket)
            {
                points.Add(new PointXY(_tick, 1));
                DisplayTick(gd, _tick, points);
                foreach (var pt in points)
                {
                    gd.generatorPoints.Add(pt);
                }
                points.Clear();

                if (_timeBetweenPackets > 0)
                {
                    _timeBetweenPackets--;
                    if (_timeBetweenPackets == 0)
                    {
                        _genStatus = Status.Sending;
                    }
                }
                else
                {
                    
                    _genStatus = Status.Sending;

                }
            }
            else if (_genStatus == Status.Sending)
            {
                
                Packet packet = new Packet();
                _packages[_packages.Count-1].addPacket(packet);
                
                
                points.Add(new PointXY(_tick, 1.25));
                points.Add(new PointXY(_tick + 0.33, 1));

                DisplayTick(gd, _tick, points);
                foreach (var pt in points)
                {
                    gd.generatorPoints.Add(pt);
                }
                points.Clear();

                _packetsInPackage--;

                if (_packetsInPackage > 0)
                {
                    _timeBetweenPackets = GenerateTBPt(); // GenerateTBPt
                    
                    if (gd.TbptStatistics.Find(x => x.position == _timeBetweenPackets) != null)
                    {
                        gd.TbptStatistics.Find(x => x.position == _timeBetweenPackets).CountUp();
                    }
                    else
                    {
                        gd.TbptStatistics.Add(new StatisticsPoint(_timeBetweenPackets, 1));
                    }
                    _genStatus = Status.WaitingForPacket;
                }
                else
                {
                    _packetsInPackage = 0;
                    _genStatus = Status.WaitingForPackage;
                    
                    points.Add(new PointXY(_tick, 0));
                    DisplayTick(gd, _tick, points);
                    foreach (var pt in points)
                    {
                        gd.generatorPoints.Add(pt);
                    }
                    points.Clear();

                    _timeBetweenPackages = GenerateTBP(); // GenerateTBP

                    if(gd.TbpStatistics.Find(x => x.position == _timeBetweenPackages)!=null){
                        gd.TbpStatistics.Find(x => x.position == _timeBetweenPackages).CountUp();
                    }
                    else
                    {
                        gd.TbpStatistics.Add(new StatisticsPoint(_timeBetweenPackages, 1));
                    }
                }
            }
            

        }

        private void DisplayTick(GeneratorDisplay gd, int tickNumber, List<PointXY> points)
        {
            int packetCount = 0;

            foreach (Package p in _packages.ToList())
                packetCount += p.getLength();

      /*      foreach (var p in points)
            {
                Debug.WriteLine("x: "+p.X+" y:"+ p.Y+  "  - "+ _genStatus.ToString()+"@Tick "+_tick);
            }
            */
            gd.DisplayTick(tickNumber, points, packetCount, _packages.Count);
        }

        private double GenerateTBP()
        {
            // Potrzeba skończonych funkcji dystrybucji.
            // 1 - Exp; 2 - Normalny; 3- Poisson; 4- Pareto; 5- Erlang
            if (_tBP_Distribution == 0)
                return Distributions.Poisson(Convert.ToInt32(_tBP_first));
            if (_tBP_Distribution == 1)
                return Distributions.Exponential(Convert.ToInt32(_tBP_first));
            if (_tBP_Distribution == 2)
                return Distributions.Normal(Convert.ToDouble(_tBP_first), Convert.ToDouble(_tBP_second));
            if (_tBP_Distribution == 3)
                return Distributions.Pareto(Convert.ToDouble(_tBP_first), Convert.ToDouble(_tBP_second));
            if (_tBP_Distribution == 4)
                return Distributions.Erlang(Convert.ToInt32(_tBP_first), Convert.ToDouble(_tBP_second));
            else
                return 0;
        }

        private double GeneratePIP()
        {
            // Potrzeba skończonych funkcji dystrybucji.
            // 1 - Exp; 2 - Normalny; 3- Poisson; 4- Pareto; 5- Erlang            
            if (_pIP_Distribution == 0)
                return Distributions.Poisson(Convert.ToInt32(_pIP_first));
            if (_pIP_Distribution == 1)
                return Distributions.Exponential(Convert.ToInt32(_pIP_first));
            if (_pIP_Distribution == 2)
                return Distributions.Normal(Convert.ToDouble(_pIP_first), Convert.ToDouble(_pIP_second));
            if (_pIP_Distribution == 3)
                return Distributions.Pareto(Convert.ToDouble(_pIP_first), Convert.ToDouble(_pIP_second));
            if (_pIP_Distribution == 4)
                return Distributions.Erlang(Convert.ToInt32(_pIP_first), Convert.ToDouble(_pIP_second));
            else
                return 0;
        }

        private double GenerateTBPt()
        {
            // Potrzeba skończonych funkcji dystrybucji.
            // 1 - Exp; 2 - Normalny; 3- Poisson; 4- Pareto; 5- Erlang
            if (_tBPt_Distribution == 0)
                return Distributions.Poisson(Convert.ToInt32(_tBPt_first));
            if (_tBPt_Distribution == 1)
                return Distributions.Exponential(Convert.ToInt32(_tBPt_first));
            if (_tBPt_Distribution == 2)
                return Distributions.Normal(Convert.ToDouble(_tBPt_first), Convert.ToDouble(_tBPt_second));
            if (_tBPt_Distribution == 3)
                return Distributions.Pareto(Convert.ToDouble(_tBPt_first), Convert.ToDouble(_tBPt_second));
            if (_tBPt_Distribution == 4)
                return Distributions.Erlang(Convert.ToInt32(_tBPt_first), Convert.ToDouble(_tBPt_second));
            else
                return 0;
        }

        public void StopEmulation(GeneratorDisplay gd)
        {
            needToStop = false;

            gd.stopEmulation();
        }

        public void StartEmulation()
        {
            needToStop = false;
        }

        public void needStop()
        {
            needToStop = true;
        }
    }
}

