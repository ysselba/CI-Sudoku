using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace sudoku
{
    public class Program
    {
        /* 0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0
         * 
         * 0 0 3 | 0 2 0 | 6 0 0
         * 9 0 0 | 3 0 5 | 0 0 1
         * 0 0 1 | 8 0 6 | 4 0 0
         * ---------------------
         * 0 0 8 | 1 0 2 | 9 0 0
         * 7 0 0 | 0 0 0 | 0 0 8
         * 0 0 6 | 7 0 8 | 2 0 0
         * ---------------------
         * 0 0 2 | 6 0 9 | 5 0 0
         * 8 0 0 | 2 0 3 | 0 0 9
         * 0 0 5 | 0 1 0 | 3 0 0
         *
         * 253|129|638
         * 968|345|251
         * 471|876|479
         * -----------
         * 138|142|936
         * 749|359|148
         * 256|768|257
         * -----------
         * 142|659|527
         * 869|273|149
         * 375|418|368
         *  
         */
        
        //pick test or bench mode and read input if necessary
        public static void Main(string[] args)
        {
            Console.WriteLine("Press t for test and b for benchmark");
            char c = Console.ReadKey().KeyChar;
            Console.WriteLine();
            switch (c)
            {
                case 't':
                    Console.WriteLine("What sudoku do you want to test?");
                    mechanisme(Console.ReadLine().Split(' '), new List<int>(), new List<TimeSpan>(), new List<int>(), 1, 10, true);
                    break;
                case 'b':
                    benchmark();
                    break;
                default:
                    Console.WriteLine("Invalid key");
                    break;
            }

        }
        
        //Preform a benchmark for multiple sudokus
        public static void benchmark() 
        {
            //TODO: output in csv betand
            //test vars
            int sMin = 1;
            int sMax = 1;
            int plateauMin = 10;
            int plateauMax = 20;
            int testFreq = 20; //TODO: kijk even hoeveel kan
            
            List<string> sudokus = new List<string>
            {
                "0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0",
                "2 0 0 0 8 0 3 0 0 0 6 0 0 7 0 0 8 4 0 3 0 5 0 0 2 0 9 0 0 0 1 0 5 4 0 8 0 0 0 0 0 0 0 0 0 4 0 2 7 0 6 0 0 0 3 0 1 0 0 7 0 4 0 7 2 0 0 4 0 0 6 0 0 0 4 0 1 0 0 0 3",
                "0 0 0 0 0 0 9 0 7 0 0 0 4 2 0 1 8 0 0 0 0 7 0 5 0 2 6 1 0 0 9 0 4 0 0 0 0 5 0 0 0 0 0 4 0 0 0 0 5 0 7 0 0 9 9 2 0 1 0 8 0 0 0 0 3 4 0 5 9 0 0 0 5 0 7 0 0 0 0 0 0",
                "0 3 0 0 5 0 0 4 0 0 0 8 0 1 0 5 0 0 4 6 0 0 0 0 0 1 2 0 7 0 5 0 2 0 8 0 0 0 0 6 0 3 0 0 0 0 4 0 1 0 9 0 3 0 2 5 0 0 0 0 0 9 8 0 0 1 0 2 0 6 0 0 0 8 0 0 6 0 0 2 0",
                "0 2 0 8 1 0 7 4 0 7 0 0 0 0 3 1 0 0 0 9 0 0 0 2 8 0 5 0 0 9 0 4 0 0 8 7 4 0 0 2 0 8 0 0 3 1 6 0 0 3 0 2 0 0 3 0 2 7 0 0 0 6 0 0 0 5 6 0 0 0 0 8 0 7 6 0 5 1 0 9 0"
            };
            foreach (string s in sudokus)
            {
                Console.WriteLine($"Sudoku: {s}");
                Console.WriteLine($"Test Frequency: {testFreq}");
                testSudokus(s, sMin, sMax, plateauMin, plateauMax, testFreq);
            }
        }
        
        //test the sudoku solver for each possible S and Plateau combination
        public static void testSudokus(string input, int SMin, int SMax, int PlateauMin, int PlateauMax, int testFreq)
        {
            string[] list = input.Split(' ');

            for (int  i = SMin; i <= SMax; i++)
            {
                for (int j = PlateauMin; j <= PlateauMax; j++)
                {
                    Console.WriteLine($"S: {i}, Plateau: {j}");
                    testSudoku(list, i, j, testFreq);
                    Console.WriteLine("");
                }
            }
        }
        
        //Preform a test for a S Plateau combination
        public static void testSudoku(string[] input, int S, int plateau, int testFreq)
        {
            //init avrage lists for iterations, time and plateau
            List<int> gem = new List<int>();
            List<TimeSpan> gemTS = new List<TimeSpan>();
            List<int> gemPlateau = new List<int>();
            
            //preform test testFreq times
            for (int i = 0; i < testFreq; i++)
            {
                mechanisme(input, gem, gemTS, gemPlateau, S, plateau, false);
            }

            //calculate avrage time
            TimeSpan gemTime = new TimeSpan(0);
            foreach (TimeSpan t in gemTS)
            {
                gemTime += t;
            }
            gemTime /= gemTS.Count;
            
            Console.WriteLine($"Average amount of iterations: {gem.Average()}");
            Console.WriteLine($"Average Time spent per sudoku (HH:MM:SS): {gemTime}");
            Console.WriteLine($"Average amount of random walks: {gemPlateau.Average()}.");
        }

        public static void mechanisme(string[] input, List<int> gem, List<TimeSpan> gemTS, List<int> gemPlateau, int S, int plateau, bool testB)
        {
            DateTime datetimebegin = DateTime.Now;
            Sudoku s = new Sudoku(input);
            SudokuSolver ss = new SudokuSolver(s);
            int colSum = ss.Columns.Sum();
            int rowSum = ss.Rows.Sum();
            int score = colSum + rowSum;
            
            int count = 0;
            int plateauCount = 0;
            int aantalkeerPlateau = 0;
            
            while (count < 9999999 && score > 0)
            {
                int newScore = score + ss.RandomBlockSwap();
                if(newScore == score) plateauCount++;
                else
                {
                    plateauCount = 0;
                    score = newScore;
                }
                count++;
                if (plateauCount >= plateau)
                {
                    score += ss.randomWalk(S);
                    aantalkeerPlateau++;
                    plateauCount = 0;
                }
            }
            gem.Add(count);
            gemTS.Add(DateTime.Now - datetimebegin);
            gemPlateau.Add(aantalkeerPlateau);

            if (testB)
            {
                Console.WriteLine($"Iterations: {count}");
                Console.WriteLine($"Calculation time: {DateTime.Now - datetimebegin}");
                Console.WriteLine($"Plateau Count: {aantalkeerPlateau}");
                s.Print();
                for (int i = 0; i < 9; i++)
                {
                    int[] c = new int[9];
                    int[] r = new int[9];
                    for (int j = 0; j < 9; j++)
                    {
                        c[j] = s.Board[i, j];
                        r[j] = s.Board[j, i];
                    }
                    Console.WriteLine($"c: {9 - c.Distinct().Count()}, r: {9 - r.Distinct().Count()}");
                }
            }
        }
    }
}
