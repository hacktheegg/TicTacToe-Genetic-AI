using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ObjectList;
using System.Diagnostics.Contracts;
using Microsoft.VisualBasic;
using System.Data.Common;

class Program
{
    static void Main(string[] args)
    {

        int GenomeLength = 9;
        int GenomeValueMax = 8;
        int GenomeAmount = 3;
        int AmountMutated = 9;

        int PopulationSize = 100;

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



        bool[][] ThreadResults = new bool[5][];



        while (true)
        {
            stats[0] = new float[PopulationSize];
            stats[1] = new float[PopulationSize];

            bool[] winner;

            for (int i = 0; i < PopulationSize - 1; i+=1)
            {
                
                for (int j = 0; j < PopulationSize - 1; j+=5)
                {
                    //Console.WriteLine(GenomeA.Length);
                    //Console.WriteLine(GenomeB.Length);

                    Thread ThreadGameA = new Thread(() => { ThreadResults[0] = Game(GenomeA[i], GenomeB[j+0], false); });
                    Thread ThreadGameB = new Thread(() => { ThreadResults[1] = Game(GenomeA[i], GenomeB[j+1], false); });
                    Thread ThreadGameC = new Thread(() => { ThreadResults[2] = Game(GenomeA[i], GenomeB[j+2], false); });
                    Thread ThreadGameD = new Thread(() => { ThreadResults[3] = Game(GenomeA[i], GenomeB[j+3], false); });
                    Thread ThreadGameE = new Thread(() => { ThreadResults[4] = Game(GenomeA[i], GenomeB[j+4], false); });



                    ThreadGameA.Start();
                    ThreadGameB.Start();
                    ThreadGameC.Start();
                    ThreadGameD.Start();
                    ThreadGameE.Start();
                    ThreadGameA.Join();
                    ThreadGameB.Join();
                    ThreadGameC.Join();
                    ThreadGameD.Join();
                    ThreadGameE.Join();
                    
                    for (int k = 0; k < 5; k++)
                    {
                        if (ThreadResults[k][1])
                        {
                            if (ThreadResults[k][0])
                            {
                                stats[0][i]++;
                            }
                            else
                            {
                                stats[1][j]++;
                            }
                        }
                        else
                        {
                            stats[0][i] += (float)0.5;
                            stats[1][j] += (float)0.5;
                        }
                    }
                    






                    /*winner = Game(GenomeA[i], GenomeB[j]);

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
                    }*/
                }
            }

            

            int[][] BestParentA = AI.BestParent(GenomeA, stats[0]);
            int[][] BestParentB = AI.BestParent(GenomeB, stats[1]);



            GenomeA = AI.EvolvePopulation(BestParentA, GenomeValueMax, GenomeAmount, PopulationSize, AmountMutated);
            GenomeB = AI.EvolvePopulation(BestParentB, GenomeValueMax, GenomeAmount, PopulationSize, AmountMutated);



            /*for (int i = 0; i < PopulationSize; i++)
            {
                GenomeA[i] = new int[GenomeAmount][];
                GenomeB[i] = new int[GenomeAmount][];

                for (int j = 0; j < GenomeAmount; j++)
                {
                    GenomeA[i][j] = AI.GenerateRandomArray(GenomeLength, GenomeValueMax);
                    GenomeB[i][j] = AI.GenerateRandomArray(GenomeLength, GenomeValueMax);
                }
            }*/





            statsPercentage[0] = stats[0].Sum() + statsPercentage[0];
            statsPercentage[1] = stats[1].Sum() + statsPercentage[1];

            if (loop % 10 == 0)
            {

                //Console.WriteLine($"" +
                //        $"%{(float)statsPercentage[0] / (float)statsPercentage.Sum() * 100 + ", ",-15} " +
                //        $"%{(float)statsPercentage[1] / (float)statsPercentage.Sum() * 100 + ", ",-15}");

                //Console.WriteLine($"" +
                //      $"%{(float)stats[0].Sum() / ((float)stats[0].Sum() + (float)stats[1].Sum()) * 100 + ", ",-15} " +
                //      $"%{(float)stats[1].Sum() / ((float)stats[0].Sum() + (float)stats[1].Sum()) * 100 + ", ",-15}");

                statsPercentage[0] = 0;
                statsPercentage[1] = 0;
            }

            //PrintArray(BestParentA);
            //Game(BestParentA,BestParentB,true);



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
            }*/

            loop++;
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

                //random = new Random();
            }
            return array;
        }
        public static int[][] Mutate(int[][] Genome, int GenomeValueMax, int GenomeAmount, int AmountMutated)
        {
            Random random = new Random(Genome[0][0]);

            int[][] evolvedGenome = Genome;

            for (int i = 0; i < AmountMutated; i++)
            {
                int randomGenome = random.Next(0, GenomeAmount);
                int randomIndex = random.Next(0, evolvedGenome.Length - 1);
                evolvedGenome[randomGenome][randomIndex] = random.Next(1, GenomeValueMax);

                //random = new Random();
            }
            PrintArray(Genome);
            PrintArray(evolvedGenome);

            return evolvedGenome;
        }
        public static int[][] BestParent(int[][][] population, float[] stats)
        {
            int best = 0;

            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i] >= stats[best])
                {
                    best = i;
                }
            }

            //Console.WriteLine(stats[best]);
            //Random random = new Random();
            //return population[random.Next(0, population.Length - 1)];
            return population[best];
        }
        public static int[][][] EvolvePopulation(int[][] BestParent, int GenomeValueMax, int GenomeAmount, int PopulationSize, int AmountMutated)
        {
            int[][][] EvolvedGenome = new int[PopulationSize][][];

            EvolvedGenome[0] = BestParent;

            for (int i = 1; i < PopulationSize; i++)
            {
                EvolvedGenome[i] = Mutate(BestParent, GenomeValueMax, GenomeAmount, AmountMutated);

            }

            return EvolvedGenome;
        }
    }



    public static bool[] Game(int[][] GenomeA, int[][] GenomeB, bool showGame)
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
            if (showGame == true)
            {
                Thread.Sleep(25);
                Console.Clear();
                PrintTicTacToe(Board);
            }
            //Thread.Sleep(25);

            //Console.Clear();
            //PrintTicTacToe(Board);

            //int CurrentMove;
            //int CurrentSubGenome = 0;

            int[][] CurrentGenome;

            if (!turn)
            {
                CurrentGenome = GenomeA;
            } else
            {
                CurrentGenome = GenomeB;
            }



            int CurrentMoveA = CurrentGenome[0][TurnNo];
            int CurrentMoveB = CurrentGenome[1][TurnNo];
            int CurrentMoveC = CurrentGenome[2][TurnNo];



        failedTurn:


            int XAxis;
            int YAxis;
            

            if (Board[Convert.ToInt32(Math.Floor((float)(CurrentMoveB / 3)))][CurrentMoveB % 3] != 0)
            {
                XAxis = CurrentMoveC % 3;
                YAxis = Convert.ToInt32(Math.Floor((float)(CurrentMoveC / 3)));
            } else
            {
                XAxis = CurrentMoveA % 3;
                YAxis = Convert.ToInt32(Math.Floor((float)(CurrentMoveA / 3)));
            }



            //if (Board[YAxis][XAxis] != 0)
            //{
            //if (CurrentSubGenome < CurrentGenome.Length - 1)
            //{
            //    CurrentSubGenome++;

            //    goto failedTurn;
            //} else
            //{
        

            /**/while (Board[YAxis][XAxis] != 0)/**/
            {
                
                if (Board[Convert.ToInt32(Math.Floor((float)(CurrentMoveB / 3)))][CurrentMoveB % 3] != 0)
                {
                    CurrentMoveC++;

                    if (CurrentMoveC > 8)
                    {
                        CurrentMoveC = 0;
                    }

                    XAxis = CurrentMoveC % 3;
                    YAxis = Convert.ToInt32(Math.Floor((float)(CurrentMoveC / 3)));
                } else
                {
                    CurrentMoveA++;

                    if (CurrentMoveA > 8)
                    {
                        CurrentMoveA = 0;
                    }

                    XAxis = CurrentMoveA % 3;
                    YAxis = Convert.ToInt32(Math.Floor((float)(CurrentMoveA / 3)));
                }
            }
                //}
            //}


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

        if (showGame == true)
        {
            Thread.Sleep(25);
            Console.Clear();
            PrintTicTacToe(Board);
            
            if (CheckState(Board))
            {
                Console.WriteLine("Congrats \"" + XY(!turn) + "\", You won!");
            }
            else
            {
                Console.WriteLine("It seems it is a tie, Good game for both sides.");
            }
        }
        //Thread.Sleep(25);
        //Console.Clear();
        //PrintTicTacToe(Board);
        
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

    public static void PrintArray(int[][] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < array[i].Length; j++)
            {
                Console.Write(array[i][j] + ", ");
            }
            Console.Write("\n");
        }
        Console.Write("\n");

        Thread.Sleep(1000);
    }
}