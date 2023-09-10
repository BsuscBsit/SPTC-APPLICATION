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

namespace SPTC_APPLICATION.View.IDGenerator.NewLayout
{
    /// <summary>
    /// Interaction logic for PrintPreview_Draft_.xaml
    /// </summary>
    public partial class PrintPreview_Draft_ : Window
    {
        //Zoom Variables
        private double zoomScale = 1.0;
        private Point panOrigin;
        private bool isPanning = false;

        public PrintPreview_Draft_()
        {
            InitializeComponent();

            //Zoom Constructors
            InitializePanning();
            zoomSlider.Value = zoomScale;
            UpdateZoomText();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MySwitch_Back(object sender, RoutedEventArgs e)
        {

        }

        private void MySwitch_Front(object sender, RoutedEventArgs e)
        {

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPage1_Click(object sender, RoutedEventArgs e)
        {
            lblPageNum.Content = "Page 1 of 2";
            btnPage1.IsEnabled = false;
            btnPage2.IsEnabled = true;
        }

        private void btnPage2_Click(object sender, RoutedEventArgs e)
        {
            lblPageNum.Content = "Page 2 of 2";
            btnPage1.IsEnabled = true;
            btnPage2.IsEnabled = false;
        }


        //Zoom Functionality
        private void InitializePanning()
        {
            scrollViewer.PreviewMouseDown += OnMouseDown;
            scrollViewer.PreviewMouseMove += OnMouseMove;
            scrollViewer.PreviewMouseUp += OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && zoomScale > 1)
            {
                var contentArea = new Rect(0, 0, scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);

                if (contentArea.Contains(e.GetPosition(scrollViewer)))
                {
                    isPanning = true;
                    panOrigin = e.GetPosition(scrollViewer);
                    Mouse.OverrideCursor = Cursors.Hand;
                    scrollViewer.CaptureMouse();
                }
            }
        }


        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning && scrollViewer.IsMouseCaptured)
            {
                Point currentPos = e.GetPosition(scrollViewer);
                double deltaX = currentPos.X - panOrigin.X;
                double deltaY = currentPos.Y - panOrigin.Y;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - deltaX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - deltaY);

                panOrigin = currentPos;
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isPanning)
            {
                isPanning = false;
                Mouse.OverrideCursor = Cursors.Arrow;
                scrollViewer.ReleaseMouseCapture();
            }
        }

        private void UpdateZoom(double newZoom)
        {
            zoomScale = Math.Max(0.1, newZoom);
            zoomSlider.Value = zoomScale;
            UpdateZoomTransform();
        }

        private void UpdateZoomTransform()
        {
            ScaleTransform scaleTransform = new ScaleTransform(zoomScale, zoomScale);
            zoomableGrid.LayoutTransform = scaleTransform;
            UpdateZoomText();
        }

        private void UpdateZoomText()
        {
            if (zoomTextBlock != null)
            {
                zoomTextBlock.Text = $"Zoom: {Math.Round(zoomScale * 100)}%";
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoom(zoomScale + 0.1);
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoom(zoomScale - 0.1);
        }

        private void ResetZoom_Click(object sender, RoutedEventArgs e)
        {
            UpdateZoom(1.0);
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateZoom(zoomSlider.Value);
        }
        
    }
}
