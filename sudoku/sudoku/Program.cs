using System;
using System.Collections;
using System.Linq.Expressions;

public class Program
{
    public static void Main(string[] args)
    {
        string[] tempInput = Console.ReadLine().Split(' ');
        List<int> input = new List<int>();
        foreach (string line in tempInput)
        {
            // Console.Write(line, int.Parse(line));
            input.Add(int.Parse(line));
        }
        Hashtable ht = new Hashtable();
        for (int i = 0; i < 9; i++)
        {
            List<int> blok = new List<int>();

            // Telkens langs een blok gaan, 2D 3x3
            for (int p = ((i/3)*3); p < (((i / 3) * 3) + 3); p++)
            {
                for (int q = ((i / 3) * 3); q < (((i / 3) * 3) + 3); q++)
                {
                    blok.Add(input[p*3 + q]);
                }
            }
            for (int n = 0; n < 9; n++)
            {
                // Coordinaten
                // blok.Add(input[n + (i*9)]);
                // Console.WriteLine($"{n}, {i}: {input[n + (i * 9)]}");
            }

            ht.Add(i, blok);
        }
        for (int i = 0; i < 9; i++)
        {
            List<int> blok = (List<int>)ht[i];
            for (int p = 0; p < blok.Count; p++)
            {
                // Iteratie van 1 t/m 9, als die niet zit in het blok dan add in lege veld
                if (blok[p]==0)
                {
                    for (int q = 1; q <= 9; q++)
                    {
                        if (!blok.Contains(q))
                        {
                            blok[p] = q;
                            break;
                        }
                    }
                }
            }
            ht[i] = blok; // Naar de volgende blok
            //foreach (int cijfer in blok)
            //{
            //    for (int p = 1; p <= 9; p++)
            //    {
            //        if (cijfer == 0 && !blok.Contains(p))
            //        {
            //            blok[cijfer] = p;
            //        }
            //    }
            //}
        }
    }

    public class Sudoku
    {
        // Klasse body
    }
}
