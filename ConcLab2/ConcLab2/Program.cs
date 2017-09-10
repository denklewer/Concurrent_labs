using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcLab2
{
    class Program
    {
        static Boolean[] mass;
        
        static Thread[] mThreadPool;
        static int CurrentPrime;
  


            
        public static void InitArray(int length)
        {
            mass = new Boolean[length];
            for (int i = 2; i < mass.Length; i++) {
                mass[i] = false;
            }

        }
      
   

        public static void Resheto() {

            for (int m = 2; m < Math.Sqrt(mass.Length); m++) {
                if (mass[m] == false) {

                    for (int k = m * 2; k < mass.Length; k = k + m)
                    {
                        if (k % m == 0)
                        {
                            mass[k] = true;

                        }
                    }

                }


            }
            int count = 0;
            for (int i = 2; i < mass.Length; i++) {
                if (mass[i] == false) {
                    count++;
                    Console.WriteLine(i);
                }

            }
            Console.WriteLine("Всего " + count);

        }

        static void FindBasics() {
            double sqr = Math.Sqrt(mass.Length);
            for (int m = 2; m <=sqr; m++)
            {
                if (mass[m] == false)
                {

                    for (int k = 2 * m; k < sqr; k = k + m)
                    {
                        if (k % m == 0)
                        {
                            mass[k] = true;

                        }
                    }

                }
            }
        }


        public static void ModifiedResheto()
        {
            double sqr = Math.Sqrt(mass.Length);
            /*      
             Этап 1, поиск простых от 2 до корня*/
            FindBasics();
            /*      
               Этап 2, поиск простых от корня до n*/
            

            for (int m = 2; m <= sqr; m++)
            {
                if (mass[m] == false)
                {
                    int floor = (int)sqr;
                
              
                     int start = floor > m*m  ? (floor%m==0? floor :  floor - floor % m + m) : m*m;
           



                    for (int k = start; k < mass.Length; k=k+m)
                    {
                        if (k % m == 0)
                        {
                            mass[k] = true;
                        }
                    }
                }


            }
    
           
       
        }


        static void FindPrimeAlg1(Object pArgs)
        {
            int[] lArgs = (int[])pArgs;
            double sqr = Math.Sqrt(mass.Length);
            for (int m = 2; m < sqr; m++)
            {
                if (mass[m] == false)
                {

                    int start = lArgs[0] % m != 0 ?
                        (lArgs[0] > m * m ? lArgs[0] - lArgs[0] % m + m : m * m) :
                        lArgs[0] ;
                    
                    for (int k = start; k < lArgs[1]; k=k+m)
                    {
                        if (k % m == 0)
                        {
                            mass[k] = true;
                        }
                    }
                }
            }
        }

        static void FindPrimeAlg2(Object pArgs)
        {
           
            int[] lArgs = (int[])pArgs;
            double sqr = Math.Sqrt(mass.Length);
            for (int m = lArgs[0]; m < lArgs[1]; m++)
            {
                if (mass[m] == false)
                {

                    int floor = (int)sqr;


                    int start = floor > m * m ? (floor % m == 0 ? floor : floor - floor % m + m) : m * m;



                    for (int k = start; k < mass.Length; k=k+m)
                    {
                        if (k % m == 0)
                        {
                            mass[k] = true;
                        }
                    }
                }
            }
        }


        public static void Alg1(int M) {
            Console.WriteLine("Декомпозиция по данным");
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
      
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M="+ M);
            Console.WriteLine("-----------------------------------------");
          
            for (int j = 0; j < 10; j++)
            {
                InitArray(mass.Length);
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                int start = (int)Math.Sqrt(mass.Length);
                int N = mass.Length-start;
                FindBasics();
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[2];
                
                    lArgs[0] = start+(N / M) * i;
                    if (start+(N / M) * (i + 1) > N - 1)
                    {
                        lArgs[1] = start+N;

                    }
                    else {
                        lArgs[1] = start+(N / M) * (i + 1);
                    }
                 
                    mThreadPool[i] = new Thread(FindPrimeAlg1);

                   
                    mThreadPool[i].Start(lArgs);
                }

                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                int count = 0 ;
                long sum = 0;
                for (int k = 2; k < mass.Length; k++)
                {
                    if (mass[k] == false)
                    {
                     
                        sum = sum + k;
                        count++;

                    }


                }
                Console.WriteLine("Всего " + count + " сумма " + sum);
            }

          
          
            Console.WriteLine("Время: " + lAllTime / 10);

        }


        public static void Alg2(int M)
        {

            Console.WriteLine("Деление базовых" );
  
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=" + M);
            Console.WriteLine("-----------------------------------------");

            for (int j = 0; j < 10; j++)
            {
                InitArray(mass.Length);
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                int start = 2;
                int N = (int)Math.Sqrt(mass.Length)-start;
                FindBasics();
             
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[2];

                    lArgs[0] = start+ N / M * i;
                    if (start+ N / M * (i + 1) > N )
                    {
                        lArgs[1] =  start+N;

                    }
                    else {
                        lArgs[1] = start+ N / M * (i + 1);
                    }

                    mThreadPool[i] = new Thread(FindPrimeAlg2);
                 
                  
                    mThreadPool[i].Start(lArgs);
                }

                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                int count = 0;
                long sum = 0;
                for (int k = 2; k < mass.Length; k++)
                {
                    if (mass[k] == false)
                    {

                        sum = sum + k;
                        count++;

                    }


                }
                Console.WriteLine("Всего " + count + " сумма " + sum);
            }



            Console.WriteLine("Время: " + lAllTime / 10);

        }

        static void FindPrimeAlg3(Object pArgs)
        {
           
            double sqr = Math.Sqrt(mass.Length);
            int m = (int)((Object[])pArgs)[0];
            ManualResetEvent ev = ((Object[])pArgs)[1] as ManualResetEvent;
            int floor = (int)sqr;
            int start = floor > m * m ? (floor % m == 0 ? floor : floor - floor % m + m) : m * m;

            for (int k = start; k < mass.Length; k = k + m)
                    {
                        if (k % m == 0)
                        {
                            mass[k] = true;
                        }
                    }

            ev.Set();

                
            
        }
        public static void Alg3(int M)
        {

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Пул потоков");
            Console.WriteLine("-----------------------------------------");
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;


          

            for (int j = 0; j < 10; j++)
            {
                InitArray(mass.Length);
                dt1 = DateTime.Now;
         
                int start = (int)Math.Sqrt(mass.Length);
                int N = mass.Length - start;
                FindBasics();

                ManualResetEvent[] events = new ManualResetEvent[start];
                for (int i = 0; i < start; i++) {
                    events[i] = new ManualResetEvent(false);
                    if (mass[i] == false && i>=2)
                    {
                       
                        ThreadPool.QueueUserWorkItem(FindPrimeAlg3, new object[] { i, events[i] });
                    }
                    else {
                        events[i].Set();
                    }
                }
                for (int i = 0; i < events.Length; i++) {
                    events[i].WaitOne();
                }
              
               // WaitHandle.WaitAll(events);
         


                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                int count = 0;
                long sum = 0;
                for (int k = 2; k < mass.Length; k++)
                {
                    if (mass[k] == false)
                    {

                        sum = sum + k;
                        count++;

                    }


                }
                Console.WriteLine("Всего " + count + " сумма " + sum);
            }



            Console.WriteLine("Время: " + lAllTime / 10);

        }
        static int getCurrentIndex()
        {
            lock ("index")
            {
                return CurrentPrime++;
            }
        }
        static void FindPrimeAlg4(Object pArgs)
        {
            ManualResetEvent ev = ((Object[])pArgs)[0] as ManualResetEvent;
            while (true)
            {
          

                
                double sqr = Math.Sqrt(mass.Length);
                int m;
                 m = getCurrentIndex();
               
                while (m<sqr && mass[m] != false) {
                    m = getCurrentIndex();
                }
                if (m >= sqr) {
                    break;
                }



                    int floor = (int)sqr;
                int start = floor > m * m ? (floor % m == 0 ? floor : floor - floor % m + m) : m * m;

                for (int k = start; k < mass.Length; k = k + m)
                {
                    if (k % m == 0)
                    {
                        mass[k] = true;
                    }
                }
            }
            ev.Set();



        }

        public static void Alg4(int M)
        {

            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("Пул потоков с очередью");
            Console.WriteLine("M= "+ M);
            Console.WriteLine("-----------------------------------------");
            DateTime dt1, dt2;
            double lAllTime;
           
            lAllTime = 0;




            for (int j = 0; j < 10; j++)
            {
                InitArray(mass.Length);
                CurrentPrime = 2;
                dt1 = DateTime.Now;
             


                FindBasics();
                
                ManualResetEvent[] events = new ManualResetEvent[M];
                for (int i = 0; i < M; i++)
                {
                    events[i] = new ManualResetEvent(false);
                 

                        ThreadPool.QueueUserWorkItem(FindPrimeAlg4, new object[] {  events[i] });
                
                  
                    
                }
                WaitHandle.WaitAll(events);



                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                int count = 0;
                long sum = 0;
                for (int k = 2; k < mass.Length; k++)
                {
                    if (mass[k] == false)
                    {

                        sum = sum + k;
                        count++;

                    }


                }
                Console.WriteLine("Всего " + count + " сумма " + sum);
            }



            Console.WriteLine("Время: " + lAllTime / 10);

        }



        static void Main(string[] args)
        {
            DateTime dt1, dt2;
            double lAllTime;
            int n = 100000000;
          
            InitArray(n);
            // Последовательная версия
            lAllTime = 0;
            for (int i = 0; i < 10; i++)
            {
                InitArray(n);
                dt1 = DateTime.Now;
                ModifiedResheto();
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                int count = 0;
                long sum = 0;
                for (int j = 2; j < mass.Length; j++)
                {
                    if (mass[j] == false)
                    {
                      
                        count++;
                        sum = sum + j;

                    }
                 

                }
                Console.WriteLine("Всего " + count + " сумма " + sum);
            }
            Console.WriteLine("Время: " + lAllTime / 10);
            // Многопоточная версия 1

           /* Alg1(2);
            Alg1(3);
            Alg1(4);*/

            // Многопоточная версия 2

           /* Alg2(2);
            Alg2(3);
            Alg2(4);*/

            // Многопоточная версия 3

           Alg3(0);

            // Многопоточная версия 4

          /*  Alg4(2);
            Alg4(3);
            Alg4(4);*/
            Console.ReadLine();



        }
    }
}
