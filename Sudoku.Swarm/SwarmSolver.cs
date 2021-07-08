using System;
using Sudoku.Core;


namespace Sudoku.Swarm
{
    public class SwarmSolver : ISudokuSolver
    {
        public  void Solve(GrilleSudoku s)
        {
            //conversion de formats en int[,]
            int[,] converted = new int[9,9];
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    converted[row,column] = s.Cellules[row * 9 + column];
                }
            }
            var sudoku = Sudoku.New(converted);
            
           var solver = new SudokuSolver();
           
            var solvedSudoku = solver.Solve(sudoku, 200, 5000, 40);
            // Possibilité d'incrémenter le nombre d'organismes jusqu'à obtenir une solution?
            // Mais les meilleures optimisations à faire sont probablement dans le code du solver lui-même, auquel cas, il faudrait copier le code dans le projet plutôt que le référencer

            var numOrganisms = 200;
     
            do
            {
                solvedSudoku = solver.Solve(sudoku, numOrganisms, 5000, 1);
                numOrganisms *= 2;
            } while (solvedSudoku.Error>0);

            //affichage du sudoku
            Console.WriteLine(solvedSudoku.ToString());
            //Reconversion depuis int[,]
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    s.Cellules[row * 9 + column] = solvedSudoku.CellValues[row, column];
                }
            }


        }
    }
}