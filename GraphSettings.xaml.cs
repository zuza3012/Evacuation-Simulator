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

namespace Ha {
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class GraphSettings : Window {
        public GraphSettings() {
            InitializeComponent();
        }

        private void SetParameters(object sender, RoutedEventArgs e) {
            if (TbNumberOfEvacuations.Text != "0") {
                DialogResult = true;
            } else {
                MessageBox.Show("Number of evacuations shouldn't be equal to 0.", "Error");
            }
        }
    }

    
}
