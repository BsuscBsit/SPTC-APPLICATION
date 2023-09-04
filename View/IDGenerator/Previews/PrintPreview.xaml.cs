﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SPTC_APPLICATION.Objects;
using SPTC_APPLICATION.View.IDGenerator.Hidden;

namespace SPTC_APPLICATION.View
{
    /// <summary>
    /// Interaction logic for PrintPreview.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        private bool isFront;
        private static ID mGrid1 = null;
        private static ID mGrid2 = null;
        private static ID mGrid3 = null;
        private static ID mGrid4 = null;
        private static int idcount = 0;

        public PrintPreview()
        {
            InitializeComponent();
            isFront = true;
            RenderIDs();
            checkIdCount();
        }

        private void RenderIDs()
        {
            if (isFront)
            {
                if (mGrid1 != null)
                {
                    grid1.Children.Clear();
                    grid1.Children.Add(mGrid1.RenderFrontID());
                }
                else
                {
                    grid1.Children.Clear();
                    grid1.Children.Add(CreateNoIDImage());
                }

                if (mGrid2 != null)
                {
                    grid2.Children.Clear();
                    grid2.Children.Add(mGrid2.RenderFrontID());
                }
                else
                {
                    grid2.Children.Clear();
                    grid2.Children.Add(CreateNoIDImage());
                }

                if (mGrid3 != null)
                {
                    grid3.Children.Clear();
                    grid3.Children.Add(mGrid3.RenderFrontID());
                }
                else
                {
                    grid3.Children.Clear();
                    grid3.Children.Add(CreateNoIDImage());
                }

                if (mGrid4 != null)
                {
                    grid4.Children.Clear();
                    grid4.Children.Add(mGrid4.RenderFrontID());
                }
                else
                {
                    grid4.Children.Clear();
                    grid4.Children.Add(CreateNoIDImage());
                }
            }
            else
            {
                if (mGrid2 != null)
                {
                    grid1.Children.Clear();
                    grid1.Children.Add(mGrid2.RenderBackID());
                }
                else
                {
                    grid1.Children.Clear();
                    grid1.Children.Add(CreateNoIDImage());
                }

                if (mGrid1 != null)
                {
                    grid2.Children.Clear();
                    grid2.Children.Add(mGrid1.RenderBackID());
                }
                else
                {
                    grid2.Children.Clear();
                    grid2.Children.Add(CreateNoIDImage());
                }

                if (mGrid4 != null)
                {
                    grid3.Children.Clear();
                    grid3.Children.Add(mGrid4.RenderBackID());
                }
                else
                {
                    grid3.Children.Clear();
                    grid3.Children.Add(CreateNoIDImage());
                }

                if (mGrid3 != null)
                {
                    grid4.Children.Clear();
                    grid4.Children.Add(mGrid3.RenderBackID());
                }
                else
                {
                    grid4.Children.Clear();
                    grid4.Children.Add(CreateNoIDImage());
                }
            }
        }

        private System.Windows.Controls.Image CreateNoIDImage()
        {
            return new System.Windows.Controls.Image
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0),
                VerticalAlignment = VerticalAlignment.Center,
                Source = new BitmapImage(new Uri("/View/Images/no_id.png", UriKind.Relative)),
                Opacity = 0.2
            };
        }

        private void Button_expand(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Grid grid = FindVisualChild<Grid>(button);
            if (grid != null)
            {
                System.Windows.Controls.Image image = FindVisualChild<System.Windows.Controls.Image>(grid);
                if (image != null)
                {
                    if (image.Source is BitmapImage bitmapImage)
                    {
                        string imagePath = "/View/Images/no_id.png";
                        if (bitmapImage.UriSource.ToString() != imagePath)
                        {
                            preview.Source = image.Source;
                        }
                    }
                    else
                    {
                        preview.Source = image.Source;
                    }
                }
            }
        }

        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild)
                    return typedChild;

                T childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetPrintData();
            (new PrintPreview()).Show();
            this.Close();
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            if (idcount < 4)
            {
                (new GenerateID()).Show();
                this.Close();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

            if (idcount >= 1)
            {
                PrintPaper printpaper = new PrintPaper();
                if (printpaper.StartPrint(new ID[] { mGrid1, mGrid2, mGrid3, mGrid4 }, true))
                {
                    EventLogger.Post($"OUT :: Print Front page");
                    if (printpaper.StartPrint(new ID[] { mGrid2, mGrid1, mGrid4, mGrid3 }, false))
                    {
                        EventLogger.Post($"OUT :: Print Back page");
                        mGrid1?.SaveInfo();
                        mGrid2?.SaveInfo();
                        mGrid3?.SaveInfo();
                        mGrid4?.SaveInfo();

                        ResetPrintData();
                    }
                    else
                    {
                        EventLogger.Post($"OUT :: Print Back page FAILED");
                    }

                }
                else
                {
                    EventLogger.Post($"OUT :: Print Front and Back page FAILED");
                }

                printpaper.Show();
                printpaper.Close();
                RenderIDs();
            }

            checkIdCount();
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //App.Current.Shutdown();
        }

        private void checkIdCount()
        {
            if (idcount == 0)
            {
                Grid0Content.Visibility = Visibility.Visible;
                btnPrint.Visibility = Visibility.Hidden;
            }
            else
            {
                Grid0Content.Visibility = Visibility.Collapsed;
                btnPrint.Visibility = Visibility.Visible;
            }
        }

        private void ResetPrintData()
        {
            mGrid1 = null;
            mGrid2 = null;
            mGrid3 = null;
            mGrid4 = null;
            idcount = 0;
        }

        private void MySwitch_Back(object sender, RoutedEventArgs e)
        {
            // Switch is in the True state
            isFront = false;
            RenderIDs();
        }

        private void MySwitch_Front(object sender, RoutedEventArgs e)
        {
            // Switch is in the False state
            isFront = true;
            RenderIDs();
        }

        public void NewID(ID id)
        {

            switch (++idcount)
            {
                case 1:
                    mGrid1 = id;
                    btnAddNew.Visibility = Visibility.Visible;
                    break;
                case 2:
                    mGrid2 = id;
                    btnAddNew.Visibility = Visibility.Visible;
                    break;
                case 3:
                    mGrid3 = id;
                    btnAddNew.Visibility = Visibility.Visible;
                    break;
                case 4:
                    mGrid4 = id;
                    btnAddNew.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
            RenderIDs();
            checkIdCount();
        }

    }
}
