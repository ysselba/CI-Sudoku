namespace sudoku
{
    public class Swap
    {
        //coord points
        public int x1;
        public int y1;
        public int x2;
        public int y2;
        public int score;
        public int x1s;
        public int y1s;
        public int x2s;
        public int y2s;
        public int v1;
        public int v2;
        private SudokuSolver ss;

        public Swap(int x1, int y1, int x2, int y2, SudokuSolver solver)
        {
            x1 = x1;
            x2 = x2;
            y1 = y1;
            y2 = y2;
            ss = solver;
            v1 = ss._sudoku.Board[x2, y2];
            v2 = ss._sudoku.Board[x1, y1];
            calcScore();
        }

        private void calcScore()
        {
            x1s = calcSwap(x1, v1, false);//changed value so that they actually swap
            y1s = calcSwap(y1,v1, true);
            x2s = calcSwap(x2,v2, false);
            y2s = calcSwap(y2,v2, true);
            int totalNew = x1s + y1s + x2s + y2s;
            int totalOld = ss.Rows[y1] + ss.Columns[x1] + ss.Rows[y2] + ss.Columns[x2];
            score = totalNew - totalOld;
        }
        
        private int calcSwap(int j, int v, bool isColumn)
        {
            int newTotal = 0;
            
            int[] newL = new int[9];
            for (int i = 0; i < 9; i++)
            {
                if (isColumn)
                {
                    newL[i] = ss._sudoku.Board[i,j];
                }
                else
                {
                    newL[i] = ss._sudoku.Board[j,i];
                }
                
            }
            newL[j] = v;
            newTotal += 9 - newL.Distinct().Count();

            return newTotal;
        }

        public void preformSwap()
        {
            //update values in board, rows, cols
            ss._sudoku.Board[x1, y1] = v1;
            ss._sudoku.Board[x2, y2] = v2;
            ss.Rows[x1] += x1s;
            ss.Columns[y1] += y1s;
            ss.Rows[x2] += x2s;
            ss.Columns[y2] += y2s;
        }
    }
}