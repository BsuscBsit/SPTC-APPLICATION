using System.Windows;
using SPTC_APPLICATION.Objects;

namespace SPTC_APPLICATION
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private int openWindowCount = 0;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IncrementOpenWindowCount();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            openWindowCount--;

            if (openWindowCount <= 0)
            {
                EventLogger.Post("Main :: Application Closed");
            }
        }
        public void IncrementOpenWindowCount()
        {
            openWindowCount++;
        }

    }
}
