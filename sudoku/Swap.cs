using System.Security.Cryptography;

namespace sudoku
{
    public class Swap
    {
        //coord points
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        //total score
        public int score;
        //score per coord and row col combination
        public int x1s;
        public int y1s;
        public int x2s;
        public int y2s;
        //the value it should be updated to
        public int v1;
        public int v2;
        //the solver
        private SudokuSolver ss;

        //create a swap
        public Swap(int x1, int y1, int x2, int y2, SudokuSolver solver)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            ss = solver;
            //give the coord the new swaped value
            v1 = ss._sudoku.Board[this.x2, this.y2];
            v2 = ss._sudoku.Board[this.x1, this.y1];
            //calc the score for the rows, cols, total
            calcScore();
        }

        private void calcScore()
        {
            calcSwap(x1, y1, x2, y2);
            int totalNew;
            int totalOld;
            
            if (x1 == x2)
            {
                totalNew = x1s + x2s + y2s;
                totalOld = ss.Rows[y1] + ss.Rows[y2] + ss.Columns[x1];
                
            }
            else if (y1 == y2)
            {
                totalNew = x1s + y1s + y2s;
                totalOld = ss.Rows[y1] + ss.Columns[x1] + ss.Columns[x2];
            }
            else
            {
                totalNew = x1s + y1s + x2s + y2s;
                totalOld = ss.Rows[y1] + ss.Rows[y2] + ss.Columns[x1] + ss.Columns[x2];
            }
            score = totalNew - totalOld;
        }
        private void calcSwap(int x1, int y1, int x2, int y2)
        {
            int count = 0;
            //cols x1 (y1s)
            HashSet<int> set = new HashSet<int>();
            for (int i = 0; i < 9; i++)
            {
                int t = ss._sudoku.Board[x1, i];
                if (i == y1)
                {
                    if(set.Add(v1)) count++;
                }
                else if (i == y2 && x1 == x2)
                {
                    if (set.Add(v2)) count++;
                }
                else
                {
                    if (set.Add(t)) count++;
                }
            }
            y1s = 9 - count;

            //cols x2 (y2s)
            count = 0;
            set = new HashSet<int>();
            for (int i = 0; i < 9; i++)
            {
                int t = ss._sudoku.Board[x2, i];
                if (i == y2)
                {
                    if(set.Add(v2)) count++;
                }
                else if (i == y1 && x1 == x2)
                {
                    if (set.Add(v1)) count++;
                }
                else
                {
                    if (set.Add(t)) count++;
                }
            }
            y2s = 9 - count;
            
            //rows y1 (x1s)
            count = 0;
            set = new HashSet<int>();
            for (int i = 0; i < 9; i++)
            {
                int t = ss._sudoku.Board[i, y1];
                if (i == x1)
                {
                    if(set.Add(v1)) count++;
                }
                else if (i == x2 && y1 == y2)
                {
                    if (set.Add(v2)) count++;
                }
                else
                {
                    if (set.Add(t)) count++;
                }
            }
            x1s = 9 - count;

            //rows y2 (x2s)
            count = 0;
            set = new HashSet<int>();
            for (int i = 0; i < 9; i++)
            {
                int t = ss._sudoku.Board[i, y2];
                if (i == x2)
                {
                    if(set.Add(v2)) count++;
                }
                else if (i == x1 && y1 == y2)
                {
                    if (set.Add(v1)) count++;
                }
                else
                {
                    if (set.Add(t)) count++;
                }
            }
            x2s = 9 - count;
        }

        public void preformSwap()
        {
            //update values in board, rows, cols
            ss._sudoku.Board[x1, y1] = v1;
            ss._sudoku.Board[x2, y2] = v2;
            ss.Rows[y1] = x1s;
            ss.Columns[x1] = y1s;
            ss.Rows[y2] = x2s;
            ss.Columns[x2] = y2s;
        }
    }
}