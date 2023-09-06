using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SPTC_APPLICATION.Database;
using SPTC_APPLICATION.Objects;
using SPTC_APPLICATION.View.Controls;

namespace SPTC_APPLICATION.View
{
    public partial class Test : System.Windows.Window
    {

        public Test()
        {
            InitializeComponent();
            EventLogger.Post("VIEW :: TEST Window");
            tabControl.SelectionChanged += TabControl_SelectionChanged;
        }

        //Test for VideoCamera
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            using (var capture = new VideoCapture(0))
            {
                Mat image = new Mat();
                while (true)
                {
                    capture.Read(image);
                    var bitmap = BitmapConverter.ToBitmap(image);
                    var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                        bitmap.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                    imageRender.Source = bitmapSource;
                    Cv2.WaitKey(1);
                }
            }*/
        }

        //Test for Signature pad

        //Test for List
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem is TabItem selectedTab)
            {
                if (selectedTab.Header.ToString() == "Test Login")
                {
                    UpdateLogin();
                }
                if (selectedTab.Header.ToString() == "Test List")
                {
                    UpdateList();
                }
            }

        }

        private void UpdateLogin()
        {
            lblName.Content = $"My Name is {AppState.USER.name.wholename}";
            lblAddress.Content = $"I am from {AppState.USER.address}";
            lblPosition.Content = $"I am a {AppState.USER.position}";
        }


        //SAMPLE ONLY, use DataTable on next Iteration
        //Problematic because of Objects
        private async void UpdateList()
        {
            List<Franchise> fetchedData = await Task.Run(() =>
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    List<Franchise> franchises = new List<Franchise>();
                    franchises.AddRange(Retrieve.GetData<Franchise>(Table.FRANCHISE, Select.ALL, Where.ALL_NOTDELETED));

                    return franchises;
                }
            });

            // Assuming you have a List<Franchise> fetchedData

            DataGrid dataGrid = new DataGrid();
            DataGridHelper<Franchise> dataGridHelper = new DataGridHelper<Franchise>(dataGrid);
            List<ColumnConfiguration> columnConfigurations = new List<ColumnConfiguration>
            {
                new ColumnConfiguration("bodynumber", "Body Number"),
                new ColumnConfiguration("licenceNO", "Plate Number"),
                new ColumnConfiguration("Operator", "Operator name"),
                new ColumnConfiguration("Driver_day", "Driver name", 55, 60, 150, Brushes.Gray, FontWeights.Bold, 12),

            };
            dataGridHelper.DesignGrid(fetchedData, columnConfigurations);

            DatagridList.Children.Add(dataGrid);


        }

        /*private void dgList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (dgList.SelectedIndex >= 0 && dgList.SelectedIndex != dgList.Items.Count - 1)
            {
                Franchise tmp = ((Franchise)dgList.SelectedItem);

                //tmpImage.Source = tmp.Operator?.image.GetSource();
            }
        }*/


        //PLANS ON EDITING FIELDS
        //first create a class that modifies
        //then attach a 
    }
}