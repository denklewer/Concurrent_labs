using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcLab3
{

    class Program
    {
        public static String buffer = "";
        public static bool finish;

        public static bool bEmpty = true;

        public static int mAllowWrite = 1;
        public static int mAllowRead = 0;
        public static int Count;

        public static List<String> Check;

        public static int N = 1;
        public static void Read()
        {
            List<String> MyMessages = new List<String>();
            while (!finish)
            {
                if (!bEmpty)
                {
                    MyMessages.Add(buffer);
                    Count++;
                    bEmpty = true;
                }

            }

       
       //     Console.WriteLine("поток " + Thread.CurrentThread.ManagedThreadId + " считал " + MyMessages.Count);
          


        }

        public static void Write()
        {
            List<String> MyMessages = new List<String>();
            int i = 0;

            String Message;
            Message = Thread.CurrentThread.Name + "-";
            for (int j = 0; j < N; j++)
            {
                MyMessages.Add(Message + j);
            }

            while (i < N)
            {
                if (bEmpty)
                {
                    buffer = MyMessages[i++];
                    bEmpty = false;
                }

            }
        }


        public static void LockRead()
        {
            List<String> MyMessages = new List<String>();
       
                while (!finish)
                {
                    if (!bEmpty)
                    {
                        lock ("read")
                        {
                            if (!bEmpty)
                            {

                                MyMessages.Add(buffer);
                            Count++;
                                bEmpty = true;

                            }
                        }
                    }

                }


            //Проверка

            /* foreach (String item in MyMessages)
             {
                 lock ("check")
                 {
                     Check.Add(item);
                 }
             }*/
            //     Console.WriteLine("поток " + Thread.CurrentThread.ManagedThreadId + " считал " + MyMessages.Count);
          


        }

        public static void LockWrite()
        {
            List<String> MyMessages = new List<String>();
            int i = 0;

            String Message = Thread.CurrentThread.Name + "-";
            for (int j = 0; j < N; j++)
            {
                MyMessages.Add(Message + j);
            }

            while (i < N)
            {
                if (bEmpty)
                {
                    lock ("write")
                    {
                        if (bEmpty)
                        {
                            buffer = MyMessages[i++];
                            bEmpty = false;
                        }

                    }
                }
            }
        }

        public static void ResetEventRead(object state)
        {
            List<String> MyMessages = new List<String>();
            var evFull = ((Object[])state)[0] as AutoResetEvent;
            var evEmpty = ((Object[])state)[1] as AutoResetEvent;
            while (true)
            {

                evFull.WaitOne();

                if (finish) break;
                MyMessages.Add(buffer);
               // Check.Add(buffer);
                Count++;
                evEmpty.Set();



            }




            //   Console.WriteLine("поток " + Thread.CurrentThread.ManagedThreadId + " считал " + MyMessages.Count);




        }

        public static void ResetEventWrite(Object args)
        {
            List<String> MyMessages = new List<String>();
            int i = 0;

            var evFull = ((Object[])args)[0] as AutoResetEvent;
            var evEmpty = ((Object[])args)[1] as AutoResetEvent;
            string Message;

            Message = Thread.CurrentThread.Name + "-";


            for (int j = 0; j < N; j++)
            {

                MyMessages.Add(Message + j);
            }

            while (i < N)
            {
                evEmpty.WaitOne();
                buffer = MyMessages[i++];
                evFull.Set();



            }



        }

        public static void SemaphoreRead(object state)
        {
            List<String> MyMessages = new List<String>();
            var lSemaphore = state as SemaphoreSlim;

            while (!finish)
            {



                if (finish) break;
                if (!bEmpty)
                {
                    lSemaphore.Wait();
                    if (!bEmpty)
                    {
                        bEmpty = true;

                        MyMessages.Add(buffer);
                        //проверка
                       // Check.Add(buffer);
                        Count++;
                    }
                    lSemaphore.Release();
                }





            }




        }

        public static void SemaphoreWrite(Object args)
        {
            List<String> MyMessages = new List<String>();
            int i = 0;

            var lSemaphore = args as SemaphoreSlim;

            string Message;

            Message = Thread.CurrentThread.Name + "-";


            for (int j = 0; j < N; j++)
            {

                MyMessages.Add(Message + j);
            }

            while (i < N)
            {
                if (bEmpty)
                {
                    lSemaphore.Wait();
                    if (bEmpty)
                    {
                        bEmpty = false;
                        buffer = MyMessages[i++];
                    }
                    lSemaphore.Release();
                }




            }



        }

        public static void InterlockedWrite()
        {
            List<String> MyMessages = new List<String>();
            int i = 0;

            String Message = Thread.CurrentThread.Name + "-";
            for (int j = 0; j < N; j++)
            {
                MyMessages.Add(Message + j);
            }

            while (i < N)
            {
                if (1 == Interlocked.CompareExchange(ref mAllowWrite, 0, 1))
                {

                    buffer = MyMessages[i];
                    i++;
                }
            }
        }

        public static void InterlockedRead()
        {
            List<String> MyMessages = new List<String>();
            while (!finish)
            {

                if (0 == Interlocked.CompareExchange(ref mAllowWrite, 1, 0))
                {
                    MyMessages.Add(buffer);
                    Interlocked.Increment(ref Count);
                }


            }

            /*  foreach (String item in MyMessages)
              {
                  //Console.WriteLine(item);
              }*/
           // Console.WriteLine("поток " + Thread.CurrentThread.ManagedThreadId + " считал " + MyMessages.Count);



        }

        /**/
        public static void PerformWithoutSync(int M)
        {
            Thread[] writters = new Thread[M];
            Thread[] readers = new Thread[M];
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
            int tCount = 20;
            for (int t = 0; t < tCount; t++)
            {
                //Начало
                dt1 = DateTime.Now;
                Count = 0;
                finish = false;
                for (int i = 0; i < writters.Length; i++)
                {
                    readers[i] = new Thread(Read);
                    writters[i] = new Thread(Write);
                    writters[i].Name = "T" + i;
                    writters[i].Start();
                    readers[i].Start();
                }
                for (int i = 0; i < M; i++)
                {
                    writters[i].Join();
                }
                finish = true;
                for (int i = 0; i < M; i++)
                {
                    readers[i].Join();
                }
                Console.WriteLine("Записано/Считано " + "?" + "/" + Count);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                //Конец

            }
            Console.WriteLine("Время: " + lAllTime / tCount);



        }
        public static void PerformWithLock(int M)
        {
            Thread[] writters = new Thread[M];
            Thread[] readers = new Thread[M];
          
            //начало

            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;

            int tCount = 20;
            for (int t = 0; t < tCount; t++)
            {
                //начало
                Check = new List<String>();
                dt1 = DateTime.Now;
                Count = 0;
                finish = false;
                for (int i = 0; i < writters.Length; i++)
                {
                    readers[i] = new Thread(LockRead);
                    writters[i] = new Thread(LockWrite);
                    writters[i].Name = "T" + i;
                    readers[i].Start();
                    writters[i].Start();
                }
                for (int i = 0; i < M; i++)
                {
                    writters[i].Join();
                }
                finish = true;

                for (int i = 0; i < M; i++)
                {
                    readers[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                //конец

                //проверка
                /*
                String Need = "";
                String ToMuch = "";
                String str;
                int cnt = 1;
                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        str = writters[i].Name + "-" + j;
                        if (!Check.Contains(str))
                        {
                            Need += (str + ",");
                        }
                        else
                        {
                            cnt = Check.Count(x => (x == str));
                            if (cnt > 1)
                            {
                                ToMuch += (str + ",");
                            }
                        }

                    }
                }
                if (ToMuch.Length == 0)
                {
                    ToMuch = "нет";
                }
                if (Need.Length == 0)
                {
                    Need = "нет";
                }
                Console.WriteLine("Лишние: " + ToMuch);
                Console.WriteLine("Нужны: " + Need);*/
                Console.WriteLine("Записано/Считано " + "?" + "/" + Count);
            }
            Console.WriteLine("Время: " + lAllTime / tCount);



        }
        public static void PerformWithResetEvent(int M)
        {
            Thread[] writters = new Thread[M];
            Thread[] readers = new Thread[M];
            Object[] args = new Object[2];
            var evFull = new AutoResetEvent(false);
            var evEmpty = new AutoResetEvent(true);
            args[0] = evFull;
            args[1] = evEmpty;
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
            int tCount = 20;
            for (int t = 0; t < tCount; t++)
            {
                //начало
                dt1 = DateTime.Now;
                Count = 0;
                Check = new List<String>();
                finish = false;
                for (int i = 0; i < writters.Length; i++)
                {
                    readers[i] = new Thread(ResetEventRead);
                    writters[i] = new Thread(ResetEventWrite);
                    writters[i].Name = "T" + i;
                    writters[i].Start(args);
                    readers[i].Start(args);

                }
                for (int i = 0; i < M; i++)
                {
                    writters[i].Join();
                }
                finish = true;
                for (int i = 0; i < M; i++)
                {
                    evFull.Set();
                    readers[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                //конец


                // проверка
                /*
                if (Count != 30)
                {
                    String Need = "";
                    String ToMuch = "";
                    String str;
                    int cnt = 1;
                    for (int i = 0; i < M; i++)
                    {
                        for (int j = 0; j < N; j++)
                        {
                            str = writters[i].Name + "-" + j;
                            if (!Check.Contains(str))
                            {
                                Need += (str + ",");
                            }
                            else
                            {
                                cnt = Check.Count(x => (x == str));
                                if (cnt > 1)
                                {
                                    ToMuch += (str + ",");
                                }
                            }
                        }
                    }
                    if (ToMuch.Length == 0)
                    {
                        ToMuch = "нет";
                    }
                    if (Need.Length == 0)
                    {
                        Need = "нет";
                    }
                    Console.WriteLine("Лишние: " + ToMuch);
                    Console.WriteLine("Нужны: " + Need);
                    }*/
                Console.WriteLine("Записано/Считано " + "?" + "/" + Count);
                }
            
            Console.WriteLine("Время: " + lAllTime / tCount);


        }

        public static void PerformWithSemaphore(int M)
        {
            Thread[] writters = new Thread[M];
            Thread[] readers = new Thread[M];
            Object args = new Object();
           
            var lSemaphore = new SemaphoreSlim(1, 1);
            args = lSemaphore;
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
            int tCount = 5;
            for (int t = 0; t < tCount; t++)
            {
                dt1 = DateTime.Now;
                //начало
                Count = 0;
                Check = new List<String>();
                finish = false;
                for (int i = 0; i < writters.Length; i++)
                {
                    readers[i] = new Thread(SemaphoreRead);
                    writters[i] = new Thread(SemaphoreWrite);
                    writters[i].Name = "T" + i;
                    writters[i].Start(args);
                    readers[i].Start(args);
                }
                for (int i = 0; i < M; i++)
                {
                    writters[i].Join();
                }

                finish = true;

                for (int i = 0; i < M; i++)
                {
                    readers[i].Join();
                }

                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                //конец




               
                //проверка 
                /*
                String Need = "";
                String ToMuch = "";
                String str;
                int cnt = 1;
                for (int i = 0; i < M; i++)
                {

                    for (int j = 0; j < N; j++)
                    {

                        str = writters[i].Name + "-" + j;
                        if (!Check.Contains(str))
                        {

                            Need += (str + ",");

                        }
                        else
                        {
                            cnt = Check.Count(x => (x == str));
                            if (cnt > 1)
                            {
                                ToMuch += (str + ",");
                            }
                        }

                    }
                }
                if (ToMuch.Length == 0)
                {
                    ToMuch = "нет";
                }
                if (Need.Length == 0)
                {
                    Need = "нет";
                }
                if ((Count != 30) | (ToMuch != "нет") | (Need != "нет"))
                {
                    Console.WriteLine("Лишние: " + ToMuch);
                    Console.WriteLine("Нужны: " + Need);
                    
                }
    */


                    Console.WriteLine("Записано/Считано " + "?" + "/" + Count);
                }
            
            Console.WriteLine("Время: " + lAllTime / tCount);
        }


        public static void PerformWithInterlocked(int M)
        {
            Thread[] writters = new Thread[M];
            Thread[] readers = new Thread[M];
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
            int tCount = 5;
            for (int t = 0; t < tCount; t++)
            {
                dt1 = DateTime.Now;

                //   начало
                Count = 0;
                finish = false;
                for (int i = 0; i < writters.Length; i++)
                {
                    readers[i] = new Thread(InterlockedRead);
                    writters[i] = new Thread(InterlockedWrite);
                    writters[i].Start();
                    readers[i].Start();


                }

                for (int i = 0; i < M; i++)
                {
                    writters[i].Join();

                }
                finish = true;

                for (int i = 0; i < M; i++)
                {
                    readers[i].Join();
                }
                Console.WriteLine("Записано/Считано " + "?" + "/" + Count);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                //конец
            }
            Console.WriteLine("Время: " + lAllTime / tCount);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("M=2");
            int M = 2;
          N = 100;
            PerformWithInterlocked(M);
            N = 1000;
            PerformWithInterlocked(M);
            N = 10000;
            PerformWithInterlocked(M);
            N = 100000;
            PerformWithInterlocked(M);
            N = 1000000;
            PerformWithInterlocked(M);

            Console.WriteLine("----------------------------------");
            Console.WriteLine("M=3");
             M = 3;
            N = 100;
            PerformWithInterlocked(M);
            N = 1000;
            PerformWithInterlocked(M);
            N = 10000;
            PerformWithInterlocked(M);
            N = 100000;
            PerformWithInterlocked(M);
            N = 1000000;
            PerformWithInterlocked(M);
            Console.WriteLine("----------------------------------");
            Console.WriteLine("M=4");
            M = 4;
            N = 100;
            PerformWithInterlocked(M);
            N = 1000;
            PerformWithInterlocked(M);
            N = 10000;
            PerformWithInterlocked(M);
         

            Console.ReadLine();
        }
    }
}
