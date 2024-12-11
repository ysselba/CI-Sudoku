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
            List<int> gem = new List<int>();
            List<TimeSpan> gemTS = new List<TimeSpan>();
            string[] input = Console.ReadLine().Split(' ');
            // Sudoku s = new Sudoku(input);
            //s.Print();
            //Console.WriteLine("");
            // SudokuSolver ss = new SudokuSolver(s);
            // int colSum = ss.Columns.Sum();
            // int rowSum = ss.Rows.Sum();
            //Console.WriteLine($"{colSum} {rowSum}");

            for (int i = 0; i < 2; i++)
            {
                mechanisme(input, gem, gemTS);
                Console.WriteLine();
            }
            Console.WriteLine(gem.Average());
            Console.WriteLine(new TimeSpan(Convert.ToInt64(gemTS.Average(t => t.Ticks))));
        }

        public static void mechanisme(string[] input, List<int> gem, List<TimeSpan> gemTS)
        {
            DateTime datetimebegin = DateTime.Now;
            Sudoku s = new Sudoku(input);
            SudokuSolver ss = new SudokuSolver(s);
            int colSum = ss.Columns.Sum();
            int rowSum = ss.Rows.Sum();

            int S = 1;
            int plateau = 10;
            int count = 0;
            int plateauCount = 0;

            while (colSum + rowSum != 0)// && count < 400000)
            {
                ss.RandomBlockSwap();

                //gaat nog iets mis omdat het alleen maar groter wordt
                int newColSum = ss.Columns.Sum();
                int newRowSum = ss.Rows.Sum();

                //Console.WriteLine($"{newColSum} {newRowSum}");
                if (newColSum == colSum && newRowSum == rowSum) plateauCount++;
                else count++;

                colSum = newColSum;
                rowSum = newRowSum;

                if (plateauCount >= plateau)
                {
                    ss.randomWalk(S);
                    plateauCount = 0;
                }
            }
            TimeSpan nieuwDT = DateTime.Now.Subtract(datetimebegin);
            Console.WriteLine($"Het duurde ongeveer {DateTime.Now.Subtract(datetimebegin)} om een oplossing te vinden.");
            s.Print();
            Console.WriteLine(
                $"Het duurde ongeveer {DateTime.Now.Subtract(datetimebegin)} om een oplossing te vinden en om die te printen.");
            Console.WriteLine($"{colSum} {rowSum}");
            Console.WriteLine($"\nCount: {count}");
            gem.Add(count);
            gemTS.Add(nieuwDT);
        }
    }
}