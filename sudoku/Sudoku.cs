namespace sudoku
{
    public class Sudoku
    {
        public int[,] Board { get; set; }
        public bool[,] Gefixeerd { get; set; }

        public Sudoku(string[] stringInput)
        {
            Board = new int[9, 9];
            Gefixeerd = new bool[9, 9];
            Init(stringInput);
        }
        
        //creates the initial sudoku list based on the given input
        private void Init(string[] stringInput)
        {
            bool[,,] used2 = new bool[3,3,9];
            for (int i = 0; i < stringInput.Length; i++)
            {
                //x en y coord in 9x9
                int x = i % 9;
                int y = i / 9;
                //x en y coord in 3x3
                int x3x3 = x / 3;
                int y3x3 = y / 3;
                
                //waarde cel en bool die kijkt of er een nieuwe waarde moet komen
                int n = int.Parse(stringInput[i]);
                bool gefixeerd = n != 0;

                //voeg toe
                if (gefixeerd)
                {
                    used2[x3x3, y3x3, n - 1] = gefixeerd;
                    Gefixeerd[x,y] = gefixeerd;
                }
                Board[x, y] = n;
            }
            
            //voor niet gefixeerde items pas waarde aan die niet in used staat
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int n = Board[x, y];
                    if (n == 0)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if (!used2[x/3, y/3, i])
                            {
                                used2[x/3, y/3, i] = true;
                                Board[x, y] = i + 1;
                                break;
                            }
                        }
                    }
                }
            }
        }
        
        //print de sudoku in de console
        public void Print()
        {
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
                    s += $"{Board[x, y]}";
                }
                Console.WriteLine(s);
            }
        }
    }
}


