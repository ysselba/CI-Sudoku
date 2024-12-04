namespace sudoku
{
    public class Sudoku
    {
        public List<SudokuCel> SudokuList { get; set; }

        public Sudoku(string[] stringInput)
        {
            SudokuList = new List<SudokuCel>();
            Init(stringInput);
        }
        
        //creates the initial sudoku list based on the given input
        private void Init(string[] stringInput)
        {
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
                SudokuList.Add(new SudokuCel(n, x9x9, y9x9, x3x3, y3x3, gefixeerd));
            }
            
            //voor niet gefixeerde items pas waarde aan die niet in used staat
            foreach (SudokuCel s in SudokuList)
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
        }
        
        //print de sudoku in de console
        public void Print()
        {
            string[,] numbers = new string[9,9];
            foreach (SudokuCel s in SudokuList)
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
}


