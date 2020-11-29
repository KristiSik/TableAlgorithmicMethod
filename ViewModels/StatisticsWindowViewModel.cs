using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using Serilog;
using TableAlgorithmicMethod.DataAccess;
using TableAlgorithmicMethod.DataAccess.Models;
using TableAlgorithmicMethod.Models;

namespace TableAlgorithmicMethod.ViewModels
{
    public class StatisticsWindowViewModel : BindableBase
    {
        private readonly ColumnSeries _tableAlgorithmicMethodColumnSeries;
        private readonly ColumnSeries _classicMethodColumnSeries;
        private readonly List<Calculation> _statistics;

        public StatisticsWindowViewModel()
        {
            DataFormats = new List<DataFormatItem>
            {
                new DataFormatItem("[16-bit] W, X – fixed-point numbers", Helpers.Constants.W_X_FIXED_POINT_DATA_FORMAT_16BIT_IDENTIFIER),
                new DataFormatItem("[24-bit] W, X – fixed-point numbers", Helpers.Constants.W_X_FIXED_POINT_DATA_FORMAT_24BIT_IDENTIFIER),
                new DataFormatItem("[32-bit] W – floating-point numbers, X – fixed-point numbers", Helpers.Constants.W_FLOATING_POINT_X_FIXED_POINT_DATA_FORMAT_32BIT_IDENTIFIER),
                new DataFormatItem("[32-bit] W, X – floating-point numbers", Helpers.Constants.W_X_FLOATING_POINT_DATA_FORMAT_32BIT_IDENTIFIER),
            };

            _tableAlgorithmicMethodColumnSeries = new ColumnSeries
            {
                Title = "Table-algorithmic method",
                ////Fill = new SolidColorBrush(Color.FromRgb(0x9f, 0xc0, 0x8f)),
                Fill = new SolidColorBrush(Color.FromRgb(0x58, 0x50, 0x8d)),
                Values = new ChartValues<long>(),
                DataLabels = false,
                MaxColumnWidth = 15,
                MinHeight = 15,
            };
            _classicMethodColumnSeries = new ColumnSeries
            {
                Title = "Classic method",
                Fill = new SolidColorBrush(Color.FromRgb(0xff, 0xa6, 0x00)),
                ////Fill = new SolidColorBrush(Color.FromRgb(0xe2, 0x70, 0x76)),
                Values = new ChartValues<long>(),
                MaxColumnWidth = 15,
                DataLabels = false,
            };

            ChartSeries = new SeriesCollection
            {
                _tableAlgorithmicMethodColumnSeries,
                _classicMethodColumnSeries,
            };

            try
            {
                _statistics = ReadDataFromDatabase();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to read statistic from database");
            }
            SelectedDataFormatIdentifier = Helpers.Constants.W_X_FIXED_POINT_DATA_FORMAT_16BIT_IDENTIFIER;
        }

        public IReadOnlyList<DataFormatItem> DataFormats { get; private set; }

        private int _selectedDataFormatIdentifier;
        public int SelectedDataFormatIdentifier
        {
            get => _selectedDataFormatIdentifier;
            set
            {
                SetProperty(ref _selectedDataFormatIdentifier, value);
                UpdateStatisticChart();
            } 
        }

        public SeriesCollection ChartSeries { get; set; }

        public string[] ChartLabels { get; set; }

        private List<Calculation> ReadDataFromDatabase()
        {
            using var dbContext = new FileDbContext();
            dbContext.Database.EnsureCreated();
            return dbContext.Statistics.ToList();
        }

        private void UpdateStatisticChart()
        {
            ChartLabels = new string[30];

            _classicMethodColumnSeries.Values.Clear();
            _tableAlgorithmicMethodColumnSeries.Values.Clear();

            for (int i = 0; i < 30; i++)
            {
                ChartLabels[i] = (i + 1).ToString();
                var statistic = _statistics.FirstOrDefault(s => s.DataFormatId == SelectedDataFormatIdentifier && s.NumberOfElements == i + 1);
                _classicMethodColumnSeries.Values.Add(statistic?.ClassicMethodElapsedTicks ?? 0);
                _tableAlgorithmicMethodColumnSeries.Values.Add(statistic?.TableAlgorithmicMethodElapsedTicks ?? 0);
            }
        }
    }
}
