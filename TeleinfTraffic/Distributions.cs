using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleinfTraffic
{
    class Distributions
    {
        /// <summary>
        /// Funkcje dystrybucji.
        /// </summary>
        static Random random = new Random();

        public static double Exponential(double lambda)
        {
            //odwrócona dystrybuanta
            double rand1, variable;

            rand1 = random.NextDouble();

            //(-1/lambda)*ln(1-u)
            //variable = (-1 / lambda) * Math.Log(1 - rand1);
            variable = Math.Log(1 - rand1)/(-1*lambda);
            return Math.Round(variable,2);
        }

        public static double Normal(double mean, double stdev)
        {
            //transformacja boxa-mullera
            double rand1, rand2, variable;

            rand1 = random.NextDouble();
            rand2 = random.NextDouble();

            //ze wzoru
            variable = Math.Cos(2 * Math.PI * rand1) * Math.Sqrt((-1) * Math.Log(rand2));

            //zmieniamy na rozkład N(mean, stdev)
            variable = mean + stdev * variable;

            return Math.Round(variable,2);
        }

        public static double Poisson(int mean)
        {
            //ze skryptu - lambda ?
            double t, variable, rand1;

            t = 0;
            variable = 0;

            while (t <= mean)
            {
                rand1 = random.NextDouble();
                t = (t - Math.Log(rand1));
                variable++;
            }
            
            return Math.Round(variable,2);
        }
        public static double Pareto(double scale, double shape)
        {
            //odwrotnosc dystrybuanty b/((1-U)^(1/a)) b-scale, a-shape, U-random
            double rand1, variable;

            rand1 = random.NextDouble();

            //variable = scale / (Math.Pow(1 - rand1, 1 / shape));
            variable = (int)(scale / Math.Pow(1 - rand1, 1 / shape));

            return Math.Round(variable,2);
        }
        public static double Erlang(int rate, double shape)
        {
            //− log(U1 · . . . · Un)/λ przybliżone
            double expo = 1;

            for (int i = 1; i <= shape; i++)
            {
                expo *= (Exponential(rate)/100);
            }

            return (int)((-1) * shape * Math.Log(expo));
        }
    }
}
