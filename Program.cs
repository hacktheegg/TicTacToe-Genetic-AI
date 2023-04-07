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
    static Random random = new Random();
    static int populationSize = 50;
    static int mutationRate = 10; // percentage
    static int elitism = 10; // percentage
    //static int[] target = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; // target array to match

    static void Main(string[] args)
    {
        // Create initial population //
        // POPULATION A
        int[][] populationA = new int[populationSize][];
        for (int i = 0; i < populationSize; i++)
        {
            populationA[i] = GenerateRandomArray();
        }
        // POPULATION B
        int[][] populationB = new int[populationSize][];
        for (int i = 0; i < populationSize; i++)
        {
            populationB[i] = GenerateRandomArray();
        }



        // Evolution loop
        int generation = 1;
        while (true)
        {
            // Sort population by fitness //

            populationA = populationA.OrderBy(x => Fitness(x)).ToArray();
            // POPULATION B
            populationB = populationB.OrderBy(x => Fitness(x)).ToArray();



            // Create new generation
            int[][] newPopulation = new int[populationSize][];
            int elitismCount = populationSize * elitism / 100;

            // Elitism: copy top percentage of population to next generation
            for (int i = 0; i < elitismCount; i++)
            {
                newPopulation[i] = populationA[i].ToArray();
            }

            // Breed remaining population
            for (int i = elitismCount; i < populationSize; i++)
            {
                int[] parent1 = SelectParent(populationA);
                int[] parent2 = SelectParent(populationA);
                int[] child = Breed(parent1, parent2);
                newPopulation[i] = child;
            }

            // Mutate new generation
            for (int i = elitismCount; i < populationSize; i++)
            {
                if (random.Next(0, 100) < mutationRate)
                {
                    Mutate(newPopulation[i]);
                }
            }

            populationA = newPopulation;
            generation++;
        }
    }



    static int[] GenerateRandomArray()
    {
        int[] array = Enumerable.Range(1, 9).ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = random.Next(i, array.Length);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
        return array;
    }

    static int Fitness(int[] arrayA, int[] arrayB)
    {
        int fitness = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == target[i])
            {
                fitness++;
            }
        }

        return fitness;
    }

    static int[] SelectParent(int[][] population)
    {
        int totalFitness = population.Sum(x => Fitness(x));
        int randomFitness = random.Next(0, totalFitness);
        int index = 0;
        while (randomFitness > 0)
        {
            randomFitness -= Fitness(population[index]);
            index++;
        }

        return index == 0 ? population[index] : population[index - 1];
    }

    static int[] Breed(int[] parent1, int[] parent2)
    {
        int[] child = new int[parent1.Length];
        int start = random.Next(0, parent1.Length);
        int end = random.Next(start, parent1.Length);
        for (int i = start; i <= end; i++)
        {
            child[i] = parent1[i];
        }
        for (int i = 0; i < parent2.Length; i++)
        {
            if (!child.Contains(parent2[i]))
            {
                for (int j = 0; j < child.Length; j++)
                {
                    if (child[j] == 0)
                    {
                        child[j] = parent2[i];
                        break;
                    }
                }
            }
        }
        return child;
    }

    static void Mutate(int[] array)
    {
        int index1 = random.Next(0, array.Length);
        int index2 = random.Next(0, array.Length);
        int temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }





















































    public static void Game()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*var geneticAlgorithm = new GeneticAlgorithm(populationSize: 10, mutationRate: 0.01, crossoverRate: 0.8, elitismCount: 2);
        var population = geneticAlgorithm.GeneratePopulation(chromosomeLength: 10, minValue: 0.0, maxValue: 1.0);

        foreach (var candidate in population)
        {
            candidate.Fitness = CalculateFitness(candidate);
        }

        for (int i = 0; i < numGenerations; i++)
        {
            population = geneticAlgorithm.EvolvePopulation(population, minValue: 0.0, maxValue: 1.0);

            foreach (var candidate in population)
            {
                candidate.Fitness = CalculateFitness(candidate);
            }
        }

        // Get the best candidate in the final population
        var bestCandidate = population.OrderByDescending(c => c.Fitness).First();
        Console.WriteLine($"Best candidate: {string.Join(", ", bestCandidate.Parameters)}");*/
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////







        //var geneticAlgorithm = new GeneticAlgorithm(populationSize: 10, mutationRate: 0.01, crossoverRate: 0.8, elitismCount: 2);
        //var population = geneticAlgorithm.GeneratePopulation(chromosomeLength: 10, minValue: 0.0, maxValue: 1.0);


        int[][] Board = new int[3][];
        Board[0] = new int[3] { 0, 0, 0 };
        Board[1] = new int[3] { 0, 0, 0 };
        Board[2] = new int[3] { 0, 0, 0 };


        // false = X
        //  true = O
        bool turn = false;
        bool failedTurn = false;

        int tempInt;


        while (!(CheckState(Board).Item1 || !Board.SelectMany(x => x).Any(x => x == 0)))
        {
        failedTurn:
            Console.Clear();
            PrintTicTacToe(Board);

            if (failedTurn)
            {
                Console.WriteLine("INVALID INPUT");
            }

            Console.WriteLine("Player \"" + XY(turn) + "\", it is currently your turn");

            Console.Write("X Axis: ");
            string XAxisString = Console.ReadKey(false).KeyChar.ToString().ToLower();
            if (!int.TryParse(XAxisString, out tempInt))
            {
                failedTurn = true;
                goto failedTurn;
            }
            else if (int.Parse(XAxisString) < 1 || int.Parse(XAxisString) > 3)
            {
                failedTurn = true;
                goto failedTurn;
            }
            int XAxis = int.Parse(XAxisString) - 1;

            Console.Write("\nY Axis: ");
            string YAxisString = Console.ReadKey(false).KeyChar.ToString().ToLower();
            if (!int.TryParse(YAxisString, out tempInt))
            {
                failedTurn = true;
                goto failedTurn;
            }
            else if (int.Parse(YAxisString) < 1 || int.Parse(YAxisString) > 3)
            {
                failedTurn = true;
                goto failedTurn;
            }
            int YAxis = int.Parse(YAxisString) - 1;


            if (Board[YAxis][XAxis] != 0)
            {
                failedTurn = true;
                goto failedTurn;
            }


            if (!turn)
            {
                Board[YAxis][XAxis] = 1;
            }
            else
            {
                Board[YAxis][XAxis] = 2;
            }


            turn = !turn;
            failedTurn = false;
        }
        //int tempInt;

        //Console.ReadKey(true);
        

        Console.Clear();
        PrintTicTacToe(Board);
        if (CheckState(Board).Item1)
        {
            Console.WriteLine("Congrats \"" + XY(!turn) + "\", You won!");
        }
        else
        {
            Console.WriteLine("It seems it is a tie, Good game for both sides.");
        }
        Console.ReadKey(true);
    }


    public static double EvaluateFitness(int[][] Board)
    {
        if (CheckState(Board).Item2 == 1)
        {
            return 1.0;
        } else
        {
            return 0.0;
        }
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



    public static Tuple<bool, int> CheckState(int[][] Board)
    {
        if (CheckStateVertical(Board).Item1)
        {
            return CheckStateVertical(Board);
        } else if (CheckStateHorizontal(Board).Item1)
        {
            return CheckStateHorizontal(Board);
        } else if (CheckStateDiagonal(Board).Item1)
        {
            return CheckStateDiagonal(Board);
        }
        return Tuple.Create(false, 0);
    }
    public static Tuple<bool, int> CheckStateVertical(int[][] Board)
    {
        for (int iterate = 0; iterate <= 2; iterate++)
        {
            if (Board[iterate][0] == Board[iterate][1] && Board[iterate][1] == Board[iterate][2] && Board[iterate][0] != 0)
            {
                return Tuple.Create(true, Board[iterate][0]);
            }
        }
        return Tuple.Create(false, 0);
    }
    public static Tuple<bool, int> CheckStateHorizontal(int[][] Board)
    {
        for (int iterate = 0; iterate <= 2; iterate++)
        {
            if (Board[0][iterate] == Board[1][iterate] && Board[1][iterate] == Board[2][iterate] && Board[0][iterate] != 0)
            {
                return Tuple.Create(true, Board[0][iterate]);
            }
        }
        return Tuple.Create(false, 0);
    }
    public static Tuple<bool, int> CheckStateDiagonal(int[][] Board)
    {
        if (Board[0][0] == Board[1][1] && Board[1][1] == Board[2][2] && Board[0][0] != 0)
        {
            return Tuple.Create(true, Board[0][0]);
        } else if (Board[2][0] == Board[1][1] && Board[1][1] == Board[0][2] && Board[2][0] != 0)
        {
            return Tuple.Create(true, Board[2][0]);
        }
        return Tuple.Create(false, 0);
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