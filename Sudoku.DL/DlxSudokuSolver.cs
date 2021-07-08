using Sudoku.Core;
using System;
namespace Sudoku.DL

{
    class DlxSudokuSolver
    {

        public void Solve(GrilleSudoku trav)
        {
          Dlx.MatrixList s = new Dlx.MatrixList(ConvertToMatrix(trav));           // constructeur prend en param int [][]
            s.search();
            trav.setSudoku(s.convertMatrixSudoku());                              // on affecte les val trouvee avec fonction search a grillesudoku
        }

         public int[][] ConvertToMatrix(GrilleSudoku grille) // conversion de grillesudoku en int[][]
        {
            int[][] sud = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                sud[i] = new int[9];
                for (int j = 0; j < 9; j++)
                {
                    sud[i][j] = grille.GetCellule(i, j);
                }
            }
            return sud;
        }
    }
}
