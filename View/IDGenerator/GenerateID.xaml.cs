﻿using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Win32;
using SPTC_APPLICATION.Objects;

namespace SPTC_APPLICATION.View
{
    /// <summary>
    /// Interaction logic for GenerateID.xaml
    /// </summary>
    public partial class GenerateID : Window
    {
        BitmapSource lastCapturedImage = null;
        bool hasPhoto = false;
        bool hasSign = false;
        bool isDriver = true;
        private Franchise franchise;
        bool isUpdate = false;

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public GenerateID()
        {
            InitializeComponent();
            tboxAddressS.Text = AppState.DEFAULT_ADDRESSLINE2;
            bDay.SelectedDate = DateTime.Today;
            EventLogger.Post("VIEW :: ID GENERATE Window");
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            btnStartPad.IsEnabled = false;
            if (videoDevices.Count == 0)
            {
                EventLogger.Post("ERR :: No video devices found.");
                btnStartCam.IsEnabled = false;
                return;
            }
            franchise = new Franchise();
            isUpdate = false;
            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
        }

        public GenerateID(Franchise franchise, bool isDriver)
        {
            InitializeComponent();
            bDay.SelectedDate = DateTime.Today;
            this.franchise = franchise;
            isUpdate = true;
            EventLogger.Post("VIEW :: ID GENERATE Window id="+franchise.id);
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            btnStartPad.IsEnabled = false;
            if (videoDevices.Count == 0)
            {
                EventLogger.Post("ERR :: No video devices found.");
                btnStartCam.IsEnabled = false;
                return;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);

            this.isDriver = isDriver;
            MySwitch.Visibility = Visibility.Hidden;
            if (isDriver)
            {
                drvOrOprt.Content = "DRIVER";
                lblPhoto.Content = "Driver's Photo";
                lblsign.Content = "Driver's Signature";
                if (franchise.Driver_day != null)
                {
                    var drv = franchise.Driver_day;
                    if(drv.name != null)
                    {
                        cbGender.SelectedIndex = ((drv.name.prefix == "Mrs.") ? 1 : 0);
                        tboxFn.Text = drv.name.firstname;
                        tboxMn.Text = drv.name.middlename;
                        tboxLn.Text = drv.name.lastname;
                        //ADD suffix for Jr. Sr etc...
                    }
                    if(drv.address != null)
                    {
                        tboxAddressB.Text = drv.address.addressline1;
                        tboxAddressS.Text = drv.address.addressline2;
                    }
                    if(drv.image != null)
                    {
                        imgIDPic.Source = drv.image.GetSource();
                        hasPhoto = true;
                    }
                    if (drv.signature != null)
                    {
                        imgSignPic.Source = drv.signature.GetSource();
                        hasSign = true;
                    }
                    if(drv.birthday != null)
                    {
                        bDay.DisplayDate = drv.birthday;
                        bDay.DataContext = drv.birthday;
                        bDay.Text = drv.birthday.ToString();
                    }
                    tboxEmePer.Text = drv.emergencyPerson;
                    tboxPhone.Text = drv.emergencyContact;
                    tboxBnum.Text = franchise.bodynumber;
                    tboxLnum.Text = franchise.licenceNO;
                }
            } else
            {
                drvOrOprt.Content = "OPERATOR";
                lblPhoto.Content = "Operator's Photo";
                lblsign.Content = "Operator's Signature";
                if (franchise.Operator != null)
                {
                    var drv = franchise.Operator;
                    if (drv.name != null)
                    {
                        cbGender.SelectedIndex = ((drv.name.prefix == "Mrs.") ? 1 : 0);
                        tboxFn.Text = drv.name.firstname;
                        tboxMn.Text = drv.name.middlename;
                        tboxLn.Text = drv.name.lastname;
                        //ADD suffix for Jr. Sr etc...
                    }
                    if (drv.address != null)
                    {
                        tboxAddressB.Text = drv.address.addressline1;
                        tboxAddressS.Text = drv.address.addressline2;
                    }
                    if (drv.image != null)
                    {
                        imgIDPic.Source = drv.image.GetSource();
                        hasPhoto = true;
                    }
                    if (drv.signature != null)
                    {
                        imgSignPic.Source = drv.signature.GetSource();
                        hasSign = true;
                    }
                    if (drv.birthday != null)
                    {
                        bDay.DisplayDate = drv.birthday;
                        bDay.DataContext = drv.birthday;
                        bDay.Text = drv.birthday.ToString();
                    }
                    tboxEmePer.Text = drv.emergencyPerson;
                    tboxPhone.Text = drv.emergencyContact;
                    tboxBnum.Text = franchise.bodynumber;
                    tboxLnum.Text = franchise.licenceNO;
                }
            }


        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.StopCamera();
            base.OnClosing(e);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.StopCamera();
            PrintPreview print = new PrintPreview();
            print.Show();
            this.Close();
        }

        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            pbCameraOpen.Value = pbCameraOpen.Minimum;
        }

        private void MySwitch_Back(object sender, RoutedEventArgs e)
        {
                // Switch is in the True state
                isDriver = false;
                drvOrOprt.Content = "Create this ID for\nOperator.";
            
        }

        private void MySwitch_Front(object sender, RoutedEventArgs e)
        {
                // Switch is in the False state
                isDriver = true;
                drvOrOprt.Content = "Create this ID for\nDriver.";
            
        }

        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // This event is called for each new camera frame
                // Convert the frame to a BitmapSource and display it
                System.Drawing.Bitmap frame = (System.Drawing.Bitmap)eventArgs.Frame.Clone();

                // Convert to BitmapSource
                BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    frame.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                // Display the captured frame in imgIDPic
                imgIDPic.Source = bitmapSource;

                // Dispose of the frame to release resources
                frame.Dispose();
            });

        }

        private async void BtnStartCam_Click(object sender, RoutedEventArgs e)
        {
            if (videoSource != null)
            {
                if (!videoSource.IsRunning)
                {
                    try
                    {
                        btnStartCam.IsEnabled = false;
                        ((System.Windows.Controls.Button)sender).Content = "Capture Image";
                        pbCameraOpen.Visibility = Visibility.Visible;
                        pbCameraOpen.Value = pbCameraOpen.Minimum;
                        pbCameraOpen.IsIndeterminate = true;

                        if (videoSource != null) videoSource.Start();

                        //Short delay. nag lalag/crash minsan pag binigla eh.
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        btnStartCam.IsEnabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        pbCameraOpen.Visibility = Visibility.Hidden;
                    }
                }
                else
                {


                    if (lastCapturedImage != null)
                    {
                        imgIDPic.Source = lastCapturedImage;
                    }
                    hasPhoto = true;

                    this.StopCamera();
                    pbCameraOpen.Value = 100;
                    pbCameraOpen.IsIndeterminate = false;
                }
            }
        }

        private void btnBrowseIDPic_Click(object sender, RoutedEventArgs e)
        {
            this.StopCamera();
            var openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedFilePath);
                bitmapImage.EndInit();

                imgIDPic.Source = bitmapImage;
                hasPhoto = true;
            }
        }

        private void btnBrowseSignPic_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedFilePath);
                bitmapImage.EndInit();

                imgSignPic.Source = bitmapImage;
                hasSign = true;
            }
        }

        private void btnPreview_Click(object sender, RoutedEventArgs e)
        {
            if (tboxLnum.Text.Length > 0 && tboxFn.Text.Length > 0 && tboxLn.Text.Length > 0 && tboxAddressB.Text.Length > 0 && tboxAddressS.Text.Length > 0 && tboxEmePer.Text.Length > 0 && tboxPhone.Text.Length > 0)
            {
                GeneratedIDPreview print = new GeneratedIDPreview();
                print.ReturnControl(this);
                if (isDriver)
                {
                    Driver @obj = new Driver();
                    SPTC_APPLICATION.Objects.Image image = null;
                    SPTC_APPLICATION.Objects.Image sign = null;
                    if (hasPhoto)
                    {
                        image = new SPTC_APPLICATION.Objects.Image(imgIDPic.Source, $"Drv - {tboxFn.Text}");
                    }
                    if (hasSign)
                    {
                        sign = new SPTC_APPLICATION.Objects.Image(imgSignPic.Source, $"Sign  -{tboxFn.Text}");
                    }

                    string prefix = (cbGender.SelectedIndex == 0) ? "Mr." : "Mrs.";
                    if (isUpdate)
                    {
                        @obj = franchise.Driver_day;
                        Name name = @obj.name;
                        name.prefix = prefix;
                        name.firstname = tboxFn.Text;
                        name.middlename = tboxMn.Text;
                        name.lastname = tboxLn.Text;
                        Address address = @obj.address;
                        address.addressline1 = tboxAddressB.Text;
                        address.addressline2 = tboxAddressS.Text;
                        @obj.WriteInto(name, address, image, sign, "", (DateTime)bDay.SelectedDate, tboxEmePer.Text, tboxPhone.Text);
                    }
                    else
                    {
                        Name name = new Name(prefix, tboxFn.Text, tboxMn.Text, tboxLn.Text, "");
                        Address address = new Address(tboxAddressB.Text, tboxAddressS.Text);
                        @obj.WriteInto(name, address, image, sign, "", (DateTime)bDay.SelectedDate, tboxEmePer.Text, tboxPhone.Text);
                    }
                    
                    franchise.WriteInto(tboxBnum.Text, null, @obj, null, tboxLnum.Text);

                    ID id = new ID(franchise, Objects.General.DRIVER_DAY);
                    print.Save(id);
                    print.Show();
                    this.Hide();
                }
                else
                {
                    Operator @obj = new Operator();
                    SPTC_APPLICATION.Objects.Image image = null;
                    SPTC_APPLICATION.Objects.Image sign = null;
                    if (hasPhoto)
                    {
                        image = new SPTC_APPLICATION.Objects.Image(imgIDPic.Source, $"Drv - {tboxFn.Text}");
                    }
                    if (hasSign)
                    {
                        sign = new SPTC_APPLICATION.Objects.Image(imgSignPic.Source, $"Sign  -{tboxFn.Text}");
                    }
                    string prefix = (cbGender.SelectedIndex == 0) ? "Mr." : "Mrs.";
                    if (isUpdate)
                    {
                        @obj = franchise.Operator;
                        Name name = @obj.name;
                        name.prefix = prefix;
                        name.firstname = tboxFn.Text;
                        name.middlename = tboxMn.Text;
                        name.lastname = tboxLn.Text;
                        Address address = @obj.address;
                        address.addressline1 = tboxAddressB.Text;
                        address.addressline2 = tboxAddressS.Text;
                        @obj.WriteInto(name, address, image, sign, "", (DateTime)bDay.SelectedDate, tboxEmePer.Text, tboxPhone.Text);
                    }
                    else
                    {
                        Name name = new Name(prefix, tboxFn.Text, tboxMn.Text, tboxLn.Text, "");
                        Address address = new Address(tboxAddressB.Text, tboxAddressS.Text);
                        @obj.WriteInto(name, address, image, sign, "", (DateTime)bDay.SelectedDate, tboxEmePer.Text, tboxPhone.Text);
                    }

                    
                    franchise.WriteInto(tboxBnum.Text, @obj, null, null, tboxLnum.Text);

                    ID id = new ID(franchise, Objects.General.OPERATOR);
                    print.Save(id);
                    print.Show();
                    this.Hide();
                }

            }
            else
            {
                ControlWindow.ShowDialog("Input Fields incomplete!", "Missing some required inputs.");
            }
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource.NewFrame -= new NewFrameEventHandler(videoSource_NewFrame);
            }
        }

        public void StopCamera()
        {
            try
            {
                if (lastCapturedImage != null)
                {
                    imgIDPic.Source = lastCapturedImage;
                }
                hasPhoto = true;
                if (videoSource != null)
                {
                    if (videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();
                        videoSource.WaitForStop();
                        videoSource.NewFrame -= new NewFrameEventHandler(videoSource_NewFrame);
                        videoSource = null;
                    }
                }
            }catch(Exception e)
            {
                ControlWindow.ShowDialog("Camera Error", e.Message, Icons.ERROR);
            }
        }
    }
}
