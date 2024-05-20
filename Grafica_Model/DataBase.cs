using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.OdeSolvers;

namespace Grafica_Model
{
    public class DataBase
    {
        #region HiddenData
        double _k1;
        double _k2;
        double _k3;
        double _k4;
        double _k5;
        double _k6;
        double _k7;
        double _P0;
        double _I0;
        double _H0;
        #endregion
        #region ModelParameters
        /// <summary>
        /// Infection rate of pray
        /// </summary>
        public double k1
        {
            get { return _k1; }
            set { _k1 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Hunting rate of healty pray and growth in population of huntrs
        /// </summary>
        public double k2
        {
            get { return _k2; }
            set { _k2 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Hunting rate of infected pray and growth in population of huntrs
        /// </summary>
        public double k3
        {
            get { return _k3; }
            set { _k3 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Growth rate of pray population
        /// </summary>
        public double k4
        {
            get { return _k4; }
            set { _k4 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Death from age in hunters
        /// </summary>
        public double k5
        {
            get { return _k5; }
            set { _k5 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Death from age in infected
        /// </summary>
        public double k6
        {
            get { return _k6; }
            set { _k6 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Death from age in pray
        /// </summary>
        public double k7
        {
            get { return _k7; }
            set { _k7 = value > 0 ? value : 0; }
        }
        #endregion
        #region InitialParameters
        /// <summary>
        /// Healty pray population initial value
        /// </summary>
        public double P0
        {
            get { return _P0; }
            set { _P0 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Infected pray population initial value 
        /// </summary>
        public double I0
        {
            get { return _I0; }
            set { _I0 = value > 0 ? value : 0; }
        }
        /// <summary>
        /// Hunters population initial value
        /// </summary>
        public double H0
        {
            get { return _H0; }
            set { _H0 = value > 0 ? value : 0; }
        }
        #endregion
        #region ReadOnlyData
        /// <summary>
        /// Number of key points
        /// </summary>
        public const int NUMBER_OF_POINTS = 800;
        public const int TIME_DURATION = 2500;
        /// <summary>
        /// Statistic about Pray
        /// </summary>
        public double[] PrayData { get; private set; }
        /// <summary>
        /// Statistic about Pray
        /// </summary>
        public double[] InfectedData { get; private set; }
        /// <summary>
        /// Statistic about Pray
        /// </summary>
        public double[] HunterData { get; private set; }
        #endregion
        public DataBase()
        {
            // Model parameters
            k1 = 0.3;    // Infection rate of pray
            k2 = 0.1;    // Hunting rate of healty pray and growth in population of huntrs
            k3 = 0.1;    // Hunting rate of infected pray and growth in population of huntrs
            k4 = 0.4;    // Growth rate of pray population
            k5 = 0.2;    // Death from age in hunters
            k6 = 0.2;    // Death from age in infected
            k7 = 0.2;    // Death from age in pray

            // Initial conditions
            P0 = 0.8;    //Healty pray population initial value
            I0 = 0.2;    //Infected pray population initial value       
            H0 = 0.1;    //Hunters population initial value

            PrayData = new double[NUMBER_OF_POINTS];
            InfectedData = new double[NUMBER_OF_POINTS];
            HunterData = new double[NUMBER_OF_POINTS];

            ReCalculate();
        }

        public void ReCalculate()
        {
            //double[] y0 = { S0, I0, R0, H0 };
            double[] y0Array = { P0, I0, H0 };

            // Convert initial conditions to DenseVector
            Vector<double> y0 = DenseVector.OfArray(y0Array);

            // Time interval
            double[] t = Generate.LinearSpaced(NUMBER_OF_POINTS, 0, TIME_DURATION); // 1000 points from 0 to 200


            // Simulate the model for different values of predator efficiency in capturing infected prey

            // Define the differential equations
            var odeFunction = (double t, Vector<double> y) =>
            {
                double P = Math.Max(y[0], 0); // Ensure non-negative values
                double I = Math.Max(y[1], 0);
                double H = Math.Max(y[2], 0);


                double dPdt = k4 * P - k1 * P * I - k2 * P * H -k7*P;
                double dIdt = k1 * P * I - k3 * H * I - k6*I ;
                double dHdt = k2 * H * P + k3 * H * I - k5 * H;

                return DenseVector.OfArray(new double[] { dPdt, dIdt, dHdt });
            };

            // Solve the differential equations using the built-in solver
            //var solver = RungeKutta.FourthOrder(odeFunction, 0, DenseVector.OfArray(y0), t);
            var solver = RungeKutta.FourthOrder(y0, 0, TIME_DURATION, NUMBER_OF_POINTS, odeFunction);

            //solver[0] - vector.
            // Extract and print the results
            Console.WriteLine($"Time    Number of Healty Pray  Number of Infected Pray   Number of Hunters   ");
            for (int i = 0; i < t.Length; i++)
            {
                PrayData[i] = Math.Max(solver[i][0], 0);
                InfectedData[i] = Math.Max(solver[i][1], 0);
                HunterData[i] = Math.Max(solver[i][2], 0);

                //double pray = Math.Max(solver[i][0], 0);
                //double infected = Math.Max(solver[i][1], 0); // Ensure non-negative values
                //double hunters = Math.Max(solver[i][2], 0);
                //Console.WriteLine($"{t[i]:F2}   {pray:F6}       {infected:F4}       {hunters:F4}");
            }
            //Console.WriteLine();
        }

    }

}
