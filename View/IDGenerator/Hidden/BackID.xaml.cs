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
                string mi = (franchise.LOperator.name.middlename.Length > 0) ? franchise.LOperator.name.middlename[0].ToString() + ". " : "";
                lblName.Content = franchise.LOperator.name.firstname + " " + mi + franchise.LOperator.name.lastname;

                lblLicense.Content = franchise.licenceNO;
                lblBodyNum.Content = franchise.bodynumber;
                lblEmePer.Content = franchise.LOperator.emergencyPerson;
                lblAddressBuilding.Content = franchise.LOperator.address.addressline1;
                lblAddressStreet.Content = franchise.LOperator.address.addressline2;
                lblContact.Content = franchise.LOperator.emergencyContact;
                if (franchise.LOperator.signature != null)
                {
                    imgSign.Source = franchise.LOperator.signature.GetSource();
                }
            } else
            {
                string mi = (franchise.lDriverDay.name.middlename.Length > 0) ? franchise.lDriverDay.name.middlename[0].ToString() + ". " : "";
                lblName.Content = franchise.lDriverDay.name.firstname + " " + mi + franchise.lDriverDay.name.lastname;

                lblLicense.Content = franchise.licenceNO;
                lblBodyNum.Content = franchise.bodynumber;
                lblEmePer.Content = franchise.lDriverDay.emergencyPerson;
                lblAddressBuilding.Content = franchise.lDriverDay.address.addressline1;
                lblAddressStreet.Content = franchise.lDriverDay.address.addressline2;
                lblContact.Content = franchise.lDriverDay.emergencyContact;
                if (franchise.lDriverDay.signature != null)
                {
                    imgSign.Source =franchise.lDriverDay.signature.GetSource();
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
