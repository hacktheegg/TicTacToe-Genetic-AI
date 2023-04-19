using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ObjectList;
using System.Diagnostics.Contracts;
using ChatGPT_based_AI_for_TicTacToe;
using Microsoft.VisualBasic;

class Program
{
    static void Main(string[] args)
    {

        int GenomeLength = 9;
        int GenomeValueMax = 8;
        int GenomeAmount = 9;
        int AmountMutated = 2;

        int PopulationSize = 500;

        float[][] stats = new float[2][];
        stats[0] = new float[PopulationSize];
        stats[1] = new float[PopulationSize];

        int loop = 1;
        float[] statsPercentage = new float[2];


        /////////////////
        // Innitialise //
        /////////////////
        int[][][] GenomeA = new int[PopulationSize][][];
        int[][][] GenomeB = new int[PopulationSize][][];
        for (int i = 0; i < PopulationSize; i++)
        {
            GenomeA[i] = new int[GenomeAmount][];
            GenomeB[i] = new int[GenomeAmount][];

            for (int j = 0; j < GenomeAmount; j++)
            {
                GenomeA[i][j] = AI.GenerateRandomArray(GenomeLength, GenomeValueMax);
                GenomeB[i][j] = AI.GenerateRandomArray(GenomeLength, GenomeValueMax);
            }
        }
        
        

        while (true)
        {
            stats[0] = new float[PopulationSize];
            stats[1] = new float[PopulationSize];

            bool[] winner;

            for (int i = 0; i < PopulationSize; i++)
            {
                for (int j = 0; j < PopulationSize; j++)
                {
                    winner = Game(GenomeA[i], GenomeB[j]);

                    if (winner[1])
                    {
                        if (!winner[0])
                        {
                            stats[0][i]++;
                        }
                        else
                        {
                            stats[1][j]++;
                        }
                    } else
                    {
                        stats[0][i] += (float)0.5;
                        stats[1][j] += (float)0.5;
                    }
                }
            }



            int[][] BestParentA = AI.BestParent(GenomeA, stats[0]);
            int[][] BestParentB = AI.BestParent(GenomeB, stats[1]);



            GenomeA = AI.EvolvePopulation(BestParentA, GenomeValueMax, GenomeAmount, PopulationSize, AmountMutated);
            GenomeB = AI.EvolvePopulation(BestParentB, GenomeValueMax, GenomeAmount, PopulationSize, AmountMutated);



            statsPercentage[0] = stats[0].Sum() + statsPercentage[0];
            statsPercentage[1] = stats[1].Sum() + statsPercentage[1];

            Console.WriteLine($"" +
                    $"%{(float)statsPercentage[0] / (float)statsPercentage.Sum() * 100 + ", ",-15} " +
                    $"%{(float)statsPercentage[1] / (float)statsPercentage.Sum() * 100 + ", ",-15}");





            /*if (winner[1])
            {
                if (!winner[0])
                {
                    stats[0]++;

                    //int[] GenomeAMutated = AI.Mutate(GenomeA, GenomeValueMax);
                    GenomeB = AI.Mutate(GenomeB, GenomeValueMax, GenomeAmount);
                }
                else
                {
                    stats[1]++;

                    //int[] GenomeBMutated = AI.Mutate(GenomeA, GenomeValueMax);
                    GenomeA = AI.Mutate(GenomeA, GenomeValueMax, GenomeAmount);
                }
            }
            else
            {
                stats[2]++;

                GenomeB = AI.Mutate(GenomeB, GenomeValueMax, GenomeAmount);
                GenomeA = AI.Mutate(GenomeA, GenomeValueMax, GenomeAmount);
            }



            if (loop % 10000 == 0)
            {
                //Console.WriteLine(
                //(float)stats[0] / (float)stats.Sum() + ", " + 
                //(float)stats[1] / (float)stats.Sum() + ", " + 
                //(float)stats[2] / (float)stats.Sum());
                Console.WriteLine($"" +
                    $"%{(float)stats[0] / (float)stats.Sum() * 100 + ", ",-15} " +
                    $"%{(float)stats[1] / (float)stats.Sum() * 100 + ", ",-15} " +
                    $"%{(float)stats[2] / (float)stats.Sum() * 100}");
            }

            loop++;*/
        }
    }



    public class AI
    {
        public static int[] GenerateRandomArray(int length, int maxInt)
        {
            Random random = new Random();

            int[] array = new int[length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = random.Next(0, maxInt);
            }
            return array;
        }
        public static int[][] Mutate(int[][] Genome, int GenomeValueMax, int GenomeAmount, int AmountMutated)
        {
            Random random = new Random();

            for (int i = 0; i < AmountMutated; i++)
            {
                int randomGenome = random.Next(0, GenomeAmount);
                int randomIndex = random.Next(0, Genome.Length - 1);
                Genome[randomGenome][randomIndex] = random.Next(1, GenomeValueMax);
            }
            
            return Genome;
        }
        public static int[][] BestParent(int[][][] population, float[] stats)
        {
            int best = 0;

            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i] > stats[best])
                {
                    best = i;
                }
            }

            //Console.WriteLine(stats[best]);

            return population[best];
        }
        public static int[][][] EvolvePopulation(int[][] BestParent, int GenomeValueMax, int GenomeAmount, int PopulationSize, int AmountMutated)
        {
            int[][][] EvolvedGenome = new int[PopulationSize][][];

            for (int i = 0; i < PopulationSize; i++)
            {
                EvolvedGenome[i] = Mutate(BestParent, GenomeValueMax, GenomeAmount, AmountMutated);
            }

            return EvolvedGenome;
        }
    }



    public static bool[] Game(int[][] GenomeA, int[][] GenomeB)
    {
        int[][] Board = new int[3][];
        Board[0] = new int[3] { 0, 0, 0 };
        Board[1] = new int[3] { 0, 0, 0 };
        Board[2] = new int[3] { 0, 0, 0 };


        // false = X
        //  true = O
        bool turn = false;
        int TurnNo = 0;

        int tempInt;


        while (!(CheckState(Board) || !Board.SelectMany(x => x).Any(x => x == 0)))
        {
            //Thread.Sleep(25);

            //Console.Clear();
            //PrintTicTacToe(Board);

            int CurrentMove;
            int CurrentSubGenome = 0;

            int[][] CurrentGenome;

            if (!turn)
            {
                CurrentGenome = GenomeA;
            } else
            {
                CurrentGenome = GenomeB;
            }

        failedTurn:


            
            CurrentMove = CurrentGenome[CurrentSubGenome][TurnNo];



            int XAxis = CurrentMove % 3;
            int YAxis = Convert.ToInt32(Math.Floor((float)(CurrentMove / 3)));



            if (Board[YAxis][XAxis] != 0)
            {
                if (CurrentSubGenome < CurrentGenome.Length - 1)
                {
                    CurrentSubGenome++;

                    goto failedTurn;
                } else
                {
                    while (Board[YAxis][XAxis] != 0)
                    {
                        CurrentMove++;

                        if (CurrentMove > 8)
                        {
                            CurrentMove = 0;
                        }

                        XAxis = CurrentMove % 3;
                        YAxis = Convert.ToInt32(Math.Floor((float)(CurrentMove / 3)));
                    }
                }
            }


            if (!turn)
            {
                Board[YAxis][XAxis] = 1;
            } else
            {
                Board[YAxis][XAxis] = 2;
            }


            turn = !turn;

            TurnNo++;
        }
        //int tempInt;

        //Console.ReadKey(true);

        //Thread.Sleep(25);
        //Console.Clear();
        //PrintTicTacToe(Board);
        if (CheckState(Board))
        {
            //Console.WriteLine("Congrats \"" + XY(!turn) + "\", You won!");
        }
        else
        {
            //Console.WriteLine("It seems it is a tie, Good game for both sides.");
        }
        //Console.ReadKey(true);
        bool[] returnBool = { !turn, CheckState(Board) };
        return returnBool;
    }


    public static string XY(bool turn)
    {
        if (turn == false)
        {
            return "X";
        }
        else
        {
            return "O";
        }
    }


    public static bool CheckState(int[][] Board)
    {
        if (CheckStateVertical(Board))
        {
            return CheckStateVertical(Board);
        } else if (CheckStateHorizontal(Board))
        {
            return CheckStateHorizontal(Board);
        } else if (CheckStateDiagonal(Board))
        {
            return CheckStateDiagonal(Board);
        }
        return false;
    }
    public static bool CheckStateVertical(int[][] Board)
    {
        for (int iterate = 0; iterate <= 2; iterate++)
        {
            if (Board[iterate][0] == Board[iterate][1] && Board[iterate][1] == Board[iterate][2] && Board[iterate][0] != 0)
            {
                return true;
            }
        }
        return false;
    }
    public static bool CheckStateHorizontal(int[][] Board)
    {
        for (int iterate = 0; iterate <= 2; iterate++)
        {
            if (Board[0][iterate] == Board[1][iterate] && Board[1][iterate] == Board[2][iterate] && Board[0][iterate] != 0)
            {
                return true;
            }
        }
        return false;
    }
    public static bool CheckStateDiagonal(int[][] Board)
    {
        if (Board[0][0] == Board[1][1] && Board[1][1] == Board[2][2] && Board[0][0] != 0)
        {
            return true;
        } else if (Board[2][0] == Board[1][1] && Board[1][1] == Board[0][2] && Board[2][0] != 0)
        {
            return true;
        }
        return false;
    }



    public static void PrintTicTacToe(int[][] TicTacToe)
    {
        Line line;
        Circle circle;

        string[][] board = Board.Create(25, 25);
        board = Square.Create(new Square(25, 25, Tuple.Create(0, 0)), board);
        board = Line.Create(new Line(Tuple.Create(8, 2), Tuple.Create(8, 23)), board);
        board = Line.Create(new Line(Tuple.Create(16, 2), Tuple.Create(16, 23)), board);
        board = Line.Create(new Line(Tuple.Create(2, 8), Tuple.Create(23, 8)), board);
        board = Line.Create(new Line(Tuple.Create(2, 16), Tuple.Create(23, 16)), board);

        for (int Yitorator = 0; Yitorator < TicTacToe.Length; Yitorator++)
        {
            for (int Xitorator = 0; Xitorator < TicTacToe[Yitorator].Length; Xitorator++)
            {
                if (TicTacToe[Yitorator][Xitorator] == 1)
                {
                    line = new Line(
                        Tuple.Create((Xitorator * 8) + 4 - 2, (Yitorator * 8) + 4 - 2),
                        Tuple.Create((Xitorator * 8) + 4 + 3, (Yitorator * 8) + 4 + 3));
                    board = Line.Create(line, board);

                    line = new Line(
                        Tuple.Create((Xitorator * 8) + 4 - 2, (Yitorator * 8) + 4 + 2),
                        Tuple.Create((Xitorator * 8) + 4 + 3, (Yitorator * 8) + 4 - 3));
                    board = Line.Create(line, board);
                } else if (TicTacToe[Yitorator][Xitorator] == 2)
                {
                    circle = new Circle(3, Tuple.Create((Xitorator * 8) + 4, (Yitorator * 8) + 4));
                    board = Circle.Create(circle, board);
                }
                //Board[Yitorator][Xitorator] = (Xitorator*Difference)+Offset
            }
        }

        Board.Print(board);
    }
}