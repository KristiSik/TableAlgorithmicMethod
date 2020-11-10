using System.Collections.Generic;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using TableAlgorithmicMethod.Helpers;
using TableAlgorithmicMethod.Models;

namespace TableAlgorithmicMethod.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            Result = "0101010010101";

            DataFormats = new List<DataFormatItem>
            {
                new DataFormatItem("[16-bit] W, X – числа з фіксованою комою", Constants.W_X_FIXED_POINT_DATA_FORMAT_16BIT_IDENTIFIER),
                new DataFormatItem("[24-bit] W, X – числа з фіксованою комою", Constants.W_X_FIXED_POINT_DATA_FORMAT_24BIT_IDENTIFIER),
                new DataFormatItem("[32-bit] W – числа з плаваючою комою, X – числа з фіксованою комою", Constants.W_FLOATING_POINT_X_FIXED_POINT_DATA_FORMAT_32BIT_IDENTIFIER),
                new DataFormatItem("[32-bit] W, X – числа з плаваючою комою", Constants.W_X_FLOATING_POINT_DATA_FORMAT_32BIT_IDENTIFIER),
            };

            CalculateCommand = new DelegateCommand(ExecuteCalculateCommand);
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

        public DelegateCommand CalculateCommand { get; }

        private void ExecuteCalculateCommand()
        {
            MessageBox.Show("Calculate button pressed.");
        }
    }
}