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
         * 
         */
        public static void Main(string[] args)
        {
            string[] input = Console.ReadLine().Split(' ');
            Sudoku s = new Sudoku(input);
            s.Print();
            Console.WriteLine("");
            SudokuSolver ss = new SudokuSolver(s);
            int count = 0;
            int colSum = ss.Columns.Sum();
            int rowSum = ss.Rows.Sum();
            
            while (colSum + rowSum != 0 && count < 1000)
            {
                ss.RandomBlockSwap();
                count++;
                //gaat nog iets mis omdat het alleen maar groter wordt
                colSum = ss.Columns.Sum();
                rowSum = ss.Rows.Sum();
                //Console.WriteLine($"{colSum} {rowSum}");
            }
            s.Print();
            Console.WriteLine($"\nCount: {count}");
        }
    }
}