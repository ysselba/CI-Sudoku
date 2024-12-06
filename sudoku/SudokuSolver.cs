using System.Runtime.Intrinsics;
using System.Xml;

namespace sudoku
{
    public class SudokuSolver
    {
        public Sudoku _sudoku;
        //totaal aantal missende nummers per row of column
        public int[] Rows;
        public int[] Columns;

        public SudokuSolver(Sudoku s)
        {
            _sudoku = s;
            Rows = new int[9];
            Columns = new int[9];
            InitRowCol();
        }
        
        public void InitRowCol()
        {
            for (int i = 0; i < 9; i++)
            {
                int[] r = new int[9];
                int[] c = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    r[j] = _sudoku.Board[j, i];
                    c[j] = _sudoku.Board[i, j];
                }
                Rows[i] = 9 - r.Distinct().Count();
                Columns[i] = 9 - c.Distinct().Count();
            }
        }

        public void RandomBlockSwap()
        {
            Random random = new Random();
            int blockX = random.Next(0, 3) * 3;
            int blockY = random.Next(0, 3) * 3;

            List<Swap> swaps = new List<Swap>();
            
            //bereken alle mogelijke swaps
            for (int x = blockX; x < blockX + 3; x++)
            {
                for (int y = blockY; y < blockY + 3; y++)
                {
                    if (!_sudoku.Gefixeerd[x,y])
                    {
                        for (int i = blockX; i < blockX + 3; i++)
                        {
                            for (int j = blockY; j < blockY + 3; j++)
                            {
                                //niet zelfde of gefixeerd
                                if (!_sudoku.Gefixeerd[i,j] && !(i == x && j == y))
                                {
                                    swaps.Add(new Swap(i, j, x, y, this));
                                }
                            }
                        }
                    }
                }
            }
            
            //kijk welke swap het beste is
            int swapIndex = -1;
            int swapScore = 0;
            for (int i = 0; i < swaps.Count; i++)
            {
                int score = swaps[i].score;
                if (score < swapScore)
                {
                    swapScore = score;
                    swapIndex = i;
                }
            }

            if (swapIndex >= 0)
            {
                swaps[swapIndex].preformSwap();
            }
        }

        public void randomWalk(int s)
        {
            while (s > 0)
            {
                Swap swap = randomSwap();
                swap.preformSwap();
                s--;
            }
        }

        public Swap randomSwap()
        {
            bool validSwap = false;
            Random random = new Random();
            Swap s = null;
            while (!validSwap)
            {
                //random block
                int blockX = random.Next(0, 3) * 3;
                int blockY = random.Next(0, 3) * 3;
                
                //random cells
                int celX = random.Next(0, 3);
                int celY = random.Next(0, 3);
                int celX2 = random.Next(0, 3);
                int celY2 = random.Next(0, 3);
                
                //calc coords
                int rX = blockX + celX;
                int rY = blockY + celY;
                int rX2 = blockX + celX2;
                int rY2 = blockY + celY2;
                
                //swap if valid
                validSwap = rX != rX2 && rY != rY2 && !_sudoku.Gefixeerd[rX, rY] && !_sudoku.Gefixeerd[rX2, rY2];
                if (validSwap) s = new Swap(rX, rY, rX2, rY2, this);
                
            }
            
            return s;
        }

        public void PrintRowCol()
        {
            string rs = "";
            string cs = "";

            for (int i = 0; i < 9; i++)
            {
                rs += Rows[i] + ",";
                cs += Columns[i] + ",";
            }

            Console.WriteLine($"Rows: {rs}\nCols: {cs}");
        }
        
        
    }
}