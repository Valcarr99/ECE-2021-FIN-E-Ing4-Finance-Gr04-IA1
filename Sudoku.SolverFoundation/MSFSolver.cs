using System;
using Microsoft.SolverFoundation.Services;
using Sudoku.Core;

namespace Sudoku.SolverFoundation
{
    public class MSFSolver:ISudokuSolver
    {

        public void Solve(GrilleSudoku s)
        {
            SolverContext problem = SolverContext.GetContext();
            Model model = problem.CreateModel();

            Decision[][] grid = new Decision[9][]; // 0-based indices, 1-based values
            for (int r = 0; r < 9; ++r)
                grid[r] = new Decision[9];

            for (int r = 0; r < 9; ++r)
                for (int c = 0; c < 9; ++c)
                    grid[r][c] = new Decision(Domain.IntegerRange(1, 9), "grid" + r + c);

            for (int r = 0; r < 9; ++r)
                model.AddDecisions(grid[r]);

            //for (int r = 0; r < 9; ++r) // alternative to above
            //  for (int c = 0; c < 9; ++c)
            //    model.AddDecisions(grid[r][c]); 

            Console.WriteLine("\nCreating generic row constraints");
            for (int r = 0; r < 9; ++r)
                model.AddConstraint("rowConstraint" + r, Model.AllDifferent(grid[r]));

            Console.WriteLine("Creating generic column constraints");
            for (int c = 0; c < 9; ++c)
            {
                for (int first = 0; first < 8; ++first)
                    for (int second = first + 1; second < 9; ++second)
                        model.AddConstraint("colConstraint" + c + first + second, grid[first][c] != grid[second][c]);
            }

            Console.WriteLine("Creating generic sub-cube constraints");
            // cube constraints for grid[a][b] and grid[x][y]
            for (int r = 0; r < 9; r += 3)
            {
                for (int c = 0; c < 9; c += 3)
                {
                    for (int a = r; a < r + 3; ++a)
                    {
                        for (int b = c; b < c + 3; ++b)
                        {
                            for (int x = r; x < r + 3; ++x)
                            {
                                for (int y = c; y < c + 3; ++y)
                                {
                                    if ((x == a && y > b) || (x > a))
                                    { // xy > ab 
                                        model.AddConstraint("cubeConstraint" + a + b + x + y, grid[a][b] != grid[x][y]);
                                    }
                                } // y
                            } // x
                        } // b
                    } // a
                } // c
            } // r

            Console.WriteLine("Creating problem specific data constraints");

            // brute force approach:
            //model.AddConstraint("v02", grid[0][2] == 6);
            //model.AddConstraint("v03", grid[0][3] == 2);
            //model.AddConstraint("v07", grid[0][7] == 8);

            //model.AddConstraint("v12", grid[1][2] == 8);
            //model.AddConstraint("v13", grid[1][3] == 9);
            //model.AddConstraint("v14", grid[1][4] == 7);

            //model.AddConstraint("v22", grid[2][2] == 4);
            //model.AddConstraint("v23", grid[2][3] == 8);
            //model.AddConstraint("v24", grid[2][4] == 1);
            //model.AddConstraint("v26", grid[2][6] == 5);

            //model.AddConstraint("v34", grid[3][4] == 6);
            //model.AddConstraint("v38", grid[3][8] == 2);

            //model.AddConstraint("v41", grid[4][1] == 7);
            //model.AddConstraint("v47", grid[4][7] == 3);

            //model.AddConstraint("v50", grid[5][0] == 6);
            //model.AddConstraint("v54", grid[5][4] == 5);

            //model.AddConstraint("v62", grid[6][2] == 2);
            //model.AddConstraint("v64", grid[6][4] == 4);
            //model.AddConstraint("v65", grid[6][5] == 7);
            //model.AddConstraint("v66", grid[6][6] == 1);

            //model.AddConstraint("v72", grid[7][2] == 3);
            //model.AddConstraint("v74", grid[7][4] == 2);
            //model.AddConstraint("v75", grid[7][5] == 8);
            //model.AddConstraint("v76", grid[7][6] == 4);

            //model.AddConstraint("v81", grid[8][1] == 5);
            //model.AddConstraint("v85", grid[8][5] == 1);
            //model.AddConstraint("v86", grid[8][6] == 2);
            ////model.AddConstraint("v86", grid[8][6] == 4); // creates unsolvable problem

            AddDataConstraints(s, model, grid); // more elegant approach

            Console.WriteLine("\nSolving. . . ");

            int numSolutions = NumberSolutions(problem);
            Console.WriteLine("\nThere is/are " + numSolutions + " Solution(s)\n");

            Solution solution = problem.Solve();
            //ShowAnswer(grid); // alternative to below

            for (int r = 0; r < 9; ++r)
            {
                for (int c = 0; c < 9; ++c)
                {
                    double v = grid[r][c].GetDouble();
                    //Console.Write(" " + v);
                    s.SetCell(r,c, (int) v);
                }
            }
        }

       

        static void AddDataConstraints(GrilleSudoku data, Model model, Decision[][] grid)
        {
            for (int r = 0; r < 9; ++r)
            {
                for (int c = 0; c < 9; ++c)
                {
                    if (data.GetCellule(r,c) != 0)
                    {
                        model.AddConstraint("v" + r + c, grid[r][c] == data.GetCellule(r, c));
                    }
                }
            }
        }

        static int NumberSolutions(SolverContext problem)
        {
            int ct = 0;
            Solution soln = problem.Solve();
            while (soln.Quality == SolverQuality.Feasible)
            {
                ++ct;
                soln.GetNext();
            }
            return ct;
        }

        //static void ShowAnswer(Decision[][] grid)
        //{
        //  for (int r = 0; r < 9; ++r) {
        //    for (int c = 0; c < 9; ++c) {
        //      double v = grid[r][c].GetDouble();
        //      Console.Write(v + " ");
        //    }
        //    Console.WriteLine("");
        //  }
        //  Console.WriteLine("");
        //}

        //static void ShowAllAnswers(SolverContext problem, Decision[][] grid)
        //{
        //  Solution soln = problem.Solve();
        //  while (soln.Quality == SolverQuality.Feasible)
        //  {
        //    ShowAnswer(grid);
        //    soln.GetNext();
        //  }
        //}

       
    } // Program

}

