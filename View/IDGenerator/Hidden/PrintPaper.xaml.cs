using System.Windows;
using System.Windows.Controls;

namespace SPTC_APPLICATION.View.IDGenerator.Hidden
{
    /// <summary>
    /// Interaction logic for PrintPaper.xaml
    /// </summary>
    public partial class PrintPaper : Window
    {

        Border[] borders;
        public PrintPaper()
        {
            InitializeComponent();
            borders = new Border[4];
            borders[0] = brd1;
            borders[1] = brd2;
            borders[2] = brd3;
            borders[3] = brd4;

        }

        public bool StartPrint(ID[] arr, bool isFront)
        {
            if (isFront)
            {
                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i].Child = new Image();
                    if (arr[i] != null)
                    {
                        borders[i].Child = arr[i].RenderFrontID();
                    }
                }
            }
            else
            {

                for (int i = 0; i < borders.Length; i++)
                {
                    borders[i].Child = new Image();
                    if (arr[i] != null)
                    {

                        borders[i].Child = arr[i].RenderBackID();
                    }

                }
            }

            string page = (isFront) ? "Front" : "Back";

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(frontPage, "Printing : " + page + " page");
                this.Hide();
                ControlWindow.ShowDialog("Success", page + "page was printed successfully!" + ((isFront) ? "\nPress ok to print the next page." : ""), Icons.NOTIFY);
                return true;
            }
            else
            {
                ControlWindow.ShowDialog("Failed", page + " page was not printed.", Icons.ERROR);
                return false;
            }
        }
    }
}
