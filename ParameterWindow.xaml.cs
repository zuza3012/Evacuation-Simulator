using System.Windows;

namespace Ha {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ParameterWindow : Window {
        public ParameterWindow() {
            InitializeComponent();
        }

        private void SetParameters(object sender, RoutedEventArgs e) {
            if (MainWindow.CheckConvertion(panicParameterTB.Text,false)) {
                if (double.Parse(panicParameterTB.Text) < 1 && double.Parse(panicParameterTB.Text) > 0) {
                    DialogResult = true;
                } else {
                    MessageBox.Show("Panic parameter shouldn't be greater than 1 or less than 0.");
                }
            } else {
                MessageBox.Show("Don't.");
            }
        }
    }
}
