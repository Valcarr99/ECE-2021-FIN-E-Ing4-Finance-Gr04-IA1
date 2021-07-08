using Sudoku.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.ConstraintSolver;

namespace Sudoku.DL.OR
{
    public class Sudoku_OR : ISudokuSolver
    {
        //2 Lignes de code supplémentaire
        int cell_size=3;
        
        IEnumerable<int> CELL = Enumerable.Range(0, 3);


        IEnumerable<int> cellIndices = Enumerable.Range(0, 9);

        public void Solve(GrilleSudoku s)
        {

            //Création d'un solver Or-tools
            Solver solver = new Solver("Sudoku");
            
            //Création de la grille de variables
            //Variables de décision

            IntVar[,] grid = solver.MakeIntVarMatrix(9, 9, 1, 9, "grid");//VERIFIER
            IntVar[] grid_flat = grid.Flatten();                         //VERIFIER 
            
            
            //Masque de résolution
            for (int i=0; i < cellIndices.Count(); i++)
            {
                for (int j=0; j < cellIndices.Count(); j++)
                {
                    if (s.GetCellule(i, j) > 0)
                    {
                        solver.Add(grid[i, j] == s.GetCellule(i, j));
                    }
                }
            }

            //Un chiffre ne figure qu'une seule fois par ligne/colonne/cellule
            for (int i=0; i < cellIndices.Count(); i++)
            {
                // Lignes
                solver.Add((from j in cellIndices
                    select grid[i, j]).ToArray().AllDifferent());

                // Colonnes
                solver.Add((from j in cellIndices
                    select grid[j, i]).ToArray().AllDifferent());

            }

            //Cellules
            for (int i=0; i < CELL.Count(); i++)
            {
                for (int j = 0; j < CELL.Count(); j++)
                {
                    solver.Add((from di in CELL
                                from dj in CELL
                                select grid[i * cell_size + di, j * cell_size + dj] 
                                 ).ToArray().AllDifferent());
                }
            }

            //Début de la résolution
            DecisionBuilder db = solver.MakePhase(grid_flat,
                Solver.INT_VAR_SIMPLE,
                Solver.INT_VALUE_SIMPLE);
            solver.NewSearch(db);

            //Mise à jour du sudoku
            //int n = cell_size * cell_size;
            //Or on sait que cell_size = 3 -> voir ligne 13
            //Inspiré de l'exemple : taille des cellules identique
            while (solver.NextSolution())
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        s.SetCell(i,j, (int) grid[i,j].Value());
                    }
                }

            }

            //Si 4 lignes dessus optionnelles, EndSearch est obligatoire
            solver.EndSearch();
             
           

        }
    }
}
