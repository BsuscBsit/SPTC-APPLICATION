using System.Windows;

namespace SPTC_APPLICATION.View
{
    /// <summary>
    /// Interaction logic for ControlWindow.xaml
    /// </summary>
    public partial class ControlWindow : Window
    {
        public ControlWindow()
        {
            InitializeComponent();
        }

        public static ControlWindow Show(string header, string content)
        {
            ControlWindow control = new ControlWindow();
            control.lblHeader.Content = header;
            control.lblContent.Content = content;
            control.Show();
            return control;
        }
        public static ControlWindow ShowDialog(string header, string content)
        {
            ControlWindow control = new ControlWindow();
            control.lblHeader.Content = header;
            control.lblContent.Content = content;
            control.ShowDialog();
            return control;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
