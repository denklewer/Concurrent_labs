using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcLab5
{


    class Program
    {

        public static void Swap(double[] pData, int i, int j) {
            double s = pData[i];
            pData[i] = pData[j];
            pData[j] = s;
        }
       public  static void SerialQuickSort(double[] pData, int first, int last)
        {
            if (first >= last)
                return;
            int PivotPos = first;
            double Pivot = pData[first];
            for (int i = first + 1; i <= last; i++)
            {
                if (pData[i] < Pivot)
                {
                    if (i != PivotPos + 1)
                        Swap(pData,i, PivotPos + 1);
                    PivotPos++;
                }
            }
            Swap(pData,first, PivotPos);
            SerialQuickSort(pData, first, PivotPos - 1);
            SerialQuickSort(pData, PivotPos + 1, last);
        }


        private static int Partition(double[] array, int from, int to, int pivot)
        {
            // requires: 0 <= from <= pivot <= to <= array.Length-1
            int last_pivot = -1;
            double pivot_val = array[pivot];
            if (from < 0 || to > array.Length - 1)
            {
                throw new System.Exception(String.Format("Partition: indices out of bounds: from={0}, to={1}, Length={2}",
                from, to, array.Length));
            }
            while (from < to)
            {
                if (array[from] > pivot_val)
                {
                    Swap(array, from, to);
                    to--;
                }
                else {
                    if (array[from] == pivot_val)
                    {
                        last_pivot = from;
                    }
                    from++;
                }
            }
            if (last_pivot == -1)
            {
                if (array[from] == pivot_val)
                {
                    return from;
                }
                else {
                    throw new System.Exception(String.Format("Partition: pivot element not found in array"));
                }
            }
            if (array[from] > pivot_val)
            {
                // bring pivot element to end of lower half
                Swap(array, last_pivot, from - 1);
                return from - 1;
            }
            else {
                // done, bring pivot element to end of lower half
                Swap(array, last_pivot, from);

                return from;
            }
        }

      

        static double[] array;
        static void ParallelQuickSort(Object args)
        {
            object[] lArgs = (object[])args;
          

            int from = (int)(lArgs[0]);
            int to = (int)(lArgs[1]);
            int depthRemaining = (int)(lArgs[2]);
            int Threshold = (int)(lArgs[3]);
            ManualResetEvent ev = (lArgs[4] as ManualResetEvent);


      

            if (to - from <= Threshold)
            {
                Array.Sort(array, from, to-from+1);
            }
            else {
                int pivot = from + (to - from) / 2;
                pivot = Partition(array, from, to, pivot);
                ManualResetEvent[] events = new ManualResetEvent[2];

                events[0] = new ManualResetEvent(false);
                events[1] = new ManualResetEvent(false);
                Object[] input = new object[5];
              
                input[0] = from;
                input[1] = pivot - 1;
                input[2] = depthRemaining - 1;
                input[3] = Threshold;
                input[4] = events[0];


                Object[] input2 = new object[5];
               
                input2[0] = pivot + 1;
                input2[1] = to;
                input2[2] = depthRemaining - 1;
                input2[3] = Threshold;
                input2[4] = events[1];


                if (depthRemaining > 0)
                {
                
                    ThreadPool.QueueUserWorkItem(ParallelQuickSort, input);
                    ThreadPool.QueueUserWorkItem(ParallelQuickSort, input2);


                    WaitHandle.WaitAll(events);


               
                }
                else {
                    ParallelQuickSort(input);
                    ParallelQuickSort(input2);
                }
            }

            ev.Set();

        }



        static void ExecuteParallelSort(double[] pArray,int Threshold, int depthRemaining) {

            array = pArray;
            ManualResetEvent[] events = new ManualResetEvent[1];

            events[0] = new ManualResetEvent(false);
            Object[] input = new object[5];
      
            input[0] = 0;
            input[1] = array.Length-1;
            input[2] = depthRemaining ;
            input[3] = Threshold;
                

            ThreadPool.QueueUserWorkItem(ParallelQuickSort, input);


            WaitHandle.WaitAll(events);

           

        }




        static void PrintMas(double[] mas) {
            foreach (var item in mas)
            {
                Console.Write(item + ",");
            }

        }

    
                static void Main(string[] args)
        {
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
            int tCount = 1;
            string result = "-----------------------------------------\n\r";


            string s = "2";
            Console.WriteLine(double.Parse(s));


            double[] mas2 = new double[1000000];
            Random r = new Random();
            for (int i = 0; i < mas2.Length; i++)
            {
                mas2[i] = r.NextDouble();
               
            }
           
           



            double[] mas = File.ReadAllText(@"D:/C#/in.txt").             
            Split(new Char[] { ';', ' ', '\n','\r' }, StringSplitOptions.RemoveEmptyEntries).
            Select(x => double.Parse(x)).ToArray();
            Console.WriteLine("Файл прочтен");
            

            if (String.Compare(args[0],"-S")==0)
            {
                Console.WriteLine("Запущена однопоточная версия сортировки");
                lAllTime = 0;
                dt1 = DateTime.Now;
                SerialQuickSort(mas, 0, mas.Length - 1);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                result += " однопоточная -- время: " + lAllTime + "\n\r";
                Console.WriteLine("Время: " + lAllTime / tCount);
                Console.ReadLine();
            }
            if (String.Compare(args[0], "-P") == 0)

            {
           
           
                Console.WriteLine("Запущена многопоточная версия сортировки");
                lAllTime = 0;
                
                result += "многопоточная версия \n\r";
/*
                      mas = File.ReadAllText(@"D:/C#/in.txt").
                Split(new Char[] { ';', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).
                Select(x => double.Parse(x)).ToArray();
                     Console.WriteLine("Файл прочтен");

                     dt1 = DateTime.Now;

                   Console.WriteLine("Глубина рекурсии 2");
                     ExecuteParallelSort(mas, 1000, 2);
                     dt2 = DateTime.Now;
                     lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                     Console.WriteLine("Время: " + lAllTime / tCount);
                     result += "глубина 2 -- время: " + lAllTime + "\n\r";
                mas = File.ReadAllText(@"D:/C#/in.txt").
            Split(new Char[] { ';', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).
            Select(x => double.Parse(x)).ToArray();
                Console.WriteLine("Файл прочтен");

                Console.WriteLine("Глубина рекурсии 3");
                lAllTime = 0;
                dt1 = DateTime.Now;
                ExecuteParallelSort(mas, 1000, 3);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                Console.WriteLine("Время: " + lAllTime / tCount);
                result += "глубина 3 -- время: " + lAllTime + "\n\r";

                mas = File.ReadAllText(@"D:/C#/in.txt").
            Split(new Char[] { ';', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).
            Select(x => double.Parse(x)).ToArray();

                Console.WriteLine("Файл прочтен");

                Console.WriteLine("Глубина рекурсии 4");
                lAllTime = 0;
                dt1 = DateTime.Now;
                ExecuteParallelSort(mas, 1000, 4);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                Console.WriteLine("Время: " + lAllTime / tCount);
                result += "глубина 4 -- время: " + lAllTime + "\n\r";
                */
        /*        mas = File.ReadAllText(@"D:/C#/in.txt").
            Split(new Char[] { ';', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).
            Select(x => double.Parse(x)).ToArray();
                Console.WriteLine("Файл прочтен");*/

                Console.WriteLine("Глубина рекурсии 5");
                lAllTime = 0;
                dt1 = DateTime.Now;
                ExecuteParallelSort(mas, 1000, 5);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                Console.WriteLine("Время: " + lAllTime / tCount);
                result += "глубина 5 -- время: " + lAllTime + "\n\r";
            }

            File.WriteAllText(@"D:/out.txt", "");//переписываем файл

            for (int i = 0; i < mas.Length; i++)
            {
                File.AppendAllText(@"D:/C#/out.txt", (mas[i].ToString() + ";\n\r"));
            }
            Console.WriteLine("Файл out записан");

    
            File.WriteAllText("D:/summary.txt", "");//переписываем файл

          
                File.AppendAllText("D:/C#/summary.txt", result);
            
            Console.WriteLine("Файл summary записан");
            Console.ReadLine();


        }
    }
}
