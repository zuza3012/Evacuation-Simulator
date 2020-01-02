using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;


namespace Ha {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        void evacuationWorker_DoWork(object sender, DoWorkEventArgs e) {
            EvacuationCalc(true, cells, panicParameter);
            evacuationWorker.ReportProgress(100);
        }

        void evacuationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            numberOfIterations++;
            for (int i = 1; i < cols - 1; i++) {
                for (int j = 1; j < rows - 1; j++) {
                    DrawSomething(i, j);
                    DrawLabel(i, j);
                }
            }
        }

        void evacuationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            MessageBox.Show("Evacuation completed succesfully." + "\n" + "Evacuation time: " + numberOfIterations + " iterations", "You are the real hero!");
        }

        // Drugi backgroundWorker do wielu symulacji "w tle" poprzez przycisk DoCrazyCalculations 

        void CalcWorker_DoWork(object sender, DoWorkEventArgs e) {
            double sum = 0;
            for (int i = 0; i < numberOfEvacuations; i++) {
                copyCells = cells.Select(s => s.ToArray()).ToArray();          // sprawdzalam, dziala ;)
                EvacuationCalc(false, copyCells, panicParameter);
                Console.WriteLine("Number of iterations: " + numberOfIterations);
                calcWorker.ReportProgress(100 * i / numberOfEvacuations);
                sum += (double)numberOfIterations;
            }

            averageTime = sum / numberOfEvacuations;
            calcWorker.ReportProgress(100);
        }

        void CalcWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            pop.pgBar.Value = e.ProgressPercentage;
        }

        void CalcWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            pop.Close();
            MessageBox.Show(numberOfEvacuations + " simulations completed! " + "\n" + "Average evacuation time: " + averageTime + " iterations", "You are the real hero!");
        }


        void panicEvacuationWorker_DoWork(object sender, DoWorkEventArgs e) {
            int k = (int)(1 / panicStep) + 1;
            double[,] data = new double[2, k];
            panicParameter = 0;
            numberOfEvacuations = 100;

            Popup pop = new Popup();
            pop.pgBar.Maximum = k;
            pop.Show();

            for (int j = 0; j < k; j++) {
                double sum = 0;
                for (int i = 0; i < numberOfEvacuations; i++) {
                    copyCells = cells.Select(s => s.ToArray()).ToArray();
                    EvacuationCalc(false, copyCells, panicParameter);
                    Console.WriteLine("Number of iterations: " + numberOfIterations);
                    calcWorker.ReportProgress(100 * i / numberOfEvacuations);
                    sum += (double)numberOfIterations;
                }
                averageTime = sum / numberOfEvacuations;

                data[0, j] = averageTime;
                data[1, j] = panicParameter;
                panicParameter += panicStep;
                pop.pgBar.Value = j;
            }
        }

        private void panicEvacuationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            pop.pgBar.Value = e.ProgressPercentage;
        }

        private void panicEvacuationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            pop.Close();
        }

    }
}