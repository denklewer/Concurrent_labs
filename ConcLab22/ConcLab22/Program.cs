using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace ConcLab22
{
    class Program
    {



        #region LINQ
        public static Dictionary<string, int> WordFreqLINQ(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new Dictionary<string, int>();
            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };


            DicWordsFreq = StrEnumFiles.
                Select(path => File.ReadAllText(path, System.Text.Encoding.Default))
                .SelectMany(text => text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(word => word.ToLower())
                .ToDictionary(group => group.Key, group => group.Count());
            // DicWordsFreq.Clear();
            return DicWordsFreq;

        }

        public static string[] TopTenWordsLINQ(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");

            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

            string[] StrArrTopTenWords;
            StrArrTopTenWords = StrEnumFiles.
               Select(path => File.ReadAllText(path, System.Text.Encoding.Default))
               .SelectMany(text => text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
               .GroupBy(word => word.ToLower())
               .Select(group => new { Word = group.Key, Count = group.Count() })
               .OrderByDescending(item => item.Count).
               Take(10).Select(pair => pair.Word).ToArray();
            ;

            // DicWordsFreq.Clear();
            return StrArrTopTenWords;

        }

        public static Dictionary<int, int> WordsDistributionLINQ(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");

            var DicWordsDistr = new Dictionary<int, int>();
            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

            DicWordsDistr = StrEnumFiles.
                 Select(path => File.ReadAllText(path, System.Text.Encoding.Default))
                 .SelectMany(text => text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                 .GroupBy(word => word.ToLower())
                 .Select(group => new { Word = group.Key, Freq = group.Count() })
                 .GroupBy(pair => pair.Freq)
                 .ToDictionary(group => group.Key, group => group.Count());




            // DicWordsFreq.Clear();
            return DicWordsDistr;

        }



        #endregion


        #region Single




        public static Dictionary<string, int> WordFreqSingle(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new Dictionary<string, int>();
            String[] wordsArray;
            string FileText;
            foreach (var file in StrEnumFiles)
            {
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };


                FileText = File.ReadAllText(file, System.Text.Encoding.Default);

                wordsArray = FileText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in wordsArray)
                {
                    String lowerWord = word.ToLower();
                    if (!DicWordsFreq.ContainsKey(lowerWord))
                    {
                        DicWordsFreq.Add(lowerWord, 1);

                    }
                    else
                    {
                        DicWordsFreq[lowerWord] += 1;
                    }
                }




            }
            Console.WriteLine("Количество слов" + DicWordsFreq.Values.Sum());
            // DicWordsFreq.Clear();
            return DicWordsFreq;

        }

        public static string[] TopTenWordsSingle(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new Dictionary<string, int>();
            String[] StrArrWords;
            string FileText;
            string MaxWord = "";
            int MaxWordFreq = 0;

            foreach (var file in StrEnumFiles)
            {
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

                FileText = File.ReadAllText(file, System.Text.Encoding.Default);
                StrArrWords = FileText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in StrArrWords)
                {
                    String lowerWord = word.ToLower();
                    if (!DicWordsFreq.ContainsKey(lowerWord))
                    {
                        DicWordsFreq.Add(lowerWord, 1);
                    }
                    else
                    {
                        DicWordsFreq[lowerWord] += 1;
                    }
                }
            }
            string[] StrArrTopTenWords = new string[10];


            for (int i = 0; i < StrArrTopTenWords.Length; i++)
            {
                foreach (var word in DicWordsFreq)
                {
                    if (word.Value > MaxWordFreq)
                    {
                        MaxWordFreq = word.Value;
                        MaxWord = word.Key;
                    }
                }
                StrArrTopTenWords[i] = MaxWord;
                DicWordsFreq.Remove(MaxWord);
                MaxWord = "";
                MaxWordFreq = 0;
            }
            //  DicWordsFreq.Clear();

            return StrArrTopTenWords;
        }



        public static Dictionary<int, int> WordsDistributionSingle(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new Dictionary<string, int>();
            var DicWordsDistrib = new Dictionary<int, int>();
            String[] StrArrWords;
            string FileText;


            foreach (var file in StrEnumFiles)
            {
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

                FileText = File.ReadAllText(file, System.Text.Encoding.Default);


                StrArrWords = FileText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in StrArrWords)
                {
                    String lowerWord = word.ToLower();
                    if (!DicWordsFreq.ContainsKey(lowerWord))
                    {
                        DicWordsFreq.Add(lowerWord, 1);
                    }
                    else
                    {
                        DicWordsFreq[lowerWord] += 1;
                    }
                }
            }

            foreach (var word in DicWordsFreq)
            {
                if (!DicWordsDistrib.ContainsKey(word.Value))
                {
                    DicWordsDistrib.Add(word.Value, 1);
                }
                else
                {
                    DicWordsDistrib[word.Value] += 1;
                }

            }

            return DicWordsDistrib;
        }

        #endregion


        #region ParallelFor




        public static ConcurrentDictionary<string, int> WordFreqParallel(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new ConcurrentDictionary<string, int>();
            String[] wordsArray;
            string FileText;
            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
            Parallel.ForEach(StrEnumFiles, (file) =>
           {
               FileText = File.ReadAllText(file, System.Text.Encoding.Default);
               wordsArray = FileText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
               foreach (var word in wordsArray)
               {
                   String lowerWord = word.ToLower();
                   DicWordsFreq.AddOrUpdate(lowerWord, 1, (sKey, oldValue) => oldValue + 1);
               }

           });
            Console.WriteLine("Количество слов" + DicWordsFreq.Values.Sum());
            // DicWordsFreq.Clear();
            return DicWordsFreq;





        }

        public static string[] TopTenWordsParallel(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new ConcurrentDictionary<string, int>();
            String[] StrArrWords;
            string FileText;
            string MaxWord = "";
            int MaxWordFreq = 0;
            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

            Parallel.ForEach(StrEnumFiles, (file) =>
            {


                FileText = File.ReadAllText(file, System.Text.Encoding.Default);
                StrArrWords = FileText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in StrArrWords)
                {
                    String lowerWord = word.ToLower();
                    DicWordsFreq.AddOrUpdate(lowerWord, 1, (sKey, oldValue) => oldValue + 1);
                }
            });
            string[] StrArrTopTenWords = new string[10];
            for (int i = 0; i < StrArrTopTenWords.Length; i++)
            {
                foreach (var word in DicWordsFreq)
                {
                    if (word.Value > MaxWordFreq)
                    {
                        MaxWordFreq = word.Value;
                        MaxWord = word.Key;
                    }
                }
                StrArrTopTenWords[i] = MaxWord;
                DicWordsFreq.TryRemove(MaxWord, out MaxWordFreq);
                MaxWord = "";
                MaxWordFreq = 0;
            }
            //  DicWordsFreq.Clear();

            return StrArrTopTenWords;
        }



        public static ConcurrentDictionary<int, int> WordsDistributionParallel(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new ConcurrentDictionary<string, int>();
            var DicWordsDistrib = new ConcurrentDictionary<int, int>();
            String[] StrArrWords;
            string FileText;


            Parallel.ForEach(StrEnumFiles, (file) =>
            {
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

                FileText = File.ReadAllText(file, System.Text.Encoding.Default);


                StrArrWords = FileText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in StrArrWords)
                {
                    String lowerWord = word.ToLower();
                    DicWordsFreq.AddOrUpdate(lowerWord, 1, (sKey, oldValue) => oldValue + 1);
                }
            });

            Parallel.ForEach(DicWordsFreq, (word) =>
            {
                DicWordsDistrib.AddOrUpdate(word.Value, 1, (sKey, oldValue) => oldValue + 1);

            });

            return DicWordsDistrib;
        }

        #endregion


        #region PLINQ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrDirectory"></param>
        /// <param name="BuffMode"> 0 - AutoBuffered, 1 - FullyBuffered, 2 - NotBuffered </param>
        /// <param name="DecMode">0 - default , 1 - block decomposition</param>
        /// <returns></returns>
     

        public static Dictionary<string, int> WordFreqPLINQ(string StrDirectory, int BuffMode, int DecMode)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");
            var DicWordsFreq = new Dictionary<string, int>();
            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
            ConcurrentDictionary<int, int> thr = new ConcurrentDictionary<int, int>();

            switch (DecMode)
            {
                case 0:
                    {
                        switch (BuffMode)
                        {
                            case 0:
                                {
                                    DicWordsFreq = StrEnumFiles.AsParallel().
                                    Select(path =>
                                    {
                                        thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                        return File.ReadAllText(path, System.Text.Encoding.Default);
                                    })
                                        .SelectMany(text =>
                                        {
                                            thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId,1, (sKey, oldValue) => oldValue + 1);
                                            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        })
                                            .GroupBy(word => word.ToLower())
                                            .ToDictionary(group => group.Key, group => group.Count());
                                    break;
                                }
                            case 1:
                                {
                                    DicWordsFreq = StrEnumFiles.AsParallel().WithMergeOptions(ParallelMergeOptions.FullyBuffered).
                                    Select(path =>
                                    {
                                        thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                        return File.ReadAllText(path, System.Text.Encoding.Default);
                                    })
                                        .SelectMany(text =>
                                        {
                                            thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        })
                                            .GroupBy(word => word.ToLower())
                                            .ToDictionary(group => group.Key, group => group.Count());
                                    break;
                                }
                            case 2:
                                {
                                    DicWordsFreq = StrEnumFiles.AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered).
                                    Select(path =>
                                    {
                                        thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                        return File.ReadAllText(path, System.Text.Encoding.Default);
                                    })
                                        .SelectMany(text =>
                                        {
                                            thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        })
                                            .GroupBy(word => word.ToLower())
                                            .ToDictionary(group => group.Key, group => group.Count());
                                    break;
                                }

                            default:
                                break;
                        };
                        break;
                    }
                case 1:
                    {
                        switch (BuffMode)
                        {
                            case 0:
                                {
                                    DicWordsFreq = Partitioner.Create(StrEnumFiles).AsParallel().
                                    Select(path =>
                                    {
                                        thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                        return File.ReadAllText(path, System.Text.Encoding.Default);
                                    })
                                        .SelectMany(text =>
                                        {
                                            thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        })
                                            .GroupBy(word => word.ToLower())
                                            .ToDictionary(group => group.Key, group => group.Count());
                                    break;
                                }
                            case 1:
                                {
                                    DicWordsFreq = Partitioner.Create(StrEnumFiles).AsParallel().WithMergeOptions(ParallelMergeOptions.FullyBuffered).
                                    Select(path =>
                                    {
                                        thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                        return File.ReadAllText(path, System.Text.Encoding.Default);
                                    })
                                        .SelectMany(text =>
                                        {
                                            thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        })
                                            .GroupBy(word => word.ToLower())
                                            .ToDictionary(group => group.Key, group => group.Count());
                                    break;
                                }
                            case 2:
                                {
                                    DicWordsFreq = Partitioner.Create(StrEnumFiles).AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered).
                                    Select(path =>
                                    {
                                        thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                        return File.ReadAllText(path, System.Text.Encoding.Default);
                                    })
                                        .SelectMany(text =>
                                        {
                                            thr.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (sKey, oldValue) => oldValue + 1);
                                            return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        })
                                            .GroupBy(word => word.ToLower())
                                            .ToDictionary(group => group.Key, group => group.Count());
                                    break;
                                }

                            default:
                                break;
                        };
                        break;
                    }


                default:
                    break;
            }
            /* DicWordsFreq = StrEnumFiles.AsParallel().
                 Select(path =>
                 {

                     //Console.WriteLine(Thread.CurrentThread.ManagedThreadId);



                     return File.ReadAllText(path, System.Text.Encoding.Default);


                 })
                 .SelectMany(text =>
                 {

                     // Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                     return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                 })
                 .GroupBy(word => word.ToLower())
                 .ToDictionary(group => group.Key, group => group.Count());
             //
             */

            Console.WriteLine("Количество потоков : " + thr.Count);
            DicWordsFreq.Clear();


            return DicWordsFreq;

        }



        public static string[] TopTenWordsPLINQ(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");

            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

            string[] StrArrTopTenWords;
            StrArrTopTenWords = StrEnumFiles.AsParallel().
               Select(path => File.ReadAllText(path, System.Text.Encoding.Default))
               .SelectMany(text => text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
               .GroupBy(word => word.ToLower())
               .Select(group => new { Word = group.Key, Count = group.Count() })
               .OrderByDescending(item => item.Count).
               Take(10).Select(pair => pair.Word).ToArray();
            ;

            // DicWordsFreq.Clear();
            return StrArrTopTenWords;

        }

        public static Dictionary<int, int> WordsDistributionPLINQ(string StrDirectory)
        {
            var StrEnumFiles = Directory.EnumerateFiles(StrDirectory, "*.txt");

            var DicWordsDistr = new Dictionary<int, int>();
            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

            DicWordsDistr = StrEnumFiles.
                 Select(path => File.ReadAllText(path, System.Text.Encoding.Default))
                 .SelectMany(text => text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                 .GroupBy(word => word.ToLower())
                 .Select(group => new { Word = group.Key, Freq = group.Count() })
                 .GroupBy(pair => pair.Freq)
                 .ToDictionary(group => group.Key, group => group.Count());




            // DicWordsFreq.Clear();
            return DicWordsDistr;

        }



        #endregion

        static void Main(string[] args)
        {
            DateTime dt1, dt2;
            double lAllTime = 0;
            int mode = 3;

            if (mode == 0)
            {
                #region Single
                Console.WriteLine("Однопоточный режим");
                Console.WriteLine("========Частота слов==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    WordFreqSingle(@"D:\C#\Texts");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10); 
                 Console.WriteLine("========Top-10 слов==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    TopTenWordsSingle(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("========Распределение слов по частотам==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    WordsDistributionSingle(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                #endregion
            }
            else if (mode == 1)
            {
                #region LINQ
                Console.WriteLine("LINQ режим");
                Console.WriteLine("========Частота слов==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    WordFreqLINQ(@"D:\C#\Texts");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("========Top-10 слов==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    TopTenWordsLINQ(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("========Распределение слов по частотам==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    WordsDistributionLINQ(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                #endregion
            }
            else if (mode == 2)
            {
                #region ParralelFor
                Console.WriteLine("ParallelFor режим");
                Console.WriteLine("========Частота слов==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    WordFreqParallel(@"D:\C#\Texts");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("========Top-10 слов==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    TopTenWordsParallel(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("========Распределение слов по частотам==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    WordsDistributionParallel(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                #endregion
            }
            else if (mode == 3)
            {
                #region PLINQ
                Console.WriteLine("PLINQ режим");
                Console.WriteLine("========Частота слов==============");
                Console.WriteLine("по диапазону, автобуферизация");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {

                    dt1 = DateTime.Now;
                    WordFreqPLINQ(@"D:\C#\Texts",0,0);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("по диапазону, полная буфферизация");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {

                    dt1 = DateTime.Now;
                    WordFreqPLINQ(@"D:\C#\Texts", 1, 0);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("по диапазону, без буфферизации");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {

                    dt1 = DateTime.Now;
                    WordFreqPLINQ(@"D:\C#\Texts", 2, 0);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("блочная, автобуферизация");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {

                    dt1 = DateTime.Now;
                    WordFreqPLINQ(@"D:\C#\Texts", 0, 1);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("блочная, полная");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {

                    dt1 = DateTime.Now;
                    WordFreqPLINQ(@"D:\C#\Texts", 1, 1);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("блочная, без буфферизации");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {

                    dt1 = DateTime.Now;
                    WordFreqPLINQ(@"D:\C#\Texts", 2, 1);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("========Top-10 слов==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    TopTenWordsPLINQ(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                Console.WriteLine("========Распределение слов по частотам==============");
                lAllTime = 0;
                for (int i = 0; i < 10; i++)
                {
                    dt1 = DateTime.Now;
                    WordsDistributionPLINQ(@"D:\C#\Texts2");
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / 10);
                #endregion
            }
            Console.ReadLine();



        }
    }
}
