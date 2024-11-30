using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

public class Program
{
    public static void Main(string[] args)
    {
        string[] tempInput = Console.ReadLine().Split(' ');
        List<SudokuCel> input = new List<SudokuCel>();
        for (int n = 0; n < tempInput.Length; n++)
        {
            bool tempboolwaarde = false;
            int grootblokx = GrootBlokCoord(n%9);
            int grootbloky = GrootBlokCoord((int)Math.Floor(Convert.ToDecimal(n / 9)));
            SudokuCel nieuwecel = new SudokuCel(int.Parse(tempInput[n]), n % 9, (int)Math.Floor(Convert.ToDecimal(n / 9)), grootblokx, grootbloky, tempboolwaarde);
            if (int.Parse(tempInput[n]) != 0)
            {
                nieuwecel.Gefixeerd = true;
            }
            input.Add(nieuwecel);
        }

        static int GrootBlokCoord(int n)
        {   return (int)Math.Floor(Convert.ToDecimal(n)) % 3;  }
        
        Hashtable ht = new Hashtable();
        for (int i = 0; i < 9; i++)
        {
            List<SudokuCel> blok = new List<SudokuCel>();
            // Telkens langs een blok gaan, 2D 3x3
            for (int p = ((i / 3) * 3); p < (((i / 3) * 3) + 3); p++)
            {
                for (int q = ((i / 3) * 3); q < (((i / 3) * 3) + 3); q++)
                {
                    blok.Add(input[p * 3 + q]);
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
                if (blok[p] == 0)
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

    public class SudokuCel
    {
        public int Waarde { get; set; }
        public int Xcoord { get; set; }
        public int Ycoord { get; set; }
        public int GrootblokX { get; set; }
        public int GrootblokY { get; set; }
        public bool Gefixeerd { get; set; }
        public SudokuCel(int waarde, int xcoord, int ycoord, int grootblokx, int grootbloky, bool gefixeerd)
        { Waarde = waarde; Xcoord = xcoord; Ycoord = ycoord; GrootblokX = grootblokx; GrootblokY = grootbloky; Gefixeerd = gefixeerd; } 
    }
}
