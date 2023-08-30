using SPTC_APPLICATION.Objects;
using System.Windows;

namespace SPTC_APPLICATION.View
{
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
            Controller.StartInitialization(this, pbLoading, tbDebugLog);
        }



    }
}
