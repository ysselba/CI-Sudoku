namespace sudoku
{
    public class SudokuCel
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