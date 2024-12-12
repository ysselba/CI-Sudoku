using System.Runtime.Intrinsics;
using System.Xml;

namespace sudoku
{
    public class SudokuSolver
    {
        public Sudoku _sudoku; //sudoku that should be solved
        public int[] Rows; //list of missing numbers in each row
        public int[] Columns; //list of missing numbers in each Column
        public Random random; //random for random pick

        public SudokuSolver(Sudoku s)
        {
            //initialize vars
            _sudoku = s;
            Rows = new int[9];
            Columns = new int[9];
            InitRowCol();
            random = new Random();
        }
        
        //calculate the initial amount of missing number per row and column
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
                //the amount of missing numbers for row or column i
                Rows[i] = 9 - r.Distinct().Count();
                Columns[i] = 9 - c.Distinct().Count();
            }
        }

        public void RandomBlockSwap()
        {
            //get random block x and y
            int blockX = random.Next(0, 3) * 3;
            int blockY = random.Next(0, 3) * 3;
            
            //list for all possible swaps in block
            List<Swap> swaps = new List<Swap>();
            
            //calculate all possible x and y for the first number
            for (int x = blockX; x < blockX + 3; x++)
            {
                for (int y = blockY; y < blockY + 3; y++)
                {
                    //check if the number is not fixed
                    if (!_sudoku.Gefixeerd[x,y])
                    {
                        //calculate all possible x and y for the second number
                        for (int i = blockX; i < blockX + 3; i++)
                        {
                            for (int j = blockY; j < blockY + 3; j++)
                            {
                                //check if the second number is not fixed and if it is not the same as the first
                                if (!_sudoku.Gefixeerd[i,j] && !(i == x && j == y))
                                {
                                    //add to possible swap
                                    swaps.Add(new Swap(i, j, x, y, this));
                                }
                            }
                        }
                    }
                }
            }
            
            //check if there is a better score than the current one
            int swapIndex = -1;
            int swapScore = 0;
            for (int i = 0; i < swaps.Count; i++)
            {
                int score = swaps[i].score;
                //update swap if same or equal to the current one
                if (score <= swapScore)
                {
                    swapScore = score;
                    swapIndex = i;
                }
            }

            //if a swap has been found that is better or equal 
            if (swapIndex > -1)
            {
                Swap sw = swaps[swapIndex];
                //Console.WriteLine($"{sw.x1},{sw.y1} | {sw.x2},{sw.y2} | {sw.score}");
                sw.preformSwap();
            }
        }

        //preform a random swap for s times
        public void randomWalk(int s)
        {
            while (s > 0)
            {
                Swap swap = randomSwap();
                swap.preformSwap();
                s--;
            }
        }

        //prefor a valid random swap
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
                validSwap = !(rX == rX2 && rY == rY2) && !_sudoku.Gefixeerd[rX, rY] && !_sudoku.Gefixeerd[rX2, rY2];
                if (validSwap)
                {
                    //preform swap
                    s = new Swap(rX, rY, rX2, rY2, this);
                    
                }
                
            }
            
            return s;
        }

        //print function for the row and col
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