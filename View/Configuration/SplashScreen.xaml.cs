using System.Windows;
using SPTC_APPLICATION.Objects;

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
