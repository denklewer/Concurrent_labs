using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ConcLab6
{
    class Program
    {
        /// <summary>
        /// Класс ребра графа
        /// </summary>
        public class Edge
        {        
            public int U; 
            public int V;
            public double Weight;
        }

        #region Conclab6

        public class MergeKruskal
        {
            private static List<Edge> Edges;
            private List<int> Vertexes;
            private List<Edge> tree;

            private Dictionary<int, int> sets;
            private int EdgesCount;
            private int VerticlesCount;

            public double Cost { get; private set; }

            public MergeKruskal(Kruskal[] input)
            {
                tree = new List<Edge>();
                Edges = new List<Edge>();
                Vertexes = new List<int>();


                for (int i = 0; i < input.Length; i++) {
                    Edges.AddRange(input[i].tree);
                 
                    
                       Vertexes= Vertexes.Union(input[i].Vertexes).ToList<int>();
                    
                }
                Edges.Sort((x, y) => (int)(x.Weight - y.Weight));
                EdgesCount = Edges.Count;
                VerticlesCount = Vertexes.Count();
                sets = new Dictionary<int, int>();

                for (int i = 0; i < VerticlesCount; i++) sets.Add(Vertexes[i], Vertexes[i]);


            }
            private int FindSet(int vertex)
            {
                return (sets[vertex]);
            }

            private void Join(int v1, int v2)
            {
                if (v1 < v2)
                {
                    int tmp = sets[v2];

                    for (int i = 0; i < sets.Count; i++)
                    {
                        if (sets.ElementAt(i).Value == tmp) sets[sets.ElementAt(i).Key] = sets[v1];
                    }

                }
                else {
                    int tmp = sets[v1];

                    for (int i = 0; i < sets.Count; i++)
                    {
                        if (sets.ElementAt(i).Value == tmp) sets[sets.ElementAt(i).Key] = sets[v2];
                    }

                }
            }
            public void BuildSpanningTree()
            {
                int k = EdgesCount;
                int i;
                this.Cost = 0;

                for (i = 0; i < k; i++)
                    if (this.FindSet(Edges[i].U) != this.FindSet(Edges[i].V))
                    {
                        tree.Add(new Edge
                        {
                            U = Edges[i].U,
                            V = Edges[i].V,
                            Weight = Edges[i].Weight
                        });

                        this.Cost += Edges[i].Weight;
                        this.Join(Edges[i].U, Edges[i].V);

                    }
            }


            public void WriteToFile()
            {
                string res = "";
                Console.WriteLine("The Edges of the Minimum Spanning Tree are:");
                for (int i = 0; i < tree.Count; i++)
                    res = res + tree[i].U + " - " + tree[i].V + "\n";

                File.WriteAllText(@"D:/C#/out.txt", "");//переписываем файл
                File.AppendAllText("D:/C#/out.txt", res);
                Console.WriteLine("Файл out записан");
            }




        }





        public class Kruskal
        {
            private List<Edge> Edges;
            public List<int> Vertexes;
            public List<Edge> tree;

            public Dictionary<int, int> sets;
            private int EdgesCount;
            private int VerticlesCount;

            public double Cost { get; private set; }




            public Kruskal() { }
            public Kruskal(string input)
            {
                tree = new List<Edge>();


                string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                EdgesCount = lines.Length;
                Edges = new List<Edge>();
                Vertexes = new List<int>();
                //Edges.Add(null);

                for (int i = 0; i < lines.Count(); i++)
                {
                    string[] line = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    Edges.Add(new Edge
                    {
                        U = int.Parse(line[0]),
                        V = int.Parse(line[1]),
                        Weight = double.Parse(line[2])
                    });
                    Edge temp = Edges.Last();
                    if (!Vertexes.Contains(temp.U)) {
                        Vertexes.Add(Edges.Last().U);

                    }
                    if (!Vertexes.Contains(temp.V)) {
                        Vertexes.Add(Edges.Last().V);
                    }
                }
                VerticlesCount = Vertexes.Count;
                sets = new Dictionary<int, int>();

                for (int i = 0; i < VerticlesCount; i++) sets.Add(Vertexes[i], Vertexes[i]);
            }


            public Kruskal(string[] lines)
            {
             
                    tree = new List<Edge>();

                    EdgesCount = lines.Length;
                    Edges = new List<Edge>();
                    Vertexes = new List<int>();
                    //Edges.Add(null);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string[] line = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        Edges.Add(new Edge
                        {
                            U = int.Parse(line[0]),
                            V = int.Parse(line[1]),
                            Weight = double.Parse(line[2])
                        });

                        Edge temp = Edges.Last();
                        if (!Vertexes.Contains(temp.U))
                        {
                            Vertexes.Add(Edges.Last().U);

                        }
                        if (!Vertexes.Contains(temp.V))
                        {
                            Vertexes.Add(Edges.Last().V);
                        }
                    }
                    VerticlesCount = Vertexes.Count;
                    sets = new Dictionary<int, int>();

                    for (int i = 0; i < VerticlesCount; i++) sets.Add(Vertexes[i], Vertexes[i]);
                
            }

            private void ArrangeEdges()
            {
                Edges.Sort((x, y) => (int)(x.Weight - y.Weight));
            }

            private int FindSet(int vertex)
            {
                return (sets[vertex]);
            }

            private void Join(int v1, int v2)
            {
                if (v1 < v2)
                {
                    int tmp = sets[v2];
                   
                    for (int i = 0; i<sets.Count;i++)
                    {
                        if (sets.ElementAt(i).Value == tmp)  sets[sets.ElementAt(i).Key] = sets[v1];
                    }

                }
                else {
                    int tmp = sets[v1];

                    for (int i = 0; i < sets.Count; i++)
                    {
                        if (sets.ElementAt(i).Value == tmp) sets[sets.ElementAt(i).Key] = sets[v2];
                    }

                }
            }

            public void BuildSpanningTree()
            {
              
                    int k = EdgesCount;
                    int i;
                    this.ArrangeEdges();
                    this.Cost = 0;

                    for (i = 0; i < k; i++)
                        if (this.FindSet(Edges[i].U) != this.FindSet(Edges[i].V))
                        {
                            tree.Add(new Edge
                            {
                                U = Edges[i].U,
                                V = Edges[i].V,
                                Weight = Edges[i].Weight
                            });

                            this.Cost += Edges[i].Weight;
                            this.Join(Edges[i].U, Edges[i].V);

                        }
                
            }

            public void DisplayInfo()
            {
             
                Console.WriteLine("The Edges of the Minimum Spanning Tree are:");
                for (int i = 0; i < tree.Count; i++)
                    Console.WriteLine(tree[i].U + " - " + tree[i].V);
            }

            public void WriteToFile()
            {
                string res = "";
                Console.WriteLine("The Edges of the Minimum Spanning Tree are:");
                for (int i = 0; i < tree.Count; i++)
                    res = res + tree[i].U + " - " + tree[i].V + "\n";

                File.WriteAllText(@"D:/C#/out.txt", "");//переписываем файл
                File.AppendAllText("D:/C#/out.txt", res);
                Console.WriteLine("Файл out записан");
            }


        }


      
 

        public static void PerformSingleKruskal(string FileName) {
            string input = File.ReadAllText(FileName);
            Kruskal k = new Kruskal(input);
            k.BuildSpanningTree();
            Console.WriteLine("Cost: " + k.Cost);
           // k.DisplayInfo();
 

        }

   
        public static Kruskal[] buffer;
   

        public static void ThreadTask(Object Args)
        {
            object[] lArgs = (object[])Args;


            int ThreadNum = (int)(lArgs[0]);
            string[] lines = (string[])(lArgs[1]);
            ManualResetEvent ev = (lArgs[2] as ManualResetEvent);
            
            buffer[ThreadNum] = new Kruskal(lines);
            buffer[ThreadNum].BuildSpanningTree();
          //  buffer[ThreadNum].DisplayInfo();
         
            ev.Set();
        }

        public static void ThreadTaskWithThread(Object Args)
        {
            object[] lArgs = (object[])Args;


            int ThreadNum = (int)(lArgs[0]);
            string[] lines = (string[])(lArgs[1]);
           

            buffer[ThreadNum] = new Kruskal(lines);
            buffer[ThreadNum].BuildSpanningTree();
            //  buffer[ThreadNum].DisplayInfo();

     
        }

        public static void PerformConcKruskal(string FileName, int M) {
          string []  input = File.ReadAllText(FileName).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int EdgeCount = input.Length;
            ManualResetEvent[] events = new ManualResetEvent[M];

            for (int i = 0; i < M; i++)
            {
                events[i] = new ManualResetEvent(false);
            }

            buffer = new Kruskal[M];
            int ind1, ind2;
                for (int i = 0; i < M; i++)
                {
                    Object[] ThreadInput = new object[3];
               ThreadInput[0] = i;
                if (i == 0)
                {
                    ind1 = 0;
                   
                }
                else {
                   ind1 = EdgeCount / M * i;
                }
                if (i == M - 1)
                {
                    ind2 = EdgeCount;
                }
                else {
                    if ((EdgeCount / M * (i + 1))>EdgeCount)
                       ind2 = EdgeCount;
                    else
                    {
                        ind2 = EdgeCount / M * (i + 1);
                    }
                }
      
    
                ThreadInput[2] = events[i];
               string[] tmpMas = new string[ind2-ind1];
                for (int j = 0; j < tmpMas.Length; j++) {
                    tmpMas[j] = input[ind1 + j];
                }
                ThreadInput[1] = tmpMas;


                ThreadPool.QueueUserWorkItem(ThreadTask, ThreadInput);

            
                //  input.CopyTo(tmpMas, 0);
                // ThreadInput[4] = tmpMas;

            }
            WaitHandle.WaitAll(events);

            MergeKruskal k = new MergeKruskal(buffer);
            k.BuildSpanningTree();
            Console.WriteLine("Cost: " + k.Cost);
         //   k.DisplayInfo();
     


        }
        public static void PerformConcKruskalWithThreads(string FileName, int M)
        {
      


            string[] input = File.ReadAllText(FileName).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int EdgeCount = input.Length;
            Thread[] threads = new Thread[M];

            for (int i = 0; i < M; i++)
            {
                threads[i] = new Thread(ThreadTaskWithThread);
            }

            buffer = new Kruskal[M];
            int ind1, ind2;
            for (int i = 0; i < M; i++)
            {
                Object[] ThreadInput = new object[2];
                ThreadInput[0] = i;
                if (i == 0)
                {
                    ind1 = 0;

                }
                else {
                    ind1 = EdgeCount / M * i;
                }
                if (i == M - 1)
                {
                    ind2 = EdgeCount;
                }
                else {
                    if ((EdgeCount / M * (i + 1)) > EdgeCount)
                        ind2 = EdgeCount;
                    else
                    {
                        ind2 = EdgeCount / M * (i + 1);
                    }
                }


               
                string[] tmpMas = new string[ind2 - ind1];
                for (int j = 0; j < tmpMas.Length; j++)
                {
                    tmpMas[j] = input[ind1 + j];
                }
                ThreadInput[1] = tmpMas;


                threads[i].Start(ThreadInput);


                //  input.CopyTo(tmpMas, 0);
                // ThreadInput[4] = tmpMas;

            }
            for (int i = 0; i < M; i++)
            {
                threads[i].Join();
            }


            MergeKruskal k = new MergeKruskal(buffer);


         
            k.BuildSpanningTree();
            Console.WriteLine("Cost: " + k.Cost);
           // k.DisplayInfo();

    


        }


        public static string GraphGenerate(int n) {
            File.WriteAllText(@"D:/graph1.txt", "");//переписываем файл

           

            int tmp;
            string res="";
            Random r = new Random();
            for (int i = 0; i <n; i++)
            {
                for (int j = 0; j <n; j++)
                {
                 
                    tmp = r.Next(1, 100);
                    res =  i + " " + j + " " + tmp + Environment.NewLine;
                    File.AppendAllText(@"D:/graph1.txt", res);
                }
            };
            return res;







        }
        #endregion

        #region HelpersThreads
        /// <summary>
        /// Класс содержащий в себе реализацию алгоритма
        /// </summary>
        public class HelperKruskal
        {
            private  Edge[] Edges; // Массив рёбер для обхода
            public  List<int> Vertexes;  // Список вершин графа
            public  List<Edge> tree;  // Итоговое минимальное остовное дерево

            // public  Dictionary<int, int> sets;  // Словарь 
            public int[] sets;  //массив для представления принадлежности ребер к поддеревьям
            public  int[] edge_color_main;   // Массив, показывающий  была ли то или иное ребро удалено из обработки алгоритмом для главного потока
            public  int[] edge_color_helper; // Массив, показывающий  была ли то или иное ребро удалено из обработки алгоритмом для вспомогательных потоков
            public  int EdgesCount;  // Количество ребер
            private  int VerticlesCount; //Количество вершин
            public  int CycleEdge = 12; // Константа, обозначающая удаленное из обработки ребро
            public  int MsfEdge = 5;  // Константа, обозначающая ребро, уже добавленное в остовное дерево
            public  bool[] ReachedArr;  // Массив для проверки достижения основным потоком границ обработки вспомогательного потока
            public  Queue<int> ReachedInd;  // Очередь индексов-границ обработки вспомогательных потоков
            
            public  double Cost { get; private set; } // Стоимость итогового маршрута в графе




            /// <summary>
            /// Конструктор для многопоточной версии
            ///   -Разбирает входную строку
            ///   -Инициализирует все члены класса
            /// </summary>
            /// <param name="input"> Входная строка состоящая из строк "Вершина1 Вершина2 Вес"</param>
            /// <param name="ThreadCount">Количество потоков. Нужно для инициализации массива ReachedArr</param>
            public HelperKruskal(string input, int ThreadCount)
            {
                tree = new List<Edge>();
                int curEdge = 0;

                string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                EdgesCount = lines.Length;
                Edges = new Edge[EdgesCount];
                Vertexes = new List<int>();
                for (int i = 0; i < lines.Count(); i++)
                {
                    string[] line = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    Edges[curEdge] = new Edge
                    {
                        U = int.Parse(line[0]),
                        V = int.Parse(line[1]),
                        Weight = double.Parse(line[2])
                    };

                    Edge temp = Edges[curEdge];

                    if (!Vertexes.Contains(temp.U))
                    {
                        Vertexes.Add(Edges[curEdge].U);

                    }
                    if (!Vertexes.Contains(temp.V))
                    {
                        Vertexes.Add(Edges[curEdge].V);
                    }
                    curEdge++;
                }
                VerticlesCount = Vertexes.Count;
                sets = new int [VerticlesCount];

                for (int i = 0; i < VerticlesCount; i++) sets[i]= i;

                edge_color_main = new int[EdgesCount];
                edge_color_helper = new int[EdgesCount];

                for (int i = 0; i < EdgesCount; i++)
                {
                    edge_color_main[i] = 0;


                }

                ReachedArr = new bool[ThreadCount];
                ReachedInd = new Queue<int>();


            }

            /// <summary>
            /// Конструктор для однопоточной версии
            ///   -Разбирает входную строку
            ///   -Инициализирует все члены класса
            /// </summary>
            /// <param name="input"> Входная строка состоящая из строк "Вершина1 Вершина2 Вес"</param>        
            public HelperKruskal(string input)
            {
                tree = new List<Edge>();
                int curEdge = 0;

                string[] lines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                EdgesCount = lines.Length;
                Edges = new Edge[EdgesCount];
                Vertexes = new List<int>();
                //Edges.Add(null);

                for (int i = 0; i < lines.Count(); i++)
                {
                    string[] line = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    Edges[curEdge] = new Edge
                    {
                        U = int.Parse(line[0]),
                        V = int.Parse(line[1]),
                        Weight = double.Parse(line[2])
                    };

                    Edge temp = Edges[curEdge];

                    if (!Vertexes.Contains(temp.U))
                    {
                        Vertexes.Add(Edges[curEdge].U);

                    }
                    if (!Vertexes.Contains(temp.V))
                    {
                        Vertexes.Add(Edges[curEdge].V);
                    }
                    curEdge++;
                }
                VerticlesCount = Vertexes.Count;
                sets = new int[VerticlesCount];

                for (int i = 0; i < VerticlesCount; i++) sets[i]=i;






            }



            /// <summary>
            /// Метод сортировки ребер графа в неубывающем порядке весов
            /// </summary>
            private  void ArrangeEdges()
            {
                Array.Sort(Edges, (x, y) => (int)(x.Weight - y.Weight));

            }

            /// <summary>
            /// Возвращает порядковый номер поддерева, которому принадлежит вершина
            /// </summary>
            /// <param name="vertex">Вершина</param>
            /// <returns></returns>
            private  int FindSet(int vertex)
            {
                return (sets[vertex]);
            }

            /// <summary>
            /// Объединяет поддеревья для вершин v1 и v2.
            /// Номером поддерева становится наименьший  номер поддерева v1 или v2
            /// </summary>
            /// <param name="v1">Вершина 1</param>
            /// <param name="v2">Вершина 2</param>
            private  void Join(int v1, int v2)
            {              
                if (sets[v1] < sets[v2])
                {
                    int tmp = sets[v2];
                    sets[v2] = sets[v1];
                    for (int i = 0; i < sets.Length; i++)
                    {
                        if (sets[i] == tmp)
                        {
                            sets[i] = sets[v1];
                        }
                    }
                }
                else
                {
                    int tmp = sets[v1];
                    sets[v1] = sets[v2];

                    for (int i = 0; i < sets.Length; i++)
                    {
                        if (sets[i] == tmp)
                        {
                            sets[i] = sets[v2];
                        }
                    }

                }
            }

            /// <summary>
            /// Строит минимальное остовное дерево. Многопоточная версия.
            /// </summary>
            public  void BuildSpanningTree()
            {

                int k = EdgesCount;         // Количество ребер
                int i;                      // впомогательная переменная-счетчик
                int threadCounter = 1;      // Счетчик. Индекс последнего потока, до границ которого дошел алгоритм и который нужно остановить.
                Cost = 0;                   // Итоговая стоимость

                // Начало
                //Сортируем ребра
                ArrangeEdges();



                //Цикл по всем ребрам
                
                for (i = 0; i < k; i++) 
                {
                    // Проверка на достижение границ очередного потока
                    if (ReachedInd.Count > 0 && i == ReachedInd.Peek()) 
                    {
                        // Если достигли отмечаем в массиве ReachedArr и увеличиваем счетчик
                        ReachedArr[threadCounter] = true;  
                        ReachedInd.Dequeue();
                        threadCounter++;
                    }

                    // Если ребро не удалено вспомогательным потоком 
                    if (edge_color_helper[i] != CycleEdge)   
                    {
                        // И если его вершины в разных поддеревьях
                        if (FindSet(Edges[i].U) != FindSet(Edges[i].V))  
                        {
                            // то добавляем ребро в итоговое дерево 
                            tree.Add(new Edge
                            {
                                U = Edges[i].U,
                                V = Edges[i].V,
                                Weight = Edges[i].Weight
                            });
                            //увеличиваем итоговую стоимость
                            Cost += Edges[i].Weight;
                            //объединяем  поддеревья вершин нашего ребра
                            Join(Edges[i].U, Edges[i].V);
                            // помечаем ребро как пройденное и добавленную алгоритмом
                            edge_color_main[i] = MsfEdge;
                        }
                        else {
                            // иначе помечаем ребро как удаленное
                            edge_color_main[i] = CycleEdge;

                        }
                    }

                }

                Console.WriteLine("Cost=" + Cost);

            }
            /// <summary>
            /// Строит минимальное остовное дерево. Однопоточная версия.
            /// </summary>
            public void BuildSpanningTreeSingle()
            {

                int k = EdgesCount;     // Количество ребер
                int i;                  // впомогательная переменная-счетчик
                Cost = 0;               // Итоговая стоимость

                // Начало
                //Сортируем ребра
                ArrangeEdges();


                //Цикл по всем ребрам
                for (i = 0; i < k; i++)
                {
                    // Если вершины очередного ребра в разных поддеревьях
                    if (FindSet(Edges[i].U) != FindSet(Edges[i].V))
                    {
                        // то добавляем ребро в итоговое дерево
                        tree.Add(new Edge
                        {
                            U = Edges[i].U,
                            V = Edges[i].V,
                            Weight = Edges[i].Weight
                        });
                        //увеличиваем итоговую стоимость
                        Cost += Edges[i].Weight;
                        //объединяем  поддеревья вершин нашего ребра
                        Join(Edges[i].U, Edges[i].V);
                    }

                }

                Console.WriteLine("Cost=" + Cost);
            }

            /// <summary>
            /// Выводит в консоль минимальное остовное дерево. Построчно, все ребра.
            /// </summary>    
            public  void DisplayInfo()
            {

                Console.WriteLine("The Edges of the Minimum Spanning Tree are:");
                for (int i = 0; i < tree.Count; i++)
                    Console.WriteLine(tree[i].U + " - " + tree[i].V);
            }


            /// <summary>
            /// Записывает минимальное остовное дерево в файл "D:/C#/out.txt". Построчно, все ребра.
            /// </summary>
            public void WriteToFile()
            {
                string res = "";
                Console.WriteLine("The Edges of the Minimum Spanning Tree are:");
                for (int i = 0; i < tree.Count; i++)
                    res = res + tree[i].U + " - " + tree[i].V + "\n";

                File.WriteAllText(@"D:/C#/out.txt", "");//переписываем файл
                File.AppendAllText("D:/C#/out.txt", res);
                Console.WriteLine("Файл out записан");
            }


            /// <summary>
            /// Функция, выполняемая вспомогательным потоком. 
            /// Циклически обходит выданный ей интервал, пока основной поток не сигнализирует потоку с номером reached остановиться
            /// </summary>
            /// <param name="start">Начало интервала ребер</param>
            /// <param name="end">Конец интервала ребер</param>
            /// <param name="reached">Номер потока</param>
            public void HelperFunction(int start,int end, int reached) {
                // цикл. до тех пор пока потоку с номером reached не сигнализировали остановиться (через ReachedArr)
                while (!ReachedArr[reached])
                {
                    //цикл по всем ребрам из интервала (start,end)
                    for (int i = start; i < end; i++)
                    {
                        // Если ребро еще никак не помечено вспомогательным потоком
                        if (edge_color_helper[i] == 0)
                        {
                            // и его вершины в одном поддереве
                            if (FindSet(Edges[i].U) == FindSet(Edges[i].V))
                            {                          
                                //  пометить ребро как удаленное из графа
                                edge_color_helper[i] = CycleEdge;


                            }
                        }
                    }
                }
            }


        }



        public static HelperKruskal mKruskal;  // Переменная-экземпляр класса HelperKruskal


        /// <summary>
        /// Вспомогательный поток, разбирает Args и запускает HelperFunction. У потока должно быть имя-порядковый номер.
        /// </summary>
        /// <param name="Args">входные аргументы для потока: начало интервала, конец интервала</param>
       public static void HelpersThread(Object Args)
        {
            object[] lArgs = (object[])Args;
            int start = (int)(lArgs[0]);
            int end = (int)(lArgs[1]);

            int reached = int.Parse(Thread.CurrentThread.Name);
            mKruskal.HelperFunction(start, end, reached);
        }

        /// <summary>
        /// Основной поток. Запускает работу основного потока (BuildSpanningTree)
        /// 
        /// </summary>           
        public static void MainThread()
        {

            mKruskal.BuildSpanningTree();
        }


        /// <summary>
        /// Читает файл FileName. Создает 1 основной поток и M дополнительных. Запускает работу многопоточной версии алгоритма.
        /// </summary>
        /// <param name="FileName">Имя входного файла</param>
        /// <param name="M">Количество потоков</param>
        public static void PerformHelpersKruskal(string FileName, int M)
        {
            // читаем файл
            string input = File.ReadAllText(FileName);
            //инициализируем структуру для алгоритма
           mKruskal=new HelperKruskal(input,M);
            // создаем потоки
            Thread[] threads = new Thread[M];
            int EdgesCnt = mKruskal.EdgesCount;
            
            threads[0] = new Thread(MainThread);

            for (int i = 1; i < M; i++)
            {
                threads[i] = new Thread(HelpersThread);
            }


            int ind1, ind2;
            for (int i = 1; i < M; i++)
            {
                Object[] ThreadInput = new object[2];

                if (i == 0)
                {
                    ind1 = 0;

                }
                else {
                    ind1 = EdgesCnt / M * i;
                }
                if (i == M - 1)
                {
                    ind2 = EdgesCnt;
                }
                else {
                    if ((EdgesCnt / M * (i + 1)) > EdgesCnt)
                        ind2 = EdgesCnt;
                    else
                    {
                        ind2 = EdgesCnt / M * (i + 1);
                    }
                }
                ThreadInput[0] = ind1;
                mKruskal.ReachedInd.Enqueue(ind1);//индекс начала интервала помещаем в очередь, чтобы обрабатывать остановку потоков
                ThreadInput[1] = ind2;

                threads[i].Name = "" + i;





                // стартуем потоки
                threads[i].Start(ThreadInput);


            }
            threads[0].Start();
            //ждем завершения основного потока
            threads[0].Join();
       
         


        }
        /// <summary>
        /// Читает входной файл. Запускает работу однопоточной версии
        /// </summary>
        /// <param name="FileName">Имя входного файла</param>
        public static void PerformHelpersKruskal(string FileName)
        {


            //читаем файл
            string input = File.ReadAllText(FileName);
            //инициализируем структуру для алгоритма
            mKruskal= new HelperKruskal(input);
            // запускаем работу алгоритма
            mKruskal.BuildSpanningTreeSingle();

        }




        #endregion


        static void Main(string[] args)
        {
            // string s =  GraphGenerate(2000);
            //  Console.ReadLine();

        int tCount = 1;
            string result = "";
            DateTime dt1, dt2;
             double lAllTime;
             
            //***************************************************************************************************
            //         Однопоточная версия
            //***************************************************************************************************
            /*    lAllTime = 0;
              dt1 = DateTime.Now;
            Console.WriteLine("Запущена версия c хелперами однопоточная");
              PerformHelpersKruskal(@"D:/C#/in.txt");    //Запускаем однопоточную версию
              dt2 = DateTime.Now;
              lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
              result += " однопоточная -- время: " + lAllTime + "\n\r";
              Console.WriteLine("Время: " + lAllTime / tCount);*/
            //****************************************************************************************************
            //         Многопотоная версия
            //****************************************************************************************************
            lAllTime = 0;
            dt1 = DateTime.Now;
            Console.WriteLine("Запущена версия c хелперами многопоточная");
            PerformHelpersKruskal(@"D:/C#/in.txt", 2);   //запускаем многопоточную версию
            dt2 = DateTime.Now;
            lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            result += " однопоточная -- время: " + lAllTime + "\n\r";
            Console.WriteLine("Время: " + lAllTime / tCount);



            Console.ReadLine();
          
        }
    }
}
