using System.Drawing;
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
        
        
        public int RandomBlockSwap()
        {
            //get random block x and y
            int blockX = random.Next(0, 3) * 3;
            int blockY = random.Next(0, 3) * 3;
            //init best swap
            Swap s = null;
            int blockSize = 3;
            //loop through all possible swaps
            for (int i = 0; i < blockSize * blockSize; i++)
            {
                for (int j = i + 1; j < blockSize * blockSize; j++)
                {
                    //1D to 2D
                    int x1 = i / blockSize + blockX;
                    int y1 = i % blockSize + blockY;
                    int x2 = j / blockSize + blockX;
                    int y2 = j % blockSize + blockY;
                    
                    //check if one of the numbers is fixed
                    if (!_sudoku.Gefixeerd[x1, y1] && !_sudoku.Gefixeerd[x2, y2])
                    { 
                        //swap if score is better
                        Swap temp = new Swap(x1, y1, x2, y2, this);
                        if (s == null)
                        {
                            s = temp;
                        }
                        else
                        {
                            if (temp.score < s.score)
                            {
                                s = temp;
                            }
                        }
                    }
                }
            }

            //if the score stayed the same or decreased preform the swap
            if (s.score <= 0)
            {
                s.preformSwap();
                return s.score;
            }

            return 0;
        }

        //preform a random swap for s times
        public int randomWalk(int s)
        {
            int diff = 0;
            while (s > 0)
            {
                Swap swap = randomSwap();
                swap.preformSwap();
                s--;
                diff += swap.score;
            }
            return diff;
        }

        //prefor a valid random swap
        public Swap randomSwap()
        {
            bool validSwap = false;
            Random random = new Random();
            Swap s = null;
            int swapCount = 0;
            while (!validSwap && swapCount < 1000)
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
                swapCount++;
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