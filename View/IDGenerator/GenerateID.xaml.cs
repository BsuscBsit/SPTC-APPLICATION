using AForge.Video;
using AForge.Video.DirectShow;
using Microsoft.Win32;
using SPTC_APPLICATION.Objects;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public GenerateID()
        {
            InitializeComponent();
            EventLogger.Post("VIEW :: ID GENERATE Window");
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

            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (videoSource != null){
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
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
            drvOrOprt.Content = "OPERATOR";
            lblPhoto.Content = "Operator's Photo";
            lblsign.Content = "Operator's Signature";
        }

        private void MySwitch_Front(object sender, RoutedEventArgs e)
        {
            // Switch is in the False state
            isDriver = true;
            drvOrOprt.Content = "DRIVER";
            lblPhoto.Content = "Driver's Photo";
            lblsign.Content = "Driver's Signature";
        }

        private void InitializeCamera()
        {
           if(videoSource != null) videoSource.Start();
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

        private void BtnStartCam_Click(object sender, RoutedEventArgs e)
        {
            if (videoSource != null)
            {
                if (!videoSource.IsRunning)
                {
                    try
                    {
                        ((System.Windows.Controls.Button)sender).Content = "Capture Image";
                        pbCameraOpen.Visibility = Visibility.Visible;
                        pbCameraOpen.Value = pbCameraOpen.Minimum;
                        pbCameraOpen.IsIndeterminate = true;


                        InitializeCamera();

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

                    if (videoSource != null && videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();
                        videoSource.WaitForStop();
                        videoSource.NewFrame -= new NewFrameEventHandler(videoSource_NewFrame);
                    }
                    pbCameraOpen.Value = 100;
                    pbCameraOpen.IsIndeterminate = false;
                }
            }
        }

        private void btnBrowseIDPic_Click(object sender, RoutedEventArgs e)
        {
            if (videoSource != null)
            {
                if (videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                }
            }
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
                

                    string prefix = (cbGender.SelectedIndex == 0) ? "Mr." : "Mrs.";
                    Name name = new Name(prefix, tboxFn.Text, tboxMn.Text, tboxLn.Text, "");
                    Address address = new Address(tboxAddressB.Text, tboxAddressS.Text);
                    SPTC_APPLICATION.Objects.Image image = null;
                    SPTC_APPLICATION.Objects.Image sign = null;
                    if (hasPhoto)
                    {
                        image = new SPTC_APPLICATION.Objects.Image(imgIDPic.Source, $"Drv - {name.firstname}");
                    }
                    if (hasSign) 
                    {
                        sign = new SPTC_APPLICATION.Objects.Image(imgSignPic.Source, $"Sign  -{name.firstname}");
                    }


                    @obj.WriteInto(name, address, image, sign, "", bDay.DisplayDate, tboxEmePer.Text, tboxPhone.Text);


                    Franchise franchise = new Franchise();
                    franchise.WriteInto(tboxBnum.Text, null, @obj, null, tboxLnum.Text);

                    ID id = new ID(franchise, Objects.General.DRIVER_DAY);
                    print.Save(id);
                    print.Show();
                    this.Hide();
                }
                else
                {
                    Operator @obj = new Operator();
                    string prefix =(cbGender.SelectedIndex == 0) ? "Mr." : "Mrs.";
                    Name name = new Name(prefix, tboxFn.Text, tboxMn.Text, tboxLn.Text, "");
                    Address address = new Address(tboxAddressB.Text, tboxAddressS.Text);
                    SPTC_APPLICATION.Objects.Image image = null;
                    SPTC_APPLICATION.Objects.Image sign = null;
                    if (hasPhoto)
                    {
                        image = new SPTC_APPLICATION.Objects.Image(imgIDPic.Source, $"Optr - {name.firstname}");
                    }
                    if (hasSign)
                    {
                        sign = new SPTC_APPLICATION.Objects.Image(imgSignPic.Source, $"Sign - {name.firstname}");
                    }


                    @obj.WriteInto(name, address, image, sign, "", bDay.DisplayDate, tboxEmePer.Text, tboxPhone.Text);


                    Franchise franchise = new Franchise();
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

        }

        
    }
}
