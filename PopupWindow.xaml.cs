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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Popup : Window {
        public bool cancel = false;
        public Popup() {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e) {
            cancel = true;
        }
    }
}
