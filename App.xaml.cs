using System.Windows;
using SPTC_APPLICATION.Objects;

namespace SPTC_APPLICATION
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            EventLogger.Post("Main :: Application Closed");
        }

    }
}
