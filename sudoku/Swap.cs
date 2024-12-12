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
            //calculate the new score for the row or col
            //for a row the x is static and for col the y is static
            y1s = calcSwap(y1, x1, v1, true);
            x1s = calcSwap(x1, y1, v1, false);
            y2s = calcSwap(y2, x2, v2, true);
            x2s = calcSwap(x2, y2, v2, false);

            int totalNew;
            int totalOld;
            totalNew = x1s + y1s + x2s + y2s;
            totalOld = ss.Rows[x1] + ss.Columns[y1] + ss.Rows[x2] + ss.Columns[y2];
            score = totalNew - totalOld;
        }
        
        private int calcSwap(int placeNew, int placeRowCol , int newValue, bool isColumn)
        {
            int count = 0;
            HashSet<int> seen = new HashSet<int>();
            if (isColumn)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (i != placeNew)
                    {
                        if (seen.Add(ss._sudoku.Board[placeRowCol,i]))
                        {
                            count++;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    if (i != placeNew)
                    {
                        if (seen.Add(ss._sudoku.Board[i,placeRowCol]))
                        {
                            count++;
                        }
                    }
                }
            }

            if (seen.Add(newValue))
            {
                count++;
            }

            return 9 - count;
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