using System.Windows;
using TableAlgorithmicMethod.ViewModels;

namespace TableAlgorithmicMethod.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
