using Sudoku;
using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Core;



namespace Sudoku.recursive.backtracking
{
    public class BacktrackingSolver : ISudokuSolver
    {
        public void Solve(GrilleSudoku s)
        {

            int[][] sJaggedTableau = Enumerable.Range(0, 9).Select(i => Enumerable.Range(0, 9).Select(j => s.GetCellule(i, j)).ToArray()).ToArray();
            var sTableau = To2D(sJaggedTableau);
            Program.estValide(sTableau, 0);
            Enumerable.Range(0, 9).ToList().ForEach(i => Enumerable.Range(0, 9).ToList().ForEach(j => s.SetCell(i, j, sTableau[i, j])));

        }

        //IEnumerable<int> ColumnSelector(int i)
        //{
        //    return null;
        //}


        static T[,] To2D<T>(T[][] source)
        {
            try
            {
                int FirstDim = source.Length;
                int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular
                var result = new T[FirstDim, SecondDim];
                for (int i = 0; i < FirstDim; ++i)
                    for (int j = 0; j < SecondDim; ++j)
                        result[i, j] = source[i][j];
                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("The given jagged array is not rectangular.");
            }
        }


    }






    class Program
    {
        
                //fonction qui test si une valeur est bien absente d'une ligne 

                public static bool absentSurLigne(int[,] grille, int ligne, int valeur)
                {
                    for (int colonne = 0; colonne < 9; colonne++)
                    {
                        if (grille[ligne, colonne] == valeur)
                            return false;
                    }
                    return true;
                }

                //fonction qui test si une valeur est bien absente d'une colonne 

                public static bool absentSurColonne(int[,] grille, int colonne, int valeur)
                {
                    for(int ligne = 0; ligne < 9; ligne++)
                    {
                        if (grille[ligne, colonne] == valeur)
                            return false;
                    }
                    return true;
                }

                //fonction qui test si la valeur est bien absente sur le bloc 

                public static bool absentSurBloc(int[,] grille, int valeur, int ligne, int colonne)
                {
                    int o = ligne - (ligne % 3);
                    int p = colonne - (colonne % 3); 
                    for(int i=o; i< o + 3; i++)
                    {
                        for(int j=p; j<p + 3; j++)
                        {
                            if (grille[i, j] == valeur)
                                return false;
                        }
                    }

                    return true;
                }


                public static bool estValide(int[,] grille, int position)
                {
                    if (position == 9 * 9)
                        return true;

                    //on récupère les coord de la case 
                    int ligne = position / 9;
                    int colonne = position % 9;

                    //si case pas vide on passe à la suivante 
                    if (grille[ligne, colonne] != 0)
                        return estValide(grille, position + 1);

                    for(int k = 1; k<= 9; k++)
                    {
                        //si la valeur est possible
                        if(absentSurLigne(grille,ligne,k) && absentSurColonne(grille,colonne,k) && absentSurBloc(grille, k, ligne, colonne))
                        {
                            //on enregistre k dans la grille
                            grille[ligne, colonne] = k;
                            //appel recursive de la fonction estValide()
                            if (estValide(grille, position + 1))
                                return true; //si le choix est bon, on renvoit true
                        }
                    }
                    //on réinitialise la case si aucun chiffre bon
                    grille[ligne, colonne] = 0;
                    //on retourne false 
                    return false;
                }

                //fonction affichage 
                public static void affichage(int[,] grille)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            Console.Write(grille[i, j]);
                        }
                        Console.WriteLine();
                    }

                }




                static void Main(string[] args)
                {
                    /*int[,] grille =
                     {
                        {9,0,0,1,0,0,0,0,5},
                        {0,0,5,0,9,0,2,0,1},
                        {8,0,0,0,4,0,0,0,0},
                        {0,0,0,0,8,0,0,0,0},
                        {0,0,0,7,0,0,0,0,0},
                        {0,0,0,0,2,6,0,0,9},
                        {2,0,0,3,0,0,0,0,6},
                        {0,0,0,2,0,0,9,0,0},
                        {0,0,1,9,0,4,5,7,0}
                    };*/

                int[,] grille =
                {
                        { 7,8,0,4,0,0,1,2,0},
                        { 6,0,0,0,7,5,0,0,9 },
                        { 0,0,0,6,0,1,0,7,8 },
                        { 0,0,7,0,4,0,2,6,0 },
                        { 0,0,1,0,5,0,9,3,0 },
                        { 9,0,4,0,6,0,0,0,5 },
                        { 0,7,0,3,0,0,0,1,2 },
                        { 1,2,0,0,0,7,4,0,0 },
                        { 0,4,9,2,0,6,0,0,7 }
                };

                    

                    //afficher la grille avant 
                    Console.WriteLine("affichage avant");
                    Console.WriteLine('\n');
                    affichage(grille);

                    estValide(grille, 0);

                    Console.WriteLine("affichage apres");
                    Console.WriteLine('\n');
                    affichage(grille);

                }

       
        
    }




}
