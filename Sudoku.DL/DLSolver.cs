using Sudoku.Core;

/*Le resultat du benchmark peut changer en fonction de chaque compilation*/


namespace Sudoku.DL
{
    public class DLSolver : ISudokuSolver
    {
       
        private static DlxSudokuSolver s = new DlxSudokuSolver(); // attribut de type DLXSudokuSolver

        public void Solve(GrilleSudoku grid)
        {
            s.Solve(grid);
        }

    }
}
