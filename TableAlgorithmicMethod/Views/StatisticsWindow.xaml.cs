using TableAlgorithmicMethod.ViewModels;

namespace TableAlgorithmicMethod.Views
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow
    {
        public StatisticsWindow()
        {
            InitializeComponent();
            DataContext = new StatisticsWindowViewModel();
        }
    }
}
