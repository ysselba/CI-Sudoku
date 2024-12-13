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
        public static void Main(string[] args)
        {
            //test vars
            int sMin = 1;
            int sMax = 2;
            int plateauMin = 9;
            int plateauMax = 10;
            int testFreq = 10;
            
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

        public static void testSudoku(string[] input, int S, int plateau, int testFreq)
        {
            List<int> gem = new List<int>();
            List<TimeSpan> gemTS = new List<TimeSpan>();
            List<int> gemPlateau = new List<int>();
            
            for (int i = 0; i < testFreq; i++)
            {
                mechanisme(input, gem, gemTS, gemPlateau, S, plateau);
                //Console.WriteLine($"Sudoku iteration: {i + 1}");
            }

            TimeSpan gemTime = new TimeSpan(0);
            foreach (TimeSpan t in gemTS)
            {
                gemTime += t;
            }
            gemTime = gemTime / gemTS.Count;
            
            Console.WriteLine($"Average amount of iterations: {gem.Average()}");
            Console.WriteLine($"Average Time spent per sudoku (HH:MM:SS): {gemTime}");
            Console.WriteLine($"Average amount of random walks: {gemPlateau.Average()}.");
        }

        public static void mechanisme(string[] input, List<int> gem, List<TimeSpan> gemTS, List<int> gemPlateau, int S, int plateau)
        {
            DateTime datetimebegin = DateTime.Now;
            Sudoku s = new Sudoku(input);
            SudokuSolver ss = new SudokuSolver(s);
            int colSum = ss.Columns.Sum();
            int rowSum = ss.Rows.Sum();
            
            int count = 0;
            int plateauCount = 0;
            int aantalkeerPlateau = 0;
            
            while (colSum + rowSum != 0)
            {
                ss.RandomBlockSwap();
                
                //gaat nog iets mis omdat het alleen maar groter wordt
                int newColSum = ss.Columns.Sum();
                int newRowSum = ss.Rows.Sum();
                
                if(newColSum + newRowSum == colSum + rowSum) plateauCount++;
                else plateauCount = 0;
                
                count++;
                
                colSum = newColSum;
                rowSum = newRowSum;
                
                if (plateauCount >= plateau)
                {
                    ss.randomWalk(S);
                    aantalkeerPlateau++;
                    plateauCount = 0;
                }
            }
            gem.Add(count);
            gemTS.Add(DateTime.Now - datetimebegin);
            gemPlateau.Add(aantalkeerPlateau);
        }
    }
}
