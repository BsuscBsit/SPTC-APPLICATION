using SPTC_APPLICATION.Objects;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SPTC_APPLICATION.View
{
    /// <summary>
    /// Interaction logic for BackID.xaml
    /// </summary>
    public partial class BackID : Window
    {
        public BackID()
        {
            InitializeComponent();
        }
        public void Populate(Franchise franchise, General type)
        {
            if (type == General.OPERATOR)
            {
                string mi = (franchise.Operator.name.middlename.Length > 0) ? franchise.Operator.name.middlename[0].ToString() + ". " : "";
                lblName.Content = franchise.Operator.name.firstname + " " + mi + franchise.Operator.name.lastname;

                lblLicense.Content = franchise.licenceNO;
                lblBodyNum.Content = franchise.bodynumber;
                lblEmePer.Content = franchise.Operator.emergencyPerson;
                lblAddressBuilding.Content = franchise.Operator.address.addressline1;
                lblAddressStreet.Content = franchise.Operator.address.addressline2;
                lblContact.Content = franchise.Operator.emergencyContact;
                if (franchise.Operator.signature != null)
                {
                    imgSign.Source = franchise.Operator.signature.GetSource();
                }
            } else
            {
                string mi = (franchise.Driver_day.name.middlename.Length > 0) ? franchise.Driver_day.name.middlename[0].ToString() + ". " : "";
                lblName.Content = franchise.Driver_day.name.firstname + " " + mi + franchise.Driver_day.name.lastname;

                lblLicense.Content = franchise.licenceNO;
                lblBodyNum.Content = franchise.bodynumber;
                lblEmePer.Content = franchise.Driver_day.emergencyPerson;
                lblAddressBuilding.Content = franchise.Driver_day.address.addressline1;
                lblAddressStreet.Content = franchise.Driver_day.address.addressline2;
                lblContact.Content = franchise.Driver_day.emergencyContact;
                if (franchise.Driver_day.signature != null)
                {
                    imgSign.Source =franchise.Driver_day.signature.GetSource();
                }
            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            window.Width = Scaler.ConvertCentimetersToDIP(8.09, this);
            window.Height = Scaler.ConvertCentimetersToDIP(11.2, this);

            label1.FontSize = Scaler.ConvertCentimetersToDIP(0.39, this);
            label2.FontSize = Scaler.ConvertCentimetersToDIP(0.39, this);

            label3.FontSize = Scaler.ConvertCentimetersToDIP(0.49, this);
            label4.FontSize = Scaler.ConvertCentimetersToDIP(0.42, this);
            label5.FontSize = Scaler.ConvertCentimetersToDIP(0.42, this);

            label6.FontSize = Scaler.ConvertCentimetersToDIP(0.35, this);
            label7.FontSize = Scaler.ConvertCentimetersToDIP(0.39, this);
            label8.FontSize = Scaler.ConvertCentimetersToDIP(0.39, this);
            label9.FontSize = Scaler.ConvertCentimetersToDIP(0.35, this);

            lblLicense.FontSize = Scaler.ConvertCentimetersToDIP(0.42, this);
            lblXPDate.FontSize = Scaler.ConvertCentimetersToDIP(0.42, this);
            lblBodyNum.FontSize = Scaler.ConvertCentimetersToDIP(2.54, this);
            lblEmePer.FontSize = Scaler.ConvertCentimetersToDIP(0.35, this);
            lblAddressBuilding.FontSize = Scaler.ConvertCentimetersToDIP(0.35, this);
            lblAddressStreet.FontSize = Scaler.ConvertCentimetersToDIP(0.35, this);
            lblContact.FontSize = Scaler.ConvertCentimetersToDIP(0.39, this);
            lblName.FontSize = Scaler.ConvertCentimetersToDIP(0.39, this);

            imgSign.Height = Scaler.ConvertCentimetersToDIP(2.4, this);
            viewbox1.Height = Scaler.ConvertCentimetersToDIP(0.7, this);
        }



    }

}
