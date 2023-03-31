using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



class Program
{
    static void Main(string[] args)
    {
        int[][] Board = new int[3][];
        Board[0] = new int[3] { 0, 0, 0 };
        Board[1] = new int[3] { 0, 0, 0 };
        Board[2] = new int[3] { 0, 0, 0 };



    }



    public static bool CheckState(int[,] Board)
    {
        if (CheckStateVertical(Board))
        {
            return true;
        } else if (CheckStateVertical(Board))
        {
            return true;
        } else if (CheckStateVertical(Board))
        {
            return true;
        }
        return false;
    }
    public static bool CheckStateVertical(int[,] Board)
    {
        for (int iterate = 0; iterate <= 2; iterate++)
        {
            if (Board[iterate, 0] == Board[iterate, 1] && Board[iterate, 1] == Board[iterate, 2])
            {
                return true;
            }
        }
        return false;
    }
    public static bool CheckStateHorizontal(int[,] Board)
    {
        for (int iterate = 0; iterate <= 2; iterate++)
        {
            if (Board[0, iterate] == Board[1, iterate] && Board[1, iterate] == Board[2, iterate])
            {
                return true;
            }
        }
        return false;
    }
    public static bool CheckStateDiagonal(int[,] Board)
    {
        if (Board[0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2])
        {
            return true;
        }
        return false;
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