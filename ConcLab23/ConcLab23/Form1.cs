using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConcLab23
{


    public partial class Form1 : Form
    {
        public class DataEntry{
            public double[] attributes;
            public int EntryClass;
            public DataEntry(string[] input) {
                attributes = new double[input.Length - 1];
                for (int i = 0; i < input.Length - 1; i++) {
                    double.TryParse(input[i],out attributes[i]);

                }
              int.TryParse(input[input.Length - 1],out EntryClass);
               
            }
        }

        /*    public class Rule {
                int ParamNum;
                int Threshold;
                public bool CorrespondsRight(int value) { return value > Threshold; }
                public bool CorrespondsLeft(int value) { return value <= Threshold; }
            }

            public class Tree {
                public Tree Left;
                public Tree Right;
                public Rule rule;
            }*/

        /*  public double Gini(DataEntry[] DataT) {
              int icount = 0;
              double p;
              int[] classes;
              double res=1;
              classes = DataT.Select((entry) => entry.EntryClass).Distinct().ToArray<int>();

              foreach (var item in classes)
              {
                  icount = DataT.Count(x => x.EntryClass == item);
                  p = Math.Pow(icount / DataT.Count(),2);
                  res = res - p;
              }
              return res;

          }*/

        public string datainfo="";
        public DataEntry[] TrainingData;
        public List<DataEntry> TestData;
        public void ReadData(string path, int rowNumber) {
            String[] row;
            int count4=0;
            int count2=0;
          
           // var rows = File.ReadLines(path, System.Text.Encoding.Default);
            List<string> rows = new List<string>();

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                while (((line = sr.ReadLine()) != null) && rowNumber>0)
                {
                    rows.Add(line);
                    rowNumber--;
                }
                if (rowNumber > 0) {             
                    MessageBox.Show("Количество строк, указанное вами больше, чем размер файла. Считан весь файл", "Предупреждение", MessageBoxButtons.OK);
                }
                }



                TrainingData = new DataEntry[rows.Count()/2];
            TestData = new List<DataEntry>();
         for (int i = 0;i<rows.Count()/2;i++) { 
                row = rows.ElementAt(i).Split(',');
                TrainingData[i] = new DataEntry(row);


                if (TrainingData[i].EntryClass == 2)
                {
                    count2++;

                }

                if (TrainingData[i].EntryClass == 4)
                {
                    count4++;

                }
           

            }
            datainfo = datainfo + "Training: 2-" + count2 + ";4-" + count4+ Environment.NewLine;
            count2 = 0;
            count4 = 0;
            for (int i = rows.Count() / 2; i < rows.Count() ; i++)
            {
                row = rows.ElementAt(i).Split(',');
                TestData.Add( new DataEntry(row));
                if (TestData.Last().EntryClass == 2) {
                    count2++;

                }

                if (TestData.Last().EntryClass == 4)
                {
                    count4++;

                }
            }

            datainfo = datainfo + "Test: 2-" + count2 + ";4-" + count4;
            count2 = 0;
            count4 = 0;



        }

        public double Distance(DataEntry a, DataEntry b) {
            double res=0;
            for (int i = 0; i < a.attributes.Length; i++) {
                res = res + Math.Pow((a.attributes[i] - b.attributes[i]), 2);
            }
            res = Math.Sqrt(res);
            return res;

        }

        public double vote_fun(DataEntry[] neighbours, int DataClass, DataEntry x) {
            double res = 0;

            foreach (var item in neighbours.Where(entry=>entry.EntryClass==DataClass))
            {
                res = res + 1/Math.Pow(Distance(x, item), 2);
            }
            return res;
      }

        public void Normalize() {
            double[] mins = new double[TrainingData[0].attributes.Length];
            double[] maxs = new double[TrainingData[0].attributes.Length];
            for (int i = 0; i < mins.Length; i++) {
                mins[i] = TrainingData.Min(x => x.attributes[i]);
                maxs[i] = TrainingData.Max(x => x.attributes[i]);
            }

          for (int j=0;j<TrainingData.Length;j++)
            {
                for (int i = 0; i < TrainingData[j].attributes.Length; i++)
                {
                    TrainingData[j].attributes[i] = (TrainingData[j].attributes[i] - mins[i]) / (maxs[i] - mins[i]);
                }
            }

        }

        public void NormalizeTest()
        {
            double[] mins = new double[TestData[0].attributes.Length];
            double[] maxs = new double[TestData[0].attributes.Length];
            for (int i = 0; i < mins.Length; i++)
            {
                mins[i] = TestData.Min(x => x.attributes[i]);
                maxs[i] = TestData.Max(x => x.attributes[i]);
            }

            for (int j = 0; j < TestData.Count; j++)
            {
                for (int i = 0; i < TestData[j].attributes.Length; i++)
                {
                    TestData[j].attributes[i] = (TestData[j].attributes[i] - mins[i]) / (maxs[i] - mins[i]);
                }
            }

        }

        public int KNN(int k, DataEntry o) {
            DataEntry[] neighbours = TrainingData.OrderBy(x => Distance(x, o)).
                                          Take(k).
                                          ToArray();
            int MaxClass = 0;
            double MaxVoteRes = 0;
            double tmp;
            foreach (var item in neighbours.Select(x=>x.EntryClass).Distinct())
            {
                tmp = vote_fun(neighbours, item, o);
                if (tmp > MaxVoteRes) {
                    MaxClass = item;
                    MaxVoteRes = tmp;

                }
            }

            return MaxClass;
        }




        public int KNNParallel(int k, DataEntry o)
        {

    

            DataEntry[] neighbours = TrainingData.AsParallel().OrderBy(x => Distance(x, o)).
                                          Take(k).
                                          ToArray();
            int MaxClass = 0;
            double MaxVoteRes = 0;
            double tmp;
            foreach (var item in neighbours.Select(x => x.EntryClass).Distinct())
            {
                tmp = vote_fun(neighbours, item, o);
                if (tmp > MaxVoteRes)
                {
                    MaxClass = item;
                    MaxVoteRes = tmp;

                }
            }

            return MaxClass;
        }

        public Form1()
        {
            InitializeComponent();

        }
 


        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
       
            
                int[] usstate = (int[])e.UserState;
                tbRes.AppendText(usstate[2] + ") Настоящий класс " + usstate[0] + "Предсказанный " + usstate[1] + Environment.NewLine);


            
      
        }
      public  int TP = 0;
        public int TN = 0;
        public int FP = 0;
        public  int FN = 0;


        public int TPP = 0;
        public int TNP = 0;
        public int FPP = 0;
        public int FNP = 0;

        DateTime dt1, dt2;
        double lAllTime = 0;
        DateTime dt1p, dt2p;
        double lAllTimep = 0;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            lAllTime = 0;
            dt1 = DateTime.Now;
            for (int i = 0; i < TestData.Count(); i++)
            {
                if (backgroundWorker1.CancellationPending) break;
                int a = TestData[i].EntryClass;
                int b = KNN(4, TestData[i]);
              


                if (a == 4 & b == 4) TP++;
                if (a == 2 & b == 2) TN++;
                if (a == 2 & b == 4) FP++;
                if (a == 4 & b == 2) FN++;
                int []userstate = new int [3];
                userstate[0] = a;
                userstate[1] = b;
                userstate[2] = i;

                backgroundWorker1.ReportProgress(100*(i+1)/ TestData.Count(),userstate);
               
            }


            dt2 = DateTime.Now;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            tbRes.AppendText("TP- " + TP + "||FP-" + FP + Environment.NewLine);
            tbRes.AppendText("FN- " + FN + "||TN-" + TN + Environment.NewLine);
            lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            tbRes.AppendText("Затрачено времени:" + lAllTime);
        }
   
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            lAllTimep = 0;
            dt1p = DateTime.Now;
            for (int i = 0; i < TestData.Count(); i++)
            {
                if (backgroundWorker2.CancellationPending) break;
             
                int a = TestData[i].EntryClass;
                int b = KNNParallel(4, TestData[i]);



                if (a == 4 & b == 4) TPP++;
                if (a == 2 & b == 2) TNP++;
                if (a == 2 & b == 4) FPP++;
                if (a == 4 & b == 2) FNP++;
                int[] userstate = new int[3];
                userstate[0] = a;
                userstate[1] = b;
                userstate[2] = i;

                backgroundWorker2.ReportProgress(100 * (i + 1) / TestData.Count(), userstate);

            }
            dt2p = DateTime.Now;
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;


            int[] usstate = (int[])e.UserState;

            tbResParallel.AppendText(usstate[2] + ") Настоящий класс " + usstate[0] + "Предсказанный " + usstate[1]+ Environment.NewLine);

        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tbResParallel.AppendText("TP- " + TPP + "||FP-" + FPP + Environment.NewLine);
            tbResParallel.AppendText("FN- " + FNP + "||TN-" + TNP + Environment.NewLine);
            lAllTimep = lAllTimep + (dt2p - dt1p).TotalMilliseconds;
            tbResParallel.AppendText("Затрачено времени:" + lAllTimep);
        }

        private void btCalculate_Click(object sender, EventArgs e)
        {

            backgroundWorker1.RunWorkerAsync();
        }
        private void btnParallelCalculate_Click(object sender, EventArgs e)
        {
            backgroundWorker2.RunWorkerAsync();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
    
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && e.KeyChar != 8)
                e.Handled = true;
        
    }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            tbRes.Clear();
            tbResParallel.Clear();
            int rowNum;
            int.TryParse(tbDownload.Text, out rowNum);
            ReadData(@"D:\C#\data mining\input.data",rowNum);
            Normalize();
            NormalizeTest();
            //backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            // backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            panel1.Enabled = true;
            panel2.Enabled = true;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker2.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker2.WorkerSupportsCancellation = true;
            tbRes.AppendText(datainfo + Environment.NewLine);
            tbResParallel.AppendText(datainfo + Environment.NewLine);
        }

        private void tbDownload_TextChanged(object sender, EventArgs e)
        {
            if (tbDownload.Text.Length > 0)
            {
                btnDownload.Enabled = true;
            }
            else
            {
                btnDownload.Enabled = false;
            }
        }

        private void btCancelParallel_Click(object sender, EventArgs e)
        {
            backgroundWorker2.CancelAsync();
   

        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        
          

        }
    }
}
