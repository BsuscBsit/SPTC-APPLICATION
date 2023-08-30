using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SPTC_APPLICATION.Objects;

namespace SPTC_APPLICATION.View.Pages
{
    /// <summary>
    /// Interaction logic for MainBody.xaml
    /// </summary>
    public partial class MainBody : Window
    {

        public MainBody()
        {
            InitializeComponent();
            username.Content = AppState.USER.position.title.ToString();
        }

        private void imgClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            (new Test()).Show();
        }

        private void btnGererate_Click(object sender, RoutedEventArgs e)
        {

            (new PrintPreview()).Show();
        }
    }
}