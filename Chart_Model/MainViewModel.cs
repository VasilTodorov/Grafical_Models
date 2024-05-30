using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Grafica_Model;
using MathNet.Numerics;

namespace Chart_Model
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private SeriesCollection? _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection!; }
            set
            {
                _seriesCollection = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeriesCollection)));
            }
        }

        public Func<double, string> YFormatter { get; set; }

        public MainViewModel()
        {
            DataBase info = new DataBase();
            info.k1 = 0.01;  //infection rate of pray
            info.k2 = 1.6; //Hunting of pray
            info.k3 = 0.1; //Hunting of infected
            info.k4 = 0.1; //Growth of pray
            info.k5 = 0.1; //death from age in hunter
            info.k6 = 0; //death from age in infected
            info.k7 = 0; //death from age in pray
            info.ReCalculate();
            SeriesCollection = new SeriesCollection();

            // Example data
            List<double> xValues = new List<double>(Generate.LinearSpaced(DataBase.NUMBER_OF_POINTS, 0, DataBase.TIME_DURATION));
            //List<double> yValues = new List<double> { 0.2, -17.9333, -17.9333, -17.9333, -17.9333, -17.9333 };
            //List<double> yValues2 = new List<double> { 0.2, 17.9333, 66, 43, -17, -12 };

            LineSeries lineSeries = new LineSeries
            {
                Title = "Pray",
                Values = new ChartValues<double>(info.PrayData),
                PointGeometry = null,
            };

            LineSeries lineSeries2 = new LineSeries
            {
                Title = "Infected",
                Values = new ChartValues<double>(info.InfectedData),
                PointGeometry = null
            };

            LineSeries lineSeries3 = new LineSeries
            {
                Title = "Hunter",
                Values = new ChartValues<double>(info.HunterData),
                PointGeometry = null
            };
            SeriesCollection.Add(lineSeries);
            SeriesCollection.Add(lineSeries2);
            SeriesCollection.Add(lineSeries3);

            YFormatter = value => value.ToString("F2");
        }

    }
}
