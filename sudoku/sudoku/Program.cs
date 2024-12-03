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
            bool tempboolwaarde = false; // De temporary bool waarde van 'Gefixeerd' is eerst gewoon false
            int grootblokx = GrootBlokCoord(n%9); // De x-coordinaat van het grote (3x3) blok waar de cel in is, deze x-coord is 0, 1 of 2
            int grootbloky = GrootBlokCoord((int)Math.Floor(Convert.ToDecimal(n / 9))); // De y-coordinaat van het grote (3x3) blok waar de cel in is, deze y-coord is 0, 1 of 2
            SudokuCel nieuwecel = new SudokuCel(int.Parse(tempInput[n]), n % 9, (int)Math.Floor(Convert.ToDecimal(n / 9)), grootblokx, grootbloky, tempboolwaarde);
            if (int.Parse(tempInput[n]) != 0)
            {
                nieuwecel.Gefixeerd = true; // Als de input op deze cel geen 0 is, is het gefixeerd
            }
            input.Add(nieuwecel);
        }

        static int GrootBlokCoord(int n)
        {   return (int)Math.Floor(Convert.ToDecimal(n)) % 3;  // Simpele methode om te gebruiken voor het berekenen v/d coordinaten van de grote 3x3 blokken
        
        Hashtable ht = new Hashtable(); // Deze hadden jullie (Yassin en Bram) erin gezet, moeten nog aangepast worden om te passen bij het nieuwe object
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
}
