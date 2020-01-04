﻿using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ha {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Graph1 : Window {
        public SeriesCollection seriesCol { get; set; }
        List<double> xValues, yValues;
        ChartValues<ObservablePoint> ListOfPoints = new ChartValues<ObservablePoint>();
        public Graph1() {
            InitializeComponent();

            DrawChart(chart1, MainWindow.path, "The most mind - blowing Evacuation Graph", "average evacuation time", "panic parameter");

            // nowy wykres
            // tab2.isEnabled = true;

        }

        private List<double>[] ReadDataFromFile(string filePath) {
            List<double>[] dataArray = new List<double>[2];

            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();
            dataArray[0] = xValues;
            dataArray[1] = yValues;


            using (TextReader reader = File.OpenText(filePath)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    string[] bits = line.Split('\t');
                    double x = double.Parse(bits[0].Replace(",", "."), CultureInfo.InvariantCulture);
                    double y = double.Parse(bits[1].Replace(",", "."), CultureInfo.InvariantCulture);

                    xValues.Add(x);
                    yValues.Add(y);
                }
            }
            return dataArray;
        }

        private void SaveGraph_Click(object sender, RoutedEventArgs e) {

            int chartWidth = (int)chart1.ActualWidth;
            int chartHeight = (int)chart1.ActualHeight;
            double resolution = 96d;

            FileStream stream = new FileStream("graph.png", FileMode.Create);
            RenderTargetBitmap bmp = new RenderTargetBitmap(chartWidth, chartHeight, resolution, resolution, PixelFormats.Pbgra32);
            bmp.Render(chart1);

            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(stream);

            MessageBox.Show("Graph has been saved!", "Saving inforamtion");

        }

        private void DrawChart(CartesianChart chart, string filePath, string chartTitle, string xTitle, string yTitle) {
            xValues = ReadDataFromFile(filePath)[0];
            yValues = ReadDataFromFile(filePath)[1];


            for (int i = 0; i < xValues.Count; i++) {
                ListOfPoints.Add(new ObservablePoint {
                    X = xValues[i],
                    Y = yValues[i]
                });
            }

            seriesCol = new SeriesCollection {
                  new LineSeries{
                     Values = ListOfPoints,
                     PointGeometry = null, // DefaultGeometries.Square tylko jak jest malo punktuf, inzcaej kupcia
                     Title = chartTitle,
                     Fill = System.Windows.Media.Brushes.Transparent
                     
                   }
             };

            chart.AxisX.Add(
            new Axis {
                MinValue = xValues.Min(),
                Title = xTitle,
            });

            chart.AxisY.Add(
            new Axis {
                MinValue = 0,
                Title = yTitle,

            });

            chart.AxisX[0].Separator.StrokeThickness = 0;
            chart.AxisY[0].Separator.StrokeThickness = 0;
            chart.AxisY[0].Separator.IsEnabled = true;



            chart.DataContext = this;
        }

    }
}