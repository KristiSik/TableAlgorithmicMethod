using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Serilog;
using TableAlgorithmicMethod.Helpers;
using TableAlgorithmicMethod.Models;
using TableAlgorithmicMethod.ScalarMultipliers;

namespace TableAlgorithmicMethod.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ColumnSeries _tableAlgorithmicMethodColumnSeries;
        private readonly ColumnSeries _classicMethodColumnSeries;
        private readonly IScalarMultiplier _classicScalarMultiplier;
        private readonly IScalarMultiplier _tableAlgorithmicScalarMultiplier;

        public MainWindowViewModel()
        {
            DataFormats = new List<DataFormatItem>
            {
                new DataFormatItem("[16-bit] W, X – fixed-point numbers", Constants.W_X_FIXED_POINT_DATA_FORMAT_16BIT_IDENTIFIER),
                new DataFormatItem("[24-bit] W, X – fixed-point numbers", Constants.W_X_FIXED_POINT_DATA_FORMAT_24BIT_IDENTIFIER),
                new DataFormatItem("[32-bit] W – floating-point numbers, X – fixed-point numbers", Constants.W_FLOATING_POINT_X_FIXED_POINT_DATA_FORMAT_32BIT_IDENTIFIER),
                new DataFormatItem("[32-bit] W, X – floating-point numbers", Constants.W_X_FLOATING_POINT_DATA_FORMAT_32BIT_IDENTIFIER),
            };

            _classicScalarMultiplier = new ClassicScalarMultiplier();
            _tableAlgorithmicScalarMultiplier = new TableAlgorithmicScalarMultiplier();

            CalculateCommand = new DelegateCommand(ExecuteCalculateCommand);

            _tableAlgorithmicMethodColumnSeries = new ColumnSeries
            {
                Title = "Table-algorithmic method",
                Values = new ChartValues<long> { 0 },
                DataLabels = true,
                MinHeight = 15,
            };
            _classicMethodColumnSeries = new ColumnSeries
            {
                Title = "Classic method",
                Values = new ChartValues<long> { 0 },
                DataLabels = true,
            };
            ChartSeries = new SeriesCollection
            {
                _tableAlgorithmicMethodColumnSeries,
                _classicMethodColumnSeries,
            };


            ChartLabels = new[] { "Elapsed ticks for calculation" };
        }

        private string _weights;
        public string Weights
        {
            get => _weights;
            set => SetProperty(ref _weights, value);
        }

        private string _inputs;
        public string Inputs
        {
            get => _inputs;
            set => SetProperty(ref _inputs, value);
        }

        private string _result;
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public IReadOnlyList<DataFormatItem> DataFormats { get; private set; }

        private int _selectedDataFormatIdentifier;
        public int SelectedDataFormatIdentifier
        {
            get => _selectedDataFormatIdentifier;
            set => SetProperty(ref _selectedDataFormatIdentifier, value);
        }

        public SeriesCollection ChartSeries { get; set; }

        public string[] ChartLabels { get; set; }

        public DelegateCommand CalculateCommand { get; }

        private void ExecuteCalculateCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(Weights) || string.IsNullOrEmpty(Inputs))
                {
                    throw new Exception("At least one weight and input value should be specified");
                }

                switch (SelectedDataFormatIdentifier)
                {
                    case Constants.W_X_FIXED_POINT_DATA_FORMAT_16BIT_IDENTIFIER:
                    case Constants.W_X_FIXED_POINT_DATA_FORMAT_24BIT_IDENTIFIER:
                        FixedPointNumberFormat fixedPointNumberFormat = SelectedDataFormatIdentifier == Constants.W_X_FIXED_POINT_DATA_FORMAT_16BIT_IDENTIFIER
                            ? FixedPointNumberFormat.Q15
                            : FixedPointNumberFormat.Q23;

                        List<int> weightsFx, inputsFx;
                        var fixedPointNumbersArithmeticOperations = new FixedPointNumbersArithmeticOperations(fixedPointNumberFormat);

                        try
                        {
                            weightsFx = ParseBinaryValues(SplitRowsIntoValues(Weights, fixedPointNumbersArithmeticOperations.NumberSize), fixedPointNumbersArithmeticOperations.NumberSize).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to read weights", ex);
                        }

                        try
                        {
                            inputsFx = ParseBinaryValues(SplitRowsIntoValues(Inputs, fixedPointNumbersArithmeticOperations.NumberSize), fixedPointNumbersArithmeticOperations.NumberSize).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to read input values", ex);
                        }

                        if (weightsFx.Count != inputsFx.Count)
                        {
                            throw new Exception("The amount of weights and input values should match");
                        }

                        Calculate(weightsFx, inputsFx, fixedPointNumbersArithmeticOperations);
                        break;

                    case Constants.W_X_FLOATING_POINT_DATA_FORMAT_32BIT_IDENTIFIER:
                        List<int> weightsFl, inputsFl;
                        var floatingPointNumbersArithmeticOperations = new FloatingPointNumbersArithmeticOperations();
                        try
                        {
                            weightsFl = ParseBinaryValues(SplitRowsIntoValues(Weights, floatingPointNumbersArithmeticOperations.NumberSize), floatingPointNumbersArithmeticOperations.NumberSize).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to read weights", ex);
                        }

                        try
                        {
                            inputsFl = ParseBinaryValues(SplitRowsIntoValues(Inputs, floatingPointNumbersArithmeticOperations.NumberSize), floatingPointNumbersArithmeticOperations.NumberSize).ToList();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Failed to read input values", ex);
                        }

                        if (weightsFl.Count != inputsFl.Count)
                        {
                            throw new Exception("The amount of weights and input values should match");
                        }

                        Calculate(weightsFl, inputsFl, floatingPointNumbersArithmeticOperations);
                        break;

                    default:
                        throw new Exception($"{SelectedDataFormatIdentifier} data format identifier is not supported");
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex?.InnerException?.Message != null)
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}Reason: {ex.InnerException.Message}";
                }

                Log.Error(ex, "Failed to calculate");
                MessageBox.Show(errorMessage, "Error");
            }
        }

        private void Calculate(List<int> weights, List<int> inputs, IArithmeticOperations arithmeticOperations)
        {
            ScalarMultiplicationResult classicMethodMultiplicationResult = _classicScalarMultiplier.Multiply(weights, inputs, arithmeticOperations);
            ScalarMultiplicationResult tableAlgorithmicMethodMultiplicationResult = _tableAlgorithmicScalarMultiplier.Multiply(weights, inputs, arithmeticOperations);
            ////Log.Information("Finished scalar multiplication of two vectors of fixed-point {Format} numbers in {Elapsed} ticks.", format, classicMethodMultiplicationResult.ElapsedTicks);
            Log.Information("{First} and {Second}", BinaryOperations.ToString(classicMethodMultiplicationResult.Value, arithmeticOperations.NumberSize), BinaryOperations.ToString(tableAlgorithmicMethodMultiplicationResult.Value, arithmeticOperations.NumberSize));

            Result = BinaryOperations.ToString(classicMethodMultiplicationResult.Value, arithmeticOperations.NumberSize);
            _tableAlgorithmicMethodColumnSeries.Values[0] = tableAlgorithmicMethodMultiplicationResult.ElapsedTicks;
            _classicMethodColumnSeries.Values[0] = classicMethodMultiplicationResult.ElapsedTicks;
        }

        private IEnumerable<int> ParseBinaryValues(IEnumerable<string> stringValues, int numberSize)
        {
            return stringValues.Select((s, i) =>
            {
                try
                {
                    return BinaryOperations.Parse(s, numberSize - 1);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to parse value '{s}' (at index {i})", ex);
                }
            });
        }

        private IEnumerable<string> SplitRowsIntoValues(string lines, int maxLength)
        {
            return lines.Split(Environment.NewLine)
                .Where(s => s != string.Empty)
                .Select((s, i) =>
                {
                    string trimmedValue = s.Trim();
                    return trimmedValue.Length > maxLength ? throw new Exception($"Value '{trimmedValue}' (at index {i}) exceeds the maximum length ({maxLength})") : trimmedValue;
                });
        }
    }
}