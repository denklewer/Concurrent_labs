using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Concurrent;
using System.Threading;
using System.Text.RegularExpressions;

namespace ConcLab4
{
    class Program
    {
        #region Обычные коллекции
        static Dictionary<string,int> TotalWordDic = new Dictionary<string, int>();
     static   Dictionary<char, int> TotalLetterDic = new Dictionary<char, int>();
  

     
     static   int TotalSentenceCount;


        static Queue<string> LinesBuffer = new Queue<string>();
        static StreamReader sr;
        static int currentFile = -1;
        static int LinesCount = 0;
       static Regex reg = new Regex(@"(((\.)|(\!)|(\?)) )|((\.)|(\!)|(\?))$");


        public static void ReadTextFiles(Object args)
        {
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            int start = (int)(lArgs[1]);
            int end = (int)(lArgs[2]);
            int WordsCount = 0;
            int LettersCount = 0;
            int SentencesCount = 0;
            var WordDic = new Dictionary<string, int>();
            var LetterDic = new Dictionary<char, int>();
            int RowCount=0;

            for (int i = start;i< end;i++) { 
                  
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
                using (StreamReader sr = new StreamReader(paths[i], System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        RowCount++;
                        String[] wordsArray = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        Char[] letterArray = line.ToCharArray();

                        foreach (var word in wordsArray)
                        {
                          
                            String lowerWord = word.ToLower();
                            WordsCount++;
                            if (!WordDic.ContainsKey(lowerWord))
                            {

                                WordDic.Add(lowerWord, 1);

                            }
                            else
                            {

                                WordDic[lowerWord] += 1;
                            }
                        }
                        foreach (var letter in letterArray)
                        {
                           
                            LettersCount++;
                            if (!LetterDic.ContainsKey(letter))
                            {

                                LetterDic.Add(letter, 1);
                            }
                            else
                            {

                                LetterDic[letter] += 1;
                            }
                        }

                    
                        MatchCollection matches = reg.Matches(line);
                        SentencesCount += matches.Count;

                    }


                }
                
        

                
         
            
          
                

       
            }
            Console.WriteLine("Количество слов - " + WordDic.Values.Sum());
            Console.WriteLine("Количество букв - " + LetterDic.Values.Sum());
            Console.WriteLine("Количество предложений - " + SentencesCount);
            Console.WriteLine("Количество строк - " + RowCount);



        }


        public static void Alg11_Thread(Object args)
        {
  
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            int start = (int)(lArgs[1]);
            int end = (int)(lArgs[2]);
            int WordsCount = 0;
            int LettersCount = 0;
            int SentencesCount = 0;
            var dic = new Dictionary<string, int>();
            var LetterDic = new Dictionary<char, int>();
            for (int i = start; i < end; i++)
            {           
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
                using (StreamReader sr = new StreamReader(paths[i], System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] wordsArray = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        Char[] letterArray = line.ToCharArray();

                        foreach (var word in wordsArray)
                        {

                            String lowerWord = word.ToLower();
                            WordsCount++;
                            if (!dic.ContainsKey(lowerWord))
                            {

                                dic.Add(lowerWord, 1);

                            }
                            else
                            {

                                dic[lowerWord] += 1;
                            }
                        }
                        foreach (var letter in letterArray)
                        {

                            LettersCount++;
                            if (!LetterDic.ContainsKey(letter))
                            {

                                LetterDic.Add(letter, 1);
                            }
                            else
                            {

                                LetterDic[letter] += 1;
                            }
                        }
                  
                      
                        MatchCollection matches = reg.Matches(line);
                        SentencesCount += matches.Count;

                    }
                }

                
             
                
            
              




            }
         /*   Console.WriteLine(Thread.CurrentThread.Name + ": Количество слов - " + WordsCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество букв - " + LettersCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество предложений - " + SentencesCount);
            */
            //merge
            foreach (var letter in LetterDic)
            {
                if (!TotalLetterDic.ContainsKey(letter.Key))
                {
                    lock ("merge")
                    {
                        TotalLetterDic.Add(letter.Key, letter.Value);
                    }
                }
                else
                {
                    lock ("merge")
                    {
                        TotalLetterDic[letter.Key] += letter.Value;
                    }
                }

            }
            foreach (var word in dic)
            {
                if (!TotalWordDic.ContainsKey(word.Key))
                {
                    lock ("merge2")
                    {
                        TotalWordDic.Add(word.Key, word.Value);
                    }
                }
                else
                {
                    lock ("merge2")
                    {
                        TotalWordDic[word.Key] += word.Value;
                    }
                }

            }

            lock ("merge3")
            {
                TotalSentenceCount += SentencesCount;
            }



        }



        public static void Alg11(int M,string [] paths) {
            TotalWordDic = new Dictionary<string, int>();
            TotalLetterDic = new Dictionary<char, int>();
            TotalSentenceCount = 0;
            Thread[] threads = new Thread[M];
            int n = paths.Length;
        
            int start;
            int end;
            for (int i = 0; i < M; i++) {
                threads[i] = new Thread(Alg11_Thread);
                threads[i].Name = "Поток#" + i;
                

            }


            for (int i = 0; i < M; i++)
            {
                start = i * (n / M);
                end = i == (M - 1) ? n : (n / M) * (i + 1);
                Object[] input = new object[3];
                input[0] = (object)(paths);
                input[1] = start;
                input[2] = end;
                threads[i].Start(input);
            }

            for (int i = 0; i < M; i++) {
                threads[i].Join();
            }


          /*  Console.WriteLine("Всего");
            Console.WriteLine( " Количество слов - " + TotalWordDic.Sum(word => word.Value));
            Console.WriteLine(" Количество букв - " + TotalLetterDic.Values.Sum());
            Console.WriteLine( " Количество предложений - " + TotalSentenceCount);*/
        }


        public static void Alg12_Thread(Object args)
        {
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            int start = (int)(lArgs[1]);
            int end = (int)(lArgs[2]);
            int WordsCount = 0;
            int LettersCount = 0;
            int SentencesCount = 0;
            for (int i = start; i < end; i++)
            {
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
                using (StreamReader sr = new StreamReader(paths[i], System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] wordsArray = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        Char[] letterArray = line.ToCharArray();

                        foreach (var word in wordsArray)
                        {

                            String lowerWord = word.ToLower();
                            WordsCount++;
                           
                                if (!TotalWordDic.ContainsKey(lowerWord))
                                {
                                lock ("merge1")
                                {
                                    if (!TotalWordDic.ContainsKey(lowerWord))
                                    {
                                        TotalWordDic.Add(lowerWord, 1);
                                    }
                                    else {
                                        TotalWordDic[lowerWord] += 1;
                                    }
                                }
                                }
                                else
                                {
                                lock ("merge1")
                                {
                                    TotalWordDic[lowerWord] += 1;
                                }
                                }
                            
                        }
                        foreach (var letter in letterArray)
                        {

                            LettersCount++;
                           
                                if (!TotalLetterDic.ContainsKey(letter))
                                {
                                lock ("merge2")
                                {
                                    if (!TotalLetterDic.ContainsKey(letter))
                                    {
                                        TotalLetterDic.Add(letter, 1);
                                    }
                                    else {
                                        TotalLetterDic[letter] += 1;
                                    }
                                }
                                }
                                else
                                {
                                lock ("merge2")
                                {
                                    TotalLetterDic[letter] += 1;
                                }
                                }
                           
                        }

                     
                        MatchCollection matches = reg.Matches(line);
                        SentencesCount += matches.Count;
                        if (matches.Count > 0)
                        {
                            lock ("merge3")
                            {

                                TotalSentenceCount += matches.Count;
                            }
                        }
                    }
                }


               
          



            }
       /*     Console.WriteLine(Thread.CurrentThread.Name + ": Количество слов - " + WordsCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество букв - " + LettersCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество предложений - " + SentencesCount);*/

            //merge


        }
        public static void Alg12(int M, string[] paths)
        {
            TotalWordDic = new Dictionary<string, int>();
            TotalLetterDic = new Dictionary<char, int>();
            TotalSentenceCount = 0;
            Thread[] threads = new Thread[M];
            int n = paths.Length;

            int start;
            int end;
            for (int i = 0; i < M; i++)
            {
                threads[i] = new Thread(Alg12_Thread);
                threads[i].Name = "Поток#" + i;


            }


            for (int i = 0; i < M; i++)
            {
                start = i * (n / M);
                end = i == (M - 1) ? n : (n / M) * (i + 1);
                Object[] input = new object[3];
                input[0] = (object)(paths);
                input[1] = start;
                input[2] = end;
                threads[i].Start(input);
            }

            for (int i = 0; i < M; i++)
            {
                threads[i].Join();
            }


/*
            Console.WriteLine(" Количество слов - " + TotalWordDic.Sum(word => word.Value));
            Console.WriteLine(" Количество букв - " + TotalLetterDic.Values.Sum());
            Console.WriteLine(" Количество предложений - " + TotalSentenceCount);*/
        }



  
        public static void Alg21_ReadThread(Object args)
        {
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            string line;         
            while (currentFile<paths.Length)
            {
                lock("ReadLine")
                {
                    
                    line = sr.ReadLine();
                    if (line == null)
                    {
                        if (currentFile < paths.Length - 1)
                        {
                            currentFile++;
                            sr.Dispose();
                            sr = new StreamReader(paths[currentFile], System.Text.Encoding.Default);
                            line = sr.ReadLine();
                        }

                        else break;
                  
                    }
                
               
                    LinesCount++;
               
                    LinesBuffer.Enqueue(line);
                }

                }
        

            }
        public static void Alg21_ParseThread(Object args)
        {
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            int WordsCount = 0;
            int LettersCount = 0;
            int SentencesCount = 0;
            string line;

            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };

         
            while ( !((currentFile==paths.Length-1) && (LinesBuffer.Count == 0)))
            {
                lock ("ReadLine")
                {
                    line = LinesBuffer.Dequeue();
                }
                String[] wordsArray = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                Char[] letterArray = line.ToCharArray();

                foreach (var word in wordsArray)
                {

                    String lowerWord = word.ToLower();
                    WordsCount++;
                    if (!TotalWordDic.ContainsKey(lowerWord))
                    {
                        lock ("merge1")
                        {
                            if (!TotalWordDic.ContainsKey(lowerWord))
                            {
                                TotalWordDic.Add(lowerWord, 1);
                            }
                            else {
                                TotalWordDic[lowerWord] += 1;
                            }
                        }
                    }
                    else
                    {
                        lock ("merge1")
                        {
                            TotalWordDic[lowerWord] += 1;
                        }
                    }

                }
                foreach (var letter in letterArray)
                {
                    LettersCount++;
                    if (!TotalLetterDic.ContainsKey(letter))
                    {
                        lock ("merge2")
                        {
                            if (!TotalLetterDic.ContainsKey(letter))
                            {
                                TotalLetterDic.Add(letter, 1);
                            }
                            else {
                                TotalLetterDic[letter] += 1;
                            }
                        }
                    }
                    else
                    {
                        lock ("merge2")
                        {
                            TotalLetterDic[letter] += 1;
                        }
                    }
                }

                Char[] SentenceSeparators = { '.', '!', '?' };



                MatchCollection matches = reg.Matches(line);
                if (matches.Count>0)
                {
                    SentencesCount += matches.Count;
                    lock ("merge3")
                {
                   
                    TotalSentenceCount += matches.Count;
                   
                }
                }



            }



        }


        public static void Alg21(int P,int C, string[] paths)
        {
            TotalWordDic = new Dictionary<string, int>();
            TotalLetterDic = new Dictionary<char, int>();
            TotalSentenceCount = 0;
            Thread[] readthreads = new Thread[P];
            Thread[] parsethreads = new Thread[C];

            currentFile =0;
            sr = new StreamReader(paths[0], System.Text.Encoding.Default);
            for (int i = 0; i < P; i++)
            {
                readthreads[i] = new Thread(Alg21_ReadThread);
                readthreads[i].Name = "Поток#" + i;


            }
            for (int i = 0; i < C; i++)
            {
                parsethreads[i] = new Thread(Alg21_ParseThread);
                parsethreads[i].Name = "Поток#2-" + i;


            }


            for (int i = 0; i < P; i++)
            {
                Object[] input = new object[1];
                input[0] = (object)(paths);       
                readthreads[i].Start(input);
             
            }
            for (int i = 0; i < C; i++)
            {
                Object[] input = new object[1];
                input[0] = (object)(paths);

                parsethreads[i].Start(input);
            }

            for (int i = 0; i < P; i++)
            {
                readthreads[i].Join();
            }
            for (int i = 0; i < C; i++)
            {
                parsethreads[i].Join();
            }


          /*   Console.WriteLine(" Количество слов - " + TotalWordDic.Sum(word => word.Value));
             Console.WriteLine(" Количество букв - " + TotalLetterDic.Values.Sum());
             Console.WriteLine(" Количество предложений - " + TotalSentenceCount);
            Console.WriteLine("Количество строк - " + LinesCount);*/
        }





        #endregion


        #region Конкуррентные коллекции
        static ConcurrentDictionary<string, int> TotalWordConcDic = new ConcurrentDictionary<string, int>();
        static ConcurrentDictionary<char, int> TotalLetterConcDic = new ConcurrentDictionary<char, int>();
        static ConcurrentQueue<string> LinesBufferConc = new ConcurrentQueue<string>();

        public static void Alg11Conc_Thread(Object args)
        {
           
            
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            int start = (int)(lArgs[1]);
            int end = (int)(lArgs[2]);
            int WordsCount = 0;
            int LettersCount = 0;
            int SentencesCount = 0;
            var dic = new ConcurrentDictionary<string, int>();
            var LetterDic = new ConcurrentDictionary<char, int>();
            for (int i = start; i < end; i++)
            {
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
                using (StreamReader sr = new StreamReader(paths[i], System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] wordsArray = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        Char[] letterArray = line.ToCharArray();

                        foreach (var word in wordsArray)
                        {

                            String lowerWord = word.ToLower();
                            WordsCount++;
                            dic.AddOrUpdate(lowerWord, 1, (sKey, oldValue) => oldValue + 1);
                        }
                        foreach (var letter in letterArray)
                        {

                            LettersCount++;                  
                            LetterDic.AddOrUpdate(letter, 1, (sKey, oldValue) => oldValue + 1);
                        }


                        MatchCollection matches = reg.Matches(line);
                        SentencesCount += matches.Count;

                    }
                }










            }
         /*   Console.WriteLine(Thread.CurrentThread.Name + ": Количество слов - " + WordsCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество букв - " + LettersCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество предложений - " + SentencesCount);
            */
            //merge
            foreach (var letter in LetterDic)
            {
                TotalLetterConcDic.AddOrUpdate(letter.Key, letter.Value, (sKey, oldValue) => oldValue + letter.Value);
            }
            foreach (var word in dic)
            {
                TotalWordConcDic.AddOrUpdate(word.Key, word.Value, (sKey, oldValue) => oldValue + word.Value);           
            }

            lock ("merge3")
            {

                TotalSentenceCount += SentencesCount;
            }



        }



        public static void Alg11Conc(int M, string[] paths)
        {
            TotalWordConcDic = new ConcurrentDictionary<string, int>();
            TotalLetterConcDic = new ConcurrentDictionary<char, int>();
            TotalSentenceCount = 0;
            Thread[] threads = new Thread[M];
            int n = paths.Length;

            int start;
            int end;
            for (int i = 0; i < M; i++)
            {
                threads[i] = new Thread(Alg11Conc_Thread);
                threads[i].Name = "Поток#" + i;


            }


            for (int i = 0; i < M; i++)
            {
                start = i * (n / M);
                end = i == (M - 1) ? n : (n / M) * (i + 1);
                Object[] input = new object[3];
                input[0] = (object)(paths);
                input[1] = start;
                input[2] = end;
                threads[i].Start(input);
            }

            for (int i = 0; i < M; i++)
            {
                threads[i].Join();
            }


         /*   Console.WriteLine("Всего");
            Console.WriteLine(" Количество слов - " + TotalWordConcDic.Sum(word => word.Value));
            Console.WriteLine(" Количество букв - " + TotalLetterConcDic.Values.Sum());
            Console.WriteLine(" Количество предложений - " + TotalSentenceCount);*/
        }

        public static void Alg12Conc_Thread(Object args)
        {
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            int start = (int)(lArgs[1]);
            int end = (int)(lArgs[2]);
            int WordsCount = 0;
            int LettersCount = 0;
            int SentencesCount = 0;
            for (int i = start; i < end; i++)
            {
                Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
                using (StreamReader sr = new StreamReader(paths[i], System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] wordsArray = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        Char[] letterArray = line.ToCharArray();

                        foreach (var word in wordsArray)
                        {
                            String lowerWord = word.ToLower();
                            WordsCount++;
                            TotalWordConcDic.AddOrUpdate(lowerWord, 1, (sKey, oldValue) => oldValue + 1);                           
                        }
                        foreach (var letter in letterArray)
                        {
                            LettersCount++;
                            TotalLetterConcDic.AddOrUpdate(letter, 1, (sKey, oldValue) => oldValue + 1);
                        }


                        MatchCollection matches = reg.Matches(line);
                        SentencesCount += matches.Count;
                        if (matches.Count > 0)
                        {
                            lock ("merge3")
                            {
                                TotalSentenceCount += matches.Count;
                            }
                        }
                    }
                }







            }
        /*    Console.WriteLine(Thread.CurrentThread.Name + ": Количество слов - " + WordsCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество букв - " + LettersCount);
            Console.WriteLine(Thread.CurrentThread.Name + ": Количество предложений - " + SentencesCount);*/

            //merge


        }
        public static void Alg12Conc(int M, string[] paths)
        {
            TotalWordConcDic = new ConcurrentDictionary<string, int>();
            TotalLetterConcDic = new ConcurrentDictionary<char, int>();
            TotalSentenceCount = 0;
            Thread[] threads = new Thread[M];
            int n = paths.Length;

            int start;
            int end;
            for (int i = 0; i < M; i++)
            {
                threads[i] = new Thread(Alg12Conc_Thread);
                threads[i].Name = "Поток#" + i;


            }


            for (int i = 0; i < M; i++)
            {
                start = i * (n / M);
                end = i == (M - 1) ? n : (n / M) * (i + 1);
                Object[] input = new object[3];
                input[0] = (object)(paths);
                input[1] = start;
                input[2] = end;
                threads[i].Start(input);
            }

            for (int i = 0; i < M; i++)
            {
                threads[i].Join();
            }


        /*    Console.WriteLine("Всего");
            Console.WriteLine(" Количество слов - " + TotalWordDic.Sum(word => word.Value));
            Console.WriteLine(" Количество букв - " + TotalLetterDic.Values.Sum());
            Console.WriteLine(" Количество предложений - " + TotalSentenceCount);*/
        }




        public static void Alg21Conc_ReadThread(Object args)
        {
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            string line;
            while (currentFile < paths.Length)
            {
                lock ("ReadLine")
                {

                    line = sr.ReadLine();

                    if (line == null)
                    {
                        if (currentFile < paths.Length - 1)
                        {
                            currentFile++;
                            sr.Dispose();
                            sr = new StreamReader(paths[currentFile], System.Text.Encoding.Default);
                            line = sr.ReadLine();
                        }
                        else
                        {
                          
                            break;
                          
                        }                
                    }
                    LinesCount++;
                }

                    LinesBufferConc.Enqueue(line);
          

            }


        }
        public static void Alg21Conc_ParseThread(Object args)
        {
            object[] lArgs = (object[])args;
            string[] paths = (string[])(lArgs[0]);
            int WordsCount = 0;
            int LettersCount = 0;
            int SentencesCount = 0;
            string line;
          
            Char[] separators = { ' ', ',', '-', '.', '!', '?', '\"', '\n', '\r' };
            int n = paths.Length;
            bool flag = true;
            while (!(LinesBufferConc.IsEmpty && currentFile>n-2))
            {
                flag = LinesBufferConc.TryDequeue(out line);

                if (!flag) {
            
                    continue;
                }
         
                String[] wordsArray = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                Char[] letterArray = line.ToCharArray();

                foreach (var word in wordsArray)
                {

                    String lowerWord = word.ToLower();
                    WordsCount++;
                    TotalWordConcDic.AddOrUpdate(lowerWord, 1, (sKey, oldValue) => oldValue + 1);
                
                }
                foreach (var letter in letterArray)
                {
                    LettersCount++;
                    TotalLetterConcDic.AddOrUpdate(letter, 1, (sKey, oldValue) => oldValue + 1);                                         
                }

                MatchCollection matches = reg.Matches(line);
                if (matches.Count > 0)
                {
                    SentencesCount += matches.Count;
                    lock ("merge3")
                    {

                        TotalSentenceCount += matches.Count;
                    }
                }



            }



        }


        public static void Alg21Conc(int P,int C, string[] paths)
        {
            TotalWordConcDic = new ConcurrentDictionary<string, int>();
            TotalLetterConcDic = new ConcurrentDictionary<char, int>();
            TotalSentenceCount = 0;
            LinesBufferConc = new ConcurrentQueue<string>();
            LinesCount = 0;
       
            Thread[] readthreads = new Thread[P];
            Thread[] parsethreads = new Thread[C];

            currentFile = 0;
            sr = new StreamReader(paths[0], System.Text.Encoding.Default);
            for (int i = 0; i < P; i++)
            {
                readthreads[i] = new Thread(Alg21Conc_ReadThread);
                readthreads[i].Name = "Поток#" + i;


            }
            for (int i = 0; i < C; i++)
            {
                parsethreads[i] = new Thread(Alg21Conc_ParseThread);
                parsethreads[i].Name = "Поток#2-" + i;


            }


            for (int i = 0; i < P; i++)
            {
                Object[] input = new object[1];
                input[0] = (object)(paths);
                readthreads[i].Start(input);

            }
            for (int i = 0; i < C; i++)
            {
                Object[] input = new object[1];
                input[0] = (object)(paths);

                parsethreads[i].Start(input);
            }

            for (int i = 0; i < P; i++)
            {
                readthreads[i].Join();
            }
            for (int i = 0; i < C; i++)
            {
                parsethreads[i].Join();
            }

         /*   Console.WriteLine(" Количество слов - " + TotalWordConcDic.Sum(word => word.Value));
            Console.WriteLine(" Количество букв - " + TotalLetterConcDic.Values.Sum());
            Console.WriteLine(" Количество предложений - " + TotalSentenceCount);
            Console.WriteLine("Количество строк - " + LinesCount);*/
        }




        #endregion
        static void Main(string[] args)
        {
            string[] paths = new string[25];
            int start = 0;
            int end = paths.Length;
            Object[] input = new object[3];

            input[0] = (object)(paths);
            input[1] = start;
            input[2] = end;
            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = @"D:\C#\file" + i + ".txt";

            }


            int tCount = 3;
            int[] trcount=  { 2, 3, 4, 10, 20} ;
         
            DateTime dt1, dt2;
            double lAllTime;
            lAllTime = 0;
          /*  Console.WriteLine("---------Однопоточная версия--------");
      

            dt1 = DateTime.Now;
            for (int i = 0; i < tCount; i++)
            {

                ReadTextFiles(input);

            dt2 = DateTime.Now;
            lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
           }

            Console.WriteLine("Время: " + lAllTime / tCount);
            Console.WriteLine("---------Алгоритм 1.1------------");
            foreach (int M in trcount)
            {   
            Console.WriteLine("--- M="+ M+"------------");
         
            lAllTime = 0;
            dt1 = DateTime.Now;
            for (int i = 0; i < tCount; i++)
            {
                Alg11(M, paths);

                dt2 = DateTime.Now;
                lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
            }

            Console.WriteLine("Время: " + lAllTime / tCount);

            }
            Console.WriteLine("---------Алгоритм 1.2---------------");
            foreach (int M in trcount)
            {
                Console.WriteLine("--- M=" + M + "------------");
                lAllTime = 0;
                dt1 = DateTime.Now;
                for (int i = 0; i < tCount; i++)
                {

                    Alg12(M, paths);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }

                Console.WriteLine("Время: " + lAllTime / tCount);
            }
            Console.WriteLine("---------Алгоритм 2.1---------------");
            foreach (int M in trcount)
            {
                Console.WriteLine("--- M=" + M + "------------");
                lAllTime = 0;
                dt1 = DateTime.Now;
                for (int i = 0; i < tCount; i++)
                {
                    Alg21(M / 2, M / 2, paths);

                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }

                Console.WriteLine("Время: " + lAllTime / tCount);
            }
            Console.WriteLine("---------Алгоритм 1.1 конкуррентный---------------");
            foreach (int M in trcount)
            {
                Console.WriteLine("--- M=" + M + "------------");
                lAllTime = 0;
                dt1 = DateTime.Now;
                for (int i = 0; i < tCount; i++)
                {
                    Alg11Conc(M, paths);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / tCount);
            }
            Console.WriteLine("---------Алгоритм 1.2 конкуррентный---------------");
            foreach (int M in trcount)
            {
                Console.WriteLine("--- M=" + M + "------------");
                lAllTime = 0;
                dt1 = DateTime.Now;
                for (int i = 0; i < tCount; i++)
                {
                    Alg12Conc(M, paths);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                }
                Console.WriteLine("Время: " + lAllTime / tCount);
            }
            */
            Console.WriteLine("---------Алгоритм 2.1 конкуррентный---------------");
            foreach (int M in trcount)
            {
                Console.WriteLine("--- M=" + M + "------------");
                lAllTime = 0;
                dt1 = DateTime.Now;
                for (int i = 0; i < tCount; i++)
                {
                    Alg21Conc(M / 2, M / 2, paths);
                    dt2 = DateTime.Now;
                    lAllTime = lAllTime + (dt2 - dt1).TotalMilliseconds;
                
                }
                Console.WriteLine("Время: " + lAllTime / tCount);


            }

       
            Console.ReadLine();

        }
    }
}
