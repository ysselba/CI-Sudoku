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
            string[] input = Console.ReadLine().Split(' ');
            List<int> gem = new List<int>();
            List<TimeSpan> gemTS = new List<TimeSpan>();
            List<int> gemPlateau = new List<int>();

            int testFreq = 2;
            for (int i = 0; i < 10; i++)
            {
                mechanisme(input, gem, gemTS, gemPlateau, 1, 10);
                Console.WriteLine($"Sudoku iteration: {i + 1}");
            }

            TimeSpan gemTime = new TimeSpan(0);
            foreach (TimeSpan t in gemTS)
            {
                gemTime += t;
            }
            gemTime = gemTime / gemTS.Count;
            
            Console.WriteLine($"Average amount of iterations: {gem.Average()}");
            Console.WriteLine($"Avrage Time spent per sudoku (HH:MM:SS): {gemTime}");
            Console.WriteLine($"The average amount of random walks: {gemPlateau.Average()}.");

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
            //s.Print();
            //if(gemTS.Count > 0) Console.WriteLine($"Time: {gemTS[gemTS.Count - 1]}");
            //Console.WriteLine($"Iterations: {count}");
            //Console.WriteLine($"Aantal keer dat een plateau is bereikt: {aantalkeerPlateau}");


            //Console.WriteLine($"Het duurde ongeveer {DateTime.Now.Subtract(datetimebegin)} om een oplossing te vinden.");
            //s.Print();
            //Console.WriteLine($"Het duurde ongeveer {DateTime.Now.Subtract(datetimebegin)} om een oplossing te vinden en om die te printen.");
            //Console.WriteLine($"{colSum} {rowSum}");
            //Console.WriteLine($"\nCount: {count}");
            /*
            //test for correctness
            for (int i = 0; i < 9; i++)
            {
                int[] r = new int[9];
                int[] c = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    r[j] = ss._sudoku.Board[j, i];
                    c[j] = ss._sudoku.Board[i, j];
                }
                //the amount of missing numbers for row or column i
                int rd = 9 - r.Distinct().Count();
                int cd = 9 - c.Distinct().Count();
                Console.WriteLine($"{i}: r:{rd} c:{cd}");
            }
            */

        }
    }
}
