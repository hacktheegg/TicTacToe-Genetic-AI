using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPT_based_AI_for_TicTacToe
{
    public class AIs
    {
        /*public void NeuralLearning(int Board)
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
        }*/
    }





    /*public class CandidateSolution
    {
        private readonly double[] output1;
        private readonly double[] output2;

        public CandidateSolution(int numGenes, int outputLength)
        {
            output1 = new double[numGenes];
            output2 = new double[outputLength];
        }

        public void Randomize(double minValue, double maxValue)
        {
            Random rand = new Random();
            for (int i = 0; i < output1.Length; i++)
            {
                output1[i] = rand.NextDouble() * (maxValue - minValue) + minValue;
            }
            for (int i = 0; i < output2.Length; i++)
            {
                output2[i] = rand.NextDouble() * (maxValue - minValue) + minValue;
            }
        }

        public int Output1()
        {
            double sigmoid1 = 1.0 / (1.0 + Math.Exp(-output1[0]));
            return (int)(sigmoid1 * 3) % 3;
        }

        public int Output2()
        {
            double sigmoid2 = 1.0 / (1.0 + Math.Exp(-output2[0]));
            return (int)(sigmoid2 * 3) % 3;
        }

        public double Fitness { get; set; }
        public int ChromosomeLength => output1.Length;
    }

    public class GeneticAlgorithm
    {
        private readonly int populationSize;
        private readonly double mutationRate;
        private readonly double crossoverRate;
        private readonly int elitismCount;

        public GeneticAlgorithm(int populationSize, double mutationRate, double crossoverRate, int elitismCount)
        {
            this.populationSize = populationSize;
            this.mutationRate = mutationRate;
            this.crossoverRate = crossoverRate;
            this.elitismCount = elitismCount;
        }

        public List<CandidateSolution> GeneratePopulation(int chromosomeLength, double minValue, double maxValue)
        {
            var population = new List<CandidateSolution>(populationSize);

            for (int i = 0; i < populationSize; i++)
            {
                var candidate = new CandidateSolution(chromosomeLength);
                candidate.Randomize(minValue, maxValue);
                population.Add(candidate);
            }

            return population;
        }

        private List<CandidateSolution> GetElites(List<CandidateSolution> population, int elitismCount)
        {
            population.Sort((x, y) => y.Fitness.CompareTo(x.Fitness)); // sort in descending order
            var elites = population.GetRange(0, Math.Min(elitismCount, population.Count));
            return elites;
        }

        private CandidateSolution Crossover(CandidateSolution parent1, CandidateSolution parent2)
        {
            var child = new CandidateSolution(parent1.ChromosomeLength);

            for (int i = 0; i < parent1.ChromosomeLength; i++)
            {
                if (Globals.RandomGenerator.NextDouble() < crossoverRate)
                {
                    child.Parameters[i] = parent1.Parameters[i];
                }
                else
                {
                    child.Parameters[i] = parent2.Parameters[i];
                }
            }

            return child;
        }

        private void Mutate(CandidateSolution candidate, double minValue, double maxValue)
        {
            for (int i = 0; i < candidate.ChromosomeLength; i++)
            {
                if (Globals.RandomGenerator.NextDouble() < mutationRate)
                {
                    candidate.Parameters[i] = Globals.RandomGenerator.NextDouble() * (maxValue - minValue) + minValue;
                }
            }
        }

        public List<CandidateSolution> EvolvePopulation(List<CandidateSolution> population, double minValue, double maxValue)
        {
            var elites = GetElites(population, elitismCount);

            var offspring = new List<CandidateSolution>(populationSize - elites.Count);

            while (offspring.Count < populationSize - elites.Count)
            {
                var parent1 = SelectParent(population);
                var parent2 = SelectParent(population);

                var child = Crossover(parent1, parent2);

                Mutate(child, minValue, maxValue);

                offspring.Add(child);
            }

            elites.AddRange(offspring);

            return elites;
        }

        private CandidateSolution SelectParent(List<CandidateSolution> population)
        {
            double fitnessSum = population.Sum(candidate => candidate.Fitness);
            double rouletteWheelPosition = Globals.RandomGenerator.NextDouble() * fitnessSum;

            double spinWheel = 0;

            foreach (CandidateSolution candidate in population)
            {
                spinWheel += candidate.Fitness;

                if (spinWheel >= rouletteWheelPosition)
                {
                    return candidate;
                }
            }

            // if we get here, something went wrong, so return the last candidate
            return population.Last();
        }
    }

    public static class Globals
    {
        public static Random RandomGenerator = new Random();
    }*/
}