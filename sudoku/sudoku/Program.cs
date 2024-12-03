using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

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
     */
    public static void Main(string[] args)
    {
        List<SudokuCel> sudoku = init();
        printSudoku(sudoku);
    }

    //creates the initial sudoku list based on the given input
    public static List<SudokuCel> init()
    {
        string[] stringInput = Console.ReadLine().Split(' ');
        List<SudokuCel> Sudoku = new List<SudokuCel>();
        bool[,,] used = new bool[3,3,9];

        for (int i = 0; i < stringInput.Length; i++)
        {
            //x en y coord in 9x9
            int x9x9 = i % 9;
            int y9x9 = i / 9;
            //x en y coord in 3x3
            int x3x3 = x9x9 / 3;
            int y3x3 = y9x9 / 3;
            //waarde cel en bool die kijkt of er een nieuwe waarde moet komen
            int n = int.Parse(stringInput[i]);
            bool gefixeerd = n != 0;
            //wanneer het gefixeerd is pas dit aan in used
            if(gefixeerd) used[x3x3, y3x3, n - 1] = true;

            //voeg toe
            Sudoku.Add(new SudokuCel(n, x9x9, y9x9, x3x3, y3x3, gefixeerd));
        }
        
        //voor niet gefixeerde items pas waarde aan die niet in used staat
        foreach (SudokuCel s in Sudoku)
        {
            if (!s.Gefixeerd)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (!used[s.GrootblokX, s.GrootblokY, i])
                    {
                        used[s.GrootblokX, s.GrootblokY, i] = true;
                        s.Waarde = i + 1;
                        break;
                    }
                }
            }
        }
        
        return Sudoku;
    }

    //print de sudoku in de console
    public static void printSudoku(List<SudokuCel> Sudoku)
    {
        string[,] numbers = new string[9,9];
        foreach (SudokuCel s in Sudoku)
        {
            int x = s.Xcoord;
            int y = s.Ycoord;
            int n = s.Waarde;

            numbers[x, y] = $"{n}";
        }

        for (int y = 0; y < 9; y++)
        {
            if(y % 3 == 0 && y != 0) Console.WriteLine("-----------");
            string s = "";
            for (int x = 0; x < 9; x++)
            {
                if (x % 3 == 0 && x != 0)
                {
                    s += "|";
                }
                s += numbers[x, y];
            }
            Console.WriteLine(s);
        }
    }
}

public class SudokuCel // Dit is het object SudokuCel dat ik gemaakt heb
{
    public int Waarde { get; set; } // De waarde van de cel (0, 1, 2, 3, 4, 5, 6, 7, 8 of 9)
    public int Xcoord { get; set; } // De x-coordinaat van de locatie van de cel (0, 1, 2, 3, 4, 5, 6, 7 of 8)
    public int Ycoord { get; set; } // De y-coordinaat van de locatie van de cel (0, 1, 2, 3, 4, 5, 6, 7 of 8)
    public int GrootblokX { get; set; } // De x-coordinaat van het grote 3x3 blok waar de cel in zit (0, 1 of 2)
    public int GrootblokY { get; set; } // De y-coordinaat van het grote 3x3 blok waar de cel in zit (0, 1 of 2)
    public bool Gefixeerd { get; set; } // Boolwaarde die beschrijft of de waarde vast staat (waarde is geen 0) of niet (waarde is 0)
    public SudokuCel(int waarde, int xcoord, int ycoord, int grootblokx, int grootbloky, bool gefixeerd)
    { Waarde = waarde; Xcoord = xcoord; Ycoord = ycoord; GrootblokX = grootblokx; GrootblokY = grootbloky; Gefixeerd = gefixeerd; } 
}
