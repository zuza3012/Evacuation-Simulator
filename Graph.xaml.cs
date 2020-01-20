using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Drawing;
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
    public partial class Graph : Window {
        public SeriesCollection seriesCol { get; set; }
        List<double> xValues, yValues;
        ChartValues<ObservablePoint> ListOfPoints = new ChartValues<ObservablePoint>();
        public Graph(int k) {
            InitializeComponent();
            if (k == 1) {

                DrawChart(chart1, MainWindow.path, "Relationship between evacuation time and panic parameter", "Panic parameter", "Average evacuation time");
            } else {
                DrawChart(chart1, MainWindow.path2, "Relationship between evacuation time and door's width", "Door's width", "Average evacuation time");
            }
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
            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                ControlToBmp(chart1, 900, 900).Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        public static Bitmap ControlToBmp(Visual target, double dpiX, double dpiY) {
            if (target == null) {
                return null;
            }
            // render control content
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            RenderTargetBitmap rtb = new RenderTargetBitmap((int)(bounds.Width * dpiX / 96.0),
                                                            (int)(bounds.Height * dpiY / 96.0),
                                                            dpiX,
                                                            dpiY,
                                                            PixelFormats.Pbgra32);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext ctx = dv.RenderOpen()) {
                VisualBrush vb = new VisualBrush(target);
                ctx.DrawRectangle(vb, null, new Rect(new System.Windows.Point(), bounds.Size));
            }
            rtb.Render(dv);

            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);
            return new Bitmap(stream);
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
                     LineSmoothness = 0,
                     PointGeometry = DefaultGeometries.Circle, // DefaultGeometries.Square tylko jak jest malo punktuf, inzcaej kupcia
                     
                     Title = chartTitle,
                     Fill = System.Windows.Media.Brushes.White

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
            chart.Background = System.Windows.Media.Brushes.White;
            chart.AxisX[0].Separator.StrokeThickness = 0;
            chart.AxisY[0].Separator.StrokeThickness = 0;
            chart.AxisY[0].Separator.IsEnabled = true;

            chart.DataContext = this;

        }

    }
}
