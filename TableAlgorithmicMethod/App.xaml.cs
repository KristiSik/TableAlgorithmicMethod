using System.Windows;
using Serilog;

namespace TableAlgorithmicMethod
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs.log")
                .CreateLogger();
        }
    }
}
