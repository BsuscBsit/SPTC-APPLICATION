using SPTC_APPLICATION.Objects;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SPTC_APPLICATION.View
{
    /// <summary>
    /// Interaction logic for Pages.xaml
    /// </summary>
    public partial class FrontID : System.Windows.Window
    {
        public FrontID()
        {
            InitializeComponent();
        }
        public void Populate(Franchise franchise, General type)
        {

            lblXPDate.Content = AppState.EXPIRATION_DATE;
            lblChairman.Content = AppState.CHAIRMAN;
            lblRegNum.Content = AppState.REGISTRATION_NO;

            if (type == General.OPERATOR)
            {
                string mi = (franchise.LOperator.name.middlename.Length > 0) ? franchise.LOperator.name.middlename[0].ToString() + ". " : "";
                lblName.Text = franchise.LOperator.name.firstname + " " + mi + franchise.LOperator.name.lastname;
                lblPosition.Text = type.ToString();
                if (franchise.LOperator.image != null)
                {
                    imgID.Source = franchise.LOperator.image.GetSource();
                }
            } else
            {
                string mi = (franchise.lDriverDay.name.middlename.Length > 0) ? franchise.lDriverDay.name.middlename[0].ToString() + ". " : "";
                lblName.Text = franchise.lDriverDay.name.firstname + " " + mi + franchise.lDriverDay.name.lastname;
                lblPosition.Text = "DRIVER";
                if (franchise.lDriverDay.image != null)
                {
                    imgID.Source = franchise.lDriverDay.image.GetSource();
                }
            }

        }



        private void window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            /*
            8 pt - 0.28 cm
            9 pt - 0.32 cm
            10 pt - 0.35 cm
            11 pt - 0.39 cm
            12 pt - 0.42 cm
            14 pt - 0.49 cm
            16 pt - 0.56 cm
            18 pt - 0.63 cm
            20 pt - 0.70 cm
            22 pt - 0.77 cm
            24 pt - 0.84 cm
            26 pt - 0.91 cm
            28 pt - 0.98 cm
            36 pt - 1.26 cm
            48 pt - 1.68 cm
            72 pt - 2.54 cm
            */
            window.Width = Scaler.ConvertCentimetersToDIP(8.09, this);
            window.Height = Scaler.ConvertCentimetersToDIP(11.2, this);

            label1.FontSize = Scaler.ConvertCentimetersToDIP(0.32, this);
            label2.FontSize = Scaler.ConvertCentimetersToDIP(0.49, this);
            label3.FontSize = Scaler.ConvertCentimetersToDIP(0.49, this);
            label4.FontSize = Scaler.ConvertCentimetersToDIP(0.28, this);
            label5.FontSize = Scaler.ConvertCentimetersToDIP(0.28, this);

            label6.FontSize = Scaler.ConvertCentimetersToDIP(0.32, this);
            label7.FontSize = Scaler.ConvertCentimetersToDIP(0.32, this);

            lblRegNum.FontSize = Scaler.ConvertCentimetersToDIP(0.32, this);
            lblXPDate.FontSize = Scaler.ConvertCentimetersToDIP(0.32, this);
            lblChairman.FontSize = Scaler.ConvertCentimetersToDIP(0.32, this);

            viewbox2.Height = Scaler.ConvertCentimetersToDIP(2.5, this);
            viewbox3.Height = Scaler.ConvertCentimetersToDIP(0.90, this);

        }
    }
}
