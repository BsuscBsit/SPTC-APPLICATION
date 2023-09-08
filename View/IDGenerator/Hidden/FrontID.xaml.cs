using SPTC_APPLICATION.Objects;

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
                string mi = (franchise.Operator.name.middlename.Length > 0) ? franchise.Operator.name.middlename[0].ToString() + ". " : "";
                lblName.Text = franchise.Operator.name.firstname + " " + mi + franchise.Operator.name.lastname;
                lblPosition.Text = type.ToString();
                if (franchise.Operator.image != null)
                {
                    imgID.Source = franchise.Operator.image.GetSource();
                }
            }
            else
            {
                string mi = (franchise.Driver_day.name.middlename.Length > 0) ? franchise.Driver_day.name.middlename[0].ToString() + ". " : "";
                lblName.Text = franchise.Driver_day.name.firstname + " " + mi + franchise.Driver_day.name.lastname;
                lblPosition.Text = "DRIVER";
                if (franchise.Driver_day.image != null)
                {
                    imgID.Source = franchise.Driver_day.image.GetSource();
                }
            }

        }



        private void window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            label1.FontSize = Scaler.PtToPx(9);
            label2.FontSize = Scaler.PtToPx(14);
            label3.FontSize = Scaler.PtToPx(14);
            label4.FontSize = Scaler.PtToPx(8);
            label5.FontSize = Scaler.PtToPx(8);

            label6.FontSize = Scaler.PtToPx(11);
            label7.FontSize = Scaler.PtToPx(9);

            lblRegNum.FontSize = Scaler.PtToPx(9);
            lblXPDate.FontSize = Scaler.PtToPx(11);
            lblChairman.FontSize = Scaler.PtToPx(11);

            viewbox2.Height = Scaler.InToDip(0.90);
            viewbox3.Height = Scaler.InToDip(0.32);

        }
    }
}
