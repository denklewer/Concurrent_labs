using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Collections.Concurrent;

namespace ConcLab21
{
    class Program
    {
       public static CancellationTokenSource cts;
       public static ConcurrentBag<TaskDetails> bag;
   

        public class TaskDetails {
           public int thrID;
            public int taskID;
            
            public double msec1;
            public double msec2;
            public TaskDetails(int thr, int task,  double ms1, double ms2) {
                thrID = thr;
                taskID = task;
             
                msec1 = ms1;
                msec2 = ms2;

            }
        }    
        public  static void ImageProcessing(string filename) {
            //     Image img = Image.FromFile(@"D:/C#/Images/1.jpg");
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
            dt1 = DateTime.Now;


            Image img = Image.FromFile(filename);
            Graphics gr;
            Bitmap bmp = new Bitmap(img);
            string[] strmas = filename.Split('/');
            string imgName = strmas.Last();

            for (int row = 0; row < bmp.Width; row++) // Indicates row number
            {
                for (int column = 0; column < bmp.Height; column++) // Indicate column number
                {
                    var colorValue = bmp.GetPixel(row, column); // Get the color pixel
                    var averageValue = ((int)colorValue.R + (int)colorValue.B + (int)colorValue.G) / 3; // get the average for black and white
                    bmp.SetPixel(row, column, Color.FromArgb(averageValue, averageValue, averageValue)); // Set the value to new pixel
                }
            }
            img = Image.FromHbitmap(bmp.GetHbitmap());
            gr = Graphics.FromImage(img);
            gr.Save();
            SolidBrush br = new SolidBrush(Color.Magenta);
            String message = "Картинка " + imgName;
            Font font = (new Font(FontFamily.GenericMonospace, img.Height/20));
            gr.DrawString(message, font, br, img.Width / 2 - gr.MeasureString(message, font).Width / 2, img.Height / 5);

            gr.Flush();
     
            string fname = @"D:/C#/Changed_Images/" +  imgName;
            img.Save(fname);
            dt2 = DateTime.Now;
            lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            int thr = Thread.CurrentThread.ManagedThreadId;
            int task = Task.CurrentId.Value;
            double ms1 = dt1.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            double ms2 = dt2.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            bag.Add(new TaskDetails(thr, task, ms1, ms2));






        }

        public static string [] InitImages(int num) {
            string fname = @"D:/C#/Images/";
            string[] images = new string[num];
            for (int i = 0; i < num; i++) {
                images[i] = fname + i + ".jpg";
            }
            return images;
        }

        public static void PerformSingleThread(int num) {
            string[] ImageFiles = InitImages(num);
            Console.WriteLine("Запуск обработки");

            for (int i = 0; i < ImageFiles.Length; i++) {
                ImageProcessing(ImageFiles[i]);
            }
  

        }
        public static void PerformWithoutPartitioner(int num) {
            cts = new CancellationTokenSource();

            Console.WriteLine("Инициирую");
            bag = new ConcurrentBag<TaskDetails>();
            string[] ImageFiles = InitImages(num);

            Console.WriteLine("Запуск обработки");

            Thread thr = new Thread(() => {
                while (true)
                {

                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        cts.Cancel();
                    }
                }
            });
            thr.Start();
            try
            {

                ParallelOptions Options = new ParallelOptions()
                {
                    CancellationToken = cts.Token,

                };

                Parallel.ForEach(ImageFiles, Options, ImageProcessing);
            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("Остановлено");
            }

            Console.WriteLine("Готово");

        }


        public static void PerformWithoutPartitioner(int num,int MaxThreadCount)
        {
            cts = new CancellationTokenSource();

            Console.WriteLine("Инициирую");
            bag = new ConcurrentBag<TaskDetails>();
            string[] ImageFiles = InitImages(num);

            Console.WriteLine("Запуск обработки");

            Thread thr = new Thread(() => {
                while (true)
                {

                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        cts.Cancel();
                    }
                }
            });
            thr.Start();
            try
            {

                ParallelOptions Options = new ParallelOptions()
                {
                    CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = MaxThreadCount

                };

                Parallel.ForEach(ImageFiles, Options, ImageProcessing);
            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("Остановлено");
            }

            Console.WriteLine("Готово");

        }


        public static void PerformBalanced(int num)
        {
            cts = new CancellationTokenSource();

            Console.WriteLine("Инициирую");
            bag = new ConcurrentBag<TaskDetails>();
            string[] ImageFiles = InitImages(num);

            Console.WriteLine("Запуск обработки");

            Thread thr = new Thread(() => {
                while (true)
                {

                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        cts.Cancel();
                    }
                }
            });
            thr.Start();
            try
            {

                ParallelOptions Options = new ParallelOptions()
                {
                    CancellationToken = cts.Token,

                };
                var PartedData = Partitioner.Create(ImageFiles, true);
                Parallel.ForEach(PartedData, Options, ImageProcessing);
            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("Остановлено");
            }

            Console.WriteLine("Готово");

        }

        public static void PerformStatic(int num)
        {
            cts = new CancellationTokenSource();

            Console.WriteLine("Инициирую");
            bag = new ConcurrentBag<TaskDetails>();
            string[] ImageFiles = InitImages(num);

            Console.WriteLine("Запуск обработки");

            Thread thr = new Thread(() => {
                while (true)
                {

                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        cts.Cancel();
                    }
                }
            });
            thr.Start();
            try
            {

                ParallelOptions Options = new ParallelOptions()
                {
                    CancellationToken = cts.Token,

                };
                Parallel.ForEach(Partitioner.Create(0, num), Options, range => {
                    for (int i = range.Item1; i < range.Item2; i++)
                        ImageProcessing(ImageFiles[i]);
                });
            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("Остановлено");
            }

            Console.WriteLine("Готово");

        }


        public static void PerformFixedBlockSize(int num,int intBlockSize)
        {
            cts = new CancellationTokenSource();

            Console.WriteLine("Инициирую");
            bag = new ConcurrentBag<TaskDetails>();
            string[] ImageFiles = InitImages(num);

            Console.WriteLine("Запуск обработки");

            Thread thr = new Thread(() => {
                while (true)
                {

                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                        cts.Cancel();
                    }
                }
            });
            thr.Start();
            try
            {

                ParallelOptions Options = new ParallelOptions()
                {
                    CancellationToken = cts.Token,

                };            
                Parallel.ForEach(Partitioner.Create(0, num, intBlockSize), Options, range => {
                    for (int i = range.Item1; i < range.Item2; i++)
                        ImageProcessing(ImageFiles[i]);
                });





            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("Остановлено");
            }

            Console.WriteLine("Готово");

        }


        static void Main(string[] args)
        {
            DateTime time = DateTime.Now;
            double ProgramStart = time.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

          PerformWithoutPartitioner(100);
           
            Console.WriteLine("id главного потока " + Thread.CurrentThread.ManagedThreadId);

            var Threads = bag.GroupBy(p => p.thrID);
            foreach (var thread in Threads)
            {
                Console.WriteLine("Поток: " + thread.Key);
                
                var res = bag.Select(p => new { thrID = p.thrID, taskID=p.taskID  } ).Where(p=>p.thrID==thread.Key).Distinct();

                foreach (var item in res)
                {
                    Console.WriteLine(" Задание: " + item.taskID);

                   var minVal =  bag.Select(f => new { msec1 = f.msec1, thrID = f.thrID, f.taskID }).Where((f => (f.thrID == thread.Key & f.taskID== item.taskID))).Min(s=>s.msec1)-ProgramStart;
                    var maxVal = bag.Select(f => new { msec2 = f.msec2, thrID = f.thrID, f.taskID }).Where((f => (f.thrID == thread.Key & f.taskID == item.taskID))).Max(s=>s.msec2)- ProgramStart;

                    Console.WriteLine("     Количество элементов в задании " + bag.Count(p => (p.taskID == item.taskID)) + " Начало " + minVal + " Конец "+ maxVal);

                }
            }
     
            Console.ReadLine();
        }
    }
}
