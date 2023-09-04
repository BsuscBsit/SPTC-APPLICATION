
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace SPTC_APPLICATION.View.Controls
{
    

public class DataGridHelper<T>
    {
        private DataGrid dataGrid;

        public DataGridHelper(DataGrid dataGrid)
        {
            this.dataGrid = dataGrid;
        }

        public void DesignGrid(List<T> items, List<ColumnConfiguration> columnConfigurations)
        {
            // Clear existing columns
            dataGrid.Columns.Clear();
            dataGrid.AutoGenerateColumns = false;
            // Define your column configurations here
            

            foreach (var config in columnConfigurations)
            {
                var column = new DataGridTextColumn
                {
                    Header = config.Header,
                    Binding = new System.Windows.Data.Binding(config.BindingPath),
                    Width = config.Width,
                    MaxWidth = config.MaxWidth,
                    CellStyle = new System.Windows.Style
                    {
                        Setters =
                    {
                        new Setter(System.Windows.Controls.Control.BackgroundProperty, config.BackgroundColor),
                        new Setter(System.Windows.Controls.Control.FontWeightProperty, config.FontWeight),
                        new Setter(System.Windows.Controls.Control.FontSizeProperty, config.FontSize)
                    }
                    }
                };

                dataGrid.Columns.Add(column);
            }

            // Set the data source
            dataGrid.ItemsSource = items;
        }
    }

    public class ColumnConfiguration
    {
        public string BindingPath { get; }
        public string Header { get; }
        public double Width { get; } = 100;
        public double MaxWidth { get; } = 200;
        public System.Windows.Media.Brush BackgroundColor { get; } = Brushes.Yellow;
        public FontWeight FontWeight { get; } = FontWeights.Bold;
        public double FontSize { get; } = 15;

        public ColumnConfiguration(string bindingPath, string header)
        {
            this.BindingPath = bindingPath;
            this.Header = header;
        }
        public ColumnConfiguration(string bindingPath, string header, double width, double maxWidth, System.Windows.Media.Brush backgroundColor, FontWeight fontWeight, double fontSize)
        {
            BindingPath = bindingPath;
            Header = header;
            Width = width;
            MaxWidth = maxWidth;
            BackgroundColor = backgroundColor;
            FontWeight = fontWeight;
            FontSize = fontSize;
        }
    }


}
