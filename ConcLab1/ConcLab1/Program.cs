using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcLab1
{
    class Program
    {
        static int[] mVector;
        static int[] mResVector;
        static Random mRnd =new Random();
        static Thread[] mThreadPool;
        static int CurrentPrime;
        static void initVector(int N) {

            mVector = new int[N];
            mResVector = new int[N];
            for (int i = 0; i <N; i++) {
                mVector[i] = mRnd.Next();
            }
        }


        static void Multiply(int pArg,int start,int end) {
            
            for (int i = start; i < end; i++) {
         
                    mResVector[i] = mVector[i] * pArg;
                
            }
          
        }

        static void MultiplyNotUniform(int pArg, int start, int end)
        {

            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    mResVector[i] = mVector[i] * pArg;
                }
            }

        }
        static void MultiplyNotUniformCircle(int pArg, int num,int start,int end)
        {

            for (int i = start; i < end; i=i+num)
            {
                for (int j = 0; j < i; j++)
                {
                    mResVector[i] = mVector[i] * pArg;
                }
            }

        }

        static void Pow(int pArg, int start, int end)
        {

            for (int i = start; i < end; i++)
            {

                for (int j = 0; j < i; j++)
                {
                    mResVector[i] = (int)Math.Pow(mVector[i], 1.78);
                }
                
            }
        }

        static void MultiplyInThread(Object pArgs) {
            int[] lArgs = (int[])pArgs;

            //Pow(lArgs[0], lArgs[1], lArgs[2]);
            
               //  Multiply(lArgs[0], lArgs[1], lArgs[2]);

            MultiplyNotUniform(lArgs[0], lArgs[1], lArgs[2]);

        }
        static void MultiplyInThreadCircle(Object pArgs)
        {
            int[] lArgs = (int[])pArgs;

            //Pow(lArgs[0], lArgs[1], lArgs[2]);

            //  Multiply(lArgs[0], lArgs[1], lArgs[2]);

            MultiplyNotUniformCircle(lArgs[0], lArgs[1], lArgs[2],lArgs[3]);

        }


        static void PerformCalcCircle(int pN)
        {

            DateTime dt1, dt2;
            double lAllTime;
            int N = pN;

            int x = 31;
            /*
            N=10
            */
            Console.WriteLine("*****************************************");
            Console.WriteLine("N=" + N);
            Console.WriteLine("*****************************************");
            initVector(N);
            // Без потоков
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("без потоков");
            Console.WriteLine("-----------------------------------------");
            lAllTime = 0;
            for (int i = 0; i < 10; i++)
            {
                dt1 = DateTime.Now;

                //   Pow(x, 0, N);

                //Multiply(x, 0, N);
                MultiplyNotUniformCircle(x, 1,0, N);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);

            // Число потоков 2
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=2");
            Console.WriteLine("-----------------------------------------");
            int M = 2;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];

                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[4];
                    lArgs[0] = x;
                    
                    lArgs[1] = M;
                    lArgs[2] = i;
                    lArgs[3] = N;
                    mThreadPool[i] = new Thread(MultiplyInThreadCircle);


                    mThreadPool[i].Start(lArgs);
                }

                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);

            // Число потоков 3
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=3");
            Console.WriteLine("-----------------------------------------");
            M = 3;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[4];
                    lArgs[0] = x;
                    lArgs[1] = M;
                    lArgs[2] = i;
                    lArgs[3] = N;
                    mThreadPool[i] = new Thread(MultiplyInThreadCircle);


                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);

            // Число потоков 4
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=4");
            Console.WriteLine("-----------------------------------------");
            M = 4;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[4];
                    lArgs[0] = x;
                    lArgs[1] = M;
                    lArgs[2] = i;
                    lArgs[3] = N;
                    mThreadPool[i] = new Thread(MultiplyInThreadCircle);


                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);

            // Число потоков 5
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=5");
            Console.WriteLine("-----------------------------------------");
            M = 5;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[4];
                    lArgs[0] = x;
                    lArgs[1] = M;
                    lArgs[2] = i;
                    lArgs[3] = N;
                    mThreadPool[i] = new Thread(MultiplyInThreadCircle);


                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);
            // Число потоков 8
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=8");
            Console.WriteLine("-----------------------------------------");
            M = 8;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[4];
                    lArgs[0] = x;
                    lArgs[1] = M;
                    lArgs[2] = i;
                    lArgs[3] = N;
                    mThreadPool[i] = new Thread(MultiplyInThreadCircle);


                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);


            // Число потоков 10
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=10");
            Console.WriteLine("-----------------------------------------");
            M = 10;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[4];
                    lArgs[0] = x;
                    lArgs[1] = M;
                    lArgs[2] = i;
                    lArgs[3] = N;
                    mThreadPool[i] = new Thread(MultiplyInThreadCircle);


                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);



        }



        static void PerformCalc(int pN) {

            DateTime dt1, dt2;
            double lAllTime;
            int N = pN;

            int x = 31;
            /*
            N=10
            */
            Console.WriteLine("*****************************************");
            Console.WriteLine("N=" + N);
            Console.WriteLine("*****************************************");
            initVector(N);
            // Без потоков
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("без потоков");
            Console.WriteLine("-----------------------------------------");
            lAllTime = 0;
            for (int i = 0; i < 10; i++)
            {
                dt1 = DateTime.Now;
             
                 //   Pow(x, 0, N);
                
                //Multiply(x, 0, N);
                MultiplyNotUniform(x, 0, N);
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);

            // Число потоков 2
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=2");
            Console.WriteLine("-----------------------------------------");
            int M = 2;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[3];
                    lArgs[0] = x;
                    lArgs[1] = (N / M) * i;
                    if ((N / M) * (i + 1) > N - 1)
                    {
                        lArgs[2] = N ;

                    }
                    else {
                        lArgs[2] = (N / M) * (i + 1);
                    }
                    mThreadPool[i] = new Thread(MultiplyInThread);

                    
                    mThreadPool[i].Start(lArgs);
                }

                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);
       
            // Число потоков 3
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=3");
            Console.WriteLine("-----------------------------------------");
            M = 3;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[3];
                    lArgs[0] = x;
                    lArgs[1] = (N / M) * i;
                    if ((N / M) * (i + 1) > N - 1)
                    {
                        lArgs[2] = N ;

                    }
                    else {
                        lArgs[2] = (N / M) * (i + 1);
                    }
                    mThreadPool[i] = new Thread(MultiplyInThread);
                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);
       
            // Число потоков 4
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=4");
            Console.WriteLine("-----------------------------------------");
            M = 4;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];    
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[3];
                    lArgs[0] = x;
                    lArgs[1] = (N / M) * i;
                    if ((N / M) * (i + 1) > N- 1)
                    {
                        lArgs[2] = N ;

                    }
                    else {
                        lArgs[2] = (N / M) * (i + 1);
                    }
                    mThreadPool[i] = new Thread(MultiplyInThread);
                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);
    
            // Число потоков 5
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=5");
            Console.WriteLine("-----------------------------------------");
            M = 5;
            lAllTime = 0;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i <M; i++)
                {
                    int[] lArgs = new int[3];
                    lArgs[0] = x;
                    lArgs[1] = (N / M) * i;
                    if ((N / M) * (i + 1) >N - 1)
                    {
                        lArgs[2] = N ;

                    }
                    else {
                        lArgs[2] = (N / M) * (i + 1);
                    }
                    mThreadPool[i] = new Thread(MultiplyInThread);
                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);
            // Число потоков 8
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=8");
            Console.WriteLine("-----------------------------------------");
            M = 8;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i <M; i++)
                {
                    int[] lArgs = new int[3];
                    lArgs[0] = x;
                    lArgs[1] = (N / M) * i;
                    if ((N / M) * (i + 1) > N - 1)
                    {
                        lArgs[2] = N ;

                    }
                    else {
                        lArgs[2] = (N / M) * (i + 1);
                    }
                    mThreadPool[i] = new Thread(MultiplyInThread);
                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);


            // Число потоков 10
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("M=10");
            Console.WriteLine("-----------------------------------------");
            M = 10;
            for (int j = 0; j < 10; j++)
            {
                dt1 = DateTime.Now;
                mThreadPool = new Thread[M];
                for (int i = 0; i < M; i++)
                {
                    int[] lArgs = new int[3];
                    lArgs[0] = x;
                    lArgs[1] = (N / M) * i;
                    if ((N / M) * (i + 1) > N - 1)
                    {
                        lArgs[2] =N;

                    }
                    else {
                        lArgs[2] = (N / M) * (i + 1);
                    }
                    mThreadPool[i] = new Thread(MultiplyInThread);
                    mThreadPool[i].Start(lArgs);
                }
                for (int i = 0; i < M; i++)
                {
                    mThreadPool[i].Join();
                }
                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }
            Console.WriteLine("Время: " + lAllTime / 10);



        }





        static void Main(string[] args)
        {
            Console.WriteLine("*****************************************");
            Console.WriteLine("ProcessorCount=" + System.Environment.ProcessorCount);
            Console.WriteLine("*****************************************");
            /*PerformCalc(10);
             PerformCalc(100);
             PerformCalc(1000);
             PerformCalc(100000);
             PerformCalc(1000000);*/
            PerformCalcCircle(100000);
            //PerformCalc(10);
         //   PerformCalc(100);

           // PerformCalc(1000);
           // PerformCalc(10000);
            // PerformCalc(1000000000);
            //PerformCalc(10000);
            //  PerformCalc(100000);
            Console.ReadLine();
          









        }
    }
}
