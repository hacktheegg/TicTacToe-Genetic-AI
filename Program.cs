using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using ObjectList;
using System.Diagnostics.Contracts;

class Program
{
    static void Main(string[] args)
    {
        int[][] Board = new int[3][];
        Board[0] = new int[3] { 0, 0, 0 };
        Board[1] = new int[3] { 0, 0, 0 };
        Board[2] = new int[3] { 0, 0, 0 };

        bool turn = false;
        bool failedTurn = false;
        
        int tempInt;
        
        Console.ReadKey(true);
        // false = X
        //  true = O

        while (!CheckState(Board))
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
            } else if (int.Parse(YAxisString) < 1 || int.Parse(YAxisString) > 3)
            {
                failedTurn = true;
                goto failedTurn;
            }
            int YAxis = int.Parse(YAxisString) - 1;

            if (!turn)
            {
                Board[YAxis][XAxis] = 1;
            } else
            {
                Board[YAxis][XAxis] = 2;
            }
            

            turn = !turn;
            Console.ReadKey(true);
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

    public static bool CheckState(int[][] Board)
    {
        if (CheckStateVertical(Board))
        {
            return true;
        } else if (CheckStateHorizontal(Board))
        {
            return true;
        } else if (CheckStateDiagonal(Board))
        {
            return true;
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






    public void AI (int Board)
    {
        // Define a neural network with one hidden layer and one output neuron
        int numInputNodes = 9;
        int numHiddenNodes = 3;
        int numOutputNodes = 1;
        double learningRate = 0.1;

        double[,] input = new double[,] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } };
        double[,] output = new double[,] { { 0 }, { 1 }, { 1 }, { 0 } };

        // Initialize weights with random values
        double[,] hiddenWeights = new double[numInputNodes, numHiddenNodes];
        double[,] outputWeights = new double[numHiddenNodes, numOutputNodes];
        Random rand = new Random();
        for (int i = 0; i < numInputNodes; i++)
        {
            for (int j = 0; j < numHiddenNodes; j++)
            {
                hiddenWeights[i, j] = rand.NextDouble() - 0.5;
            }
        }
        for (int i = 0; i < numHiddenNodes; i++)
        {
            for (int j = 0; j < numOutputNodes; j++)
            {
                outputWeights[i, j] = rand.NextDouble() - 0.5;
            }
        }

        // Train the network using backpropagation
        for (int epoch = 0; epoch < 1000; epoch++)
        {
            for (int i = 0; i < input.GetLength(0); i++)
            {
                // Forward pass
                double[] hiddenLayerOutput = new double[numHiddenNodes];
                double[] outputLayerOutput = new double[numOutputNodes];

                for (int j = 0; j < numHiddenNodes; j++)
                {
                    double sum = 0;
                    // Calculate hidden layer output
                    for (int k = 0; k < numInputNodes; k++)
                    {
                        sum += input[i, k] * hiddenWeights[k, j];
                    }
                    hiddenLayerOutput[j] = Math.Tanh(sum);
                }

                // Calculate output layer output
                for (int j = 0; j < numOutputNodes; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < numHiddenNodes; k++)
                    {
                        sum += hiddenLayerOutput[k] * outputWeights[k, j];
                    }
                    outputLayerOutput[j] = Math.Tanh(sum);
                }

                // Backward pass
                double[] outputError = new double[numOutputNodes];
                for (int j = 0; j < numOutputNodes; j++)
                {
                    outputError[j] = (output[i, j] - outputLayerOutput[j]) * (1 - outputLayerOutput[j] * outputLayerOutput[j]);
                }

                double[] hiddenError = new double[numHiddenNodes];
                for (int j = 0; j < numHiddenNodes; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < numOutputNodes; k++)
                    {
                        sum += outputError[k] * outputWeights[j, k];
                    }
                    hiddenError[j] = sum * (1 - hiddenLayerOutput[j] * hiddenLayerOutput[j]);
                }

                // Update weights
                for (int j = 0; j < numHiddenNodes; j++)
                {
                    for (int k = 0; k < numInputNodes; k++)
                    {
                        hiddenWeights[k, j] += learningRate * hiddenError[j] * input[i, k];
                    }
                }

                for (int j = 0; j < numOutputNodes; j++)
                {
                    for (int k = 0; k < numHiddenNodes; k++)
                    {
                        outputWeights[k, j] += learningRate * outputError[j] * hiddenLayerOutput[k];
                    }
                }
            }
        }
    }
}