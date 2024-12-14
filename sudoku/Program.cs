using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace sudoku
{
    public class Program
    {
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
                    SolveSudoku(Console.ReadLine().Split(' '), new List<int>(), new List<TimeSpan>(), new List<int>(), 1, 10, true);
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
            //test vars
            int sMin = 1;
            int sMax = 4;
            int plateauMin = 10;
            int plateauMax = 20;
            int testFreq = 100;
            WriterCSV writer = new WriterCSV();

            //benchmark sudokus
            List<string> sudokus = new List<string>
            {
                "0 0 3 0 2 0 6 0 0 9 0 0 3 0 5 0 0 1 0 0 1 8 0 6 4 0 0 0 0 8 1 0 2 9 0 0 7 0 0 0 0 0 0 0 8 0 0 6 7 0 8 2 0 0 0 0 2 6 0 9 5 0 0 8 0 0 2 0 3 0 0 9 0 0 5 0 1 0 3 0 0",
                "2 0 0 0 8 0 3 0 0 0 6 0 0 7 0 0 8 4 0 3 0 5 0 0 2 0 9 0 0 0 1 0 5 4 0 8 0 0 0 0 0 0 0 0 0 4 0 2 7 0 6 0 0 0 3 0 1 0 0 7 0 4 0 7 2 0 0 4 0 0 6 0 0 0 4 0 1 0 0 0 3",
                "0 0 0 0 0 0 9 0 7 0 0 0 4 2 0 1 8 0 0 0 0 7 0 5 0 2 6 1 0 0 9 0 4 0 0 0 0 5 0 0 0 0 0 4 0 0 0 0 5 0 7 0 0 9 9 2 0 1 0 8 0 0 0 0 3 4 0 5 9 0 0 0 5 0 7 0 0 0 0 0 0",
                "0 3 0 0 5 0 0 4 0 0 0 8 0 1 0 5 0 0 4 6 0 0 0 0 0 1 2 0 7 0 5 0 2 0 8 0 0 0 0 6 0 3 0 0 0 0 4 0 1 0 9 0 3 0 2 5 0 0 0 0 0 9 8 0 0 1 0 2 0 6 0 0 0 8 0 0 6 0 0 2 0",
                "0 2 0 8 1 0 7 4 0 7 0 0 0 0 3 1 0 0 0 9 0 0 0 2 8 0 5 0 0 9 0 4 0 0 8 7 4 0 0 2 0 8 0 0 3 1 6 0 0 3 0 2 0 0 3 0 2 7 0 0 0 6 0 0 0 5 6 0 0 0 0 8 0 7 6 0 5 1 0 9 0"
            };
            
            //solve the given sudokus testFreq times
            foreach (string s in sudokus)
            {
                Console.WriteLine($"Sudoku: {s}");
                Console.WriteLine($"Test Frequency: {testFreq}");
                testSudokus(s, sMin, sMax, plateauMin, plateauMax, testFreq, writer);
            }
            writer.UpdateCSV();
        }
        
        //test the sudoku solver for each possible S and Plateau combination
        public static void testSudokus(string input, int SMin, int SMax, int PlateauMin, int PlateauMax, int testFreq, WriterCSV writer)
        {
            string[] list = input.Split(' ');

            for (int  i = SMin; i <= SMax; i++)
            {
                for (int j = PlateauMin; j <= PlateauMax; j++)
                {
                    Console.WriteLine($"S: {i}, Plateau: {j}");
                    testSudoku(list, input, i, j, testFreq, writer);
                    Console.WriteLine("");
                }
            }
        }
        
        //Preform a test for a S Plateau combination
        public static void testSudoku(string[] input, string inputString, int S, int plateau, int testFreq, WriterCSV writer)
        {
            //init avrage lists for iterations, time and plateau
            List<int> gem = new List<int>();
            List<TimeSpan> gemTS = new List<TimeSpan>();
            List<int> gemPlateau = new List<int>();
            
            //preform test testFreq times
            for (int i = 0; i < testFreq; i++)
            {
                SolveSudoku(input, gem, gemTS, gemPlateau, S, plateau, false);
            }

            //calculate avrage time
            TimeSpan gemTime = new TimeSpan(0);
            foreach (TimeSpan t in gemTS)
            {
                gemTime += t;
            }
            gemTime /= gemTS.Count;
            double gemCount = gem.Average();
            double gemPlat = gemPlateau.Average();
            
            Console.WriteLine($"Average amount of iterations: {gemCount}");
            Console.WriteLine($"Average Time spent per sudoku (HH:MM:SS): {gemTime}");
            Console.WriteLine($"Average amount of random walks: {gemPlat}.");
            //string colNames = "S;Plateau;TestFrequency;AverageTime;AverageCount;AveragePlateauCount;Sudoku";
            writer.addLine(S,plateau,testFreq,gemTime,gemCount,gemPlat,inputString);
        }

        public static void SolveSudoku(string[] input, List<int> gem, List<TimeSpan> gemTS, List<int> gemPlateau, int S, int plateau, bool testB)
        {
            //init the sudoku
            DateTime datetimebegin = DateTime.Now;
            Sudoku s = new Sudoku(input);
            SudokuSolver ss = new SudokuSolver(s);
            //start score sudoku
            int colSum = ss.Columns.Sum();
            int rowSum = ss.Rows.Sum();
            int score = colSum + rowSum;
            //init bench variables
            int count = 0;
            int plateauCount = 0;
            int aantalkeerPlateau = 0;
            //stop if count gets to high or sudoku is solved
            while (count < 9999999 && score > 0)
            {
                //score after best random swap
                int newScore = score + ss.RandomBlockSwap();
                //when the new score is the same count plateau
                //otherwise reset the plateau and update the score
                if(newScore == score) plateauCount++;
                else
                {
                    plateauCount = 0;
                    score = newScore;
                }
                count++;
                //when plateau to high preform s long random walk
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
            //if testmode is active write the result
            if (testB)
            {
                Console.WriteLine($"Iterations: {count}");
                Console.WriteLine($"Calculation time: {DateTime.Now - datetimebegin}");
                Console.WriteLine($"Plateau Count: {aantalkeerPlateau}");
                s.Print();
                //check all rows and cols for duplicates
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
