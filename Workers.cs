using System;
using System.ComponentModel;
using System.Windows;


namespace Ha {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        void evacuationWorker_DoWork(object sender, DoWorkEventArgs e) {
            numberOfIterations = 0;
            EvacuationCalc(true, cells, panicParameter);
            evacuationWorker.ReportProgress(100);
        }

        void evacuationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            for (int i = 1; i < cols - 1; i++) {
                for (int j = 1; j < rows - 1; j++) {
                    DrawSomething(i, j);
                    DrawLabel(i, j);
                }
            }
        }

        void evacuationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            MessageBox.Show("Evacuation completed succesfully." + "\n" + "Evacuation time: " + numberOfIterations + " iterations", "You are the real hero!");
            cells = Cell.DeepCopy(TheMostImportantCopyOfCells);

            System.Threading.Thread.Sleep(500);
            for (int i = 1; i < cols - 1; i++) {                //wracamy do ulozenia poczatkowego
                for (int j = 1; j < rows - 1; j++) {
                    DrawSomething(i, j);
                    DrawLabel(i, j);
                }
            }
        }

        // Drugi backgroundWorker do wielu symulacji "w tle" poprzez przycisk Simulate N Evacuations 

        void CalcWorker_DoWork(object sender, DoWorkEventArgs e) {
            double sum = 0;
            numberOfIterations = 0;
            for (int i = 0; i < numberOfEvacuations; i++) {
                Cell[][] copyCells = Cell.DeepCopy(cells);
                numberOfIterations = 0;
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
            pop.Hide();
            MessageBox.Show(numberOfEvacuations + " simulations completed! " + "\n" + "Average evacuation time: " + averageTime + " iterations", "You are the real hero!");   
        }



        void panicEvacuationWorker_DoWork(object sender, DoWorkEventArgs e) {
            int k = (int)(1 / panicStep);
            numberOfSimulations = k;
            data = new double[2, k];
            panicParameter = 0;

            for (int j = 0; j < k; j++) {
                double sum = 0;
                //Console.WriteLine("Kupa");
                for (int i = 0; i < numberOfEvacuations; i++) {
                    Cell[][] copyCells = Cell.DeepCopy(cells);
                    numberOfIterations = 0;
                    EvacuationCalc(false, copyCells, panicParameter);
                    Console.WriteLine("Number of iterations: " + numberOfIterations);
                    sum += (double)numberOfIterations;
                }
                averageTime = sum / numberOfEvacuations;

                data[0, j] = averageTime;
                data[1, j] = panicParameter;
                panicParameter += panicStep;
                panicEvacuationWorker.ReportProgress(100 * j / k);
                /*Console.WriteLine("Dane:");
                for (int z = 0; z < k; z++) {
                    Console.Write(data[0, z] + "\t" + data[1, z]);
                    Console.WriteLine();
                }*/
                
            }
            panicEvacuationWorker.ReportProgress(100);
           
        }

        private void panicEvacuationWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            pop.pgBar.Value = e.ProgressPercentage;
        }

        private void panicEvacuationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            //tutaj wstawic okienko z wykresem albo moze zapisywanie wykresu do pliku, cokolwiek 
            pop.Hide();
            MessageBox.Show(numberOfSimulations + " simulations completed! " + "\n" + "I will save data and Fancy Graph", "You are the real hero!");
            string fileName = DateTime.Now.ToString("h/mm/ss_tt");
            path = SaveArrayToFile("data" + fileName + ".txt",data,"Simulations saved") + @"\" + "data" + fileName + ".txt";

            Graph1 graph1 = new Graph1();
            graph1.Show();
        }

    }
}