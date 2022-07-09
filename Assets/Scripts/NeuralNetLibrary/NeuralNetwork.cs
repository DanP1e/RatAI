using NeuralNetworkLibrary.Math;
using System;

namespace NeuralNetworkLibrary
{
    public class NeuralNetwork : ICloneable
    {
        private static JaggedArrayCopier<Neuron> s_neuronsMapCopier = new JaggedArrayCopier<Neuron>();
        private static JaggedArrayCopier<float> s_weightsMapCopier = new JaggedArrayCopier<float>();
        private IActivationFunction _activationFunction = new SigmoidFunction();
        private Random _random;

        public Neuron[][] Neurons { get; private set; }
        public float[][] NeuronWeights { get; private set; }

        public NeuralNetwork(Neuron[][] neurons, float[][] neuronWeights, IActivationFunction activationFunction)
        {
            Neurons = neurons;
            NeuronWeights = neuronWeights;
            _activationFunction = activationFunction;
        }
        // Генерация нейросети с пустыми входами и рандомными весами
        public NeuralNetwork(params int[] layers)
        {
            if (layers.Length < 2) 
                throw new ArgumentException("Invalid number of parameters, at least 2");

            Neurons = new Neuron[layers.Length][];
            NeuronWeights = new float[layers.Length - 1][];
            Neurons[0] = new Neuron[layers[0]];
            for (int neuron = 0; neuron < Neurons[0].Length; neuron++)
            {
                Neurons[0][neuron] = new Neuron();
            }
            for (int layer = 1; layer < layers.Length; layer++) 
            {
                Neurons[layer] = new Neuron[layers[layer]];              
                for (int neuron = 0; neuron < Neurons[layer].Length; neuron++)
                {
                    Neurons[layer][neuron] = new Neuron();
                }
                NeuronWeights[layer - 1] = new float[Neurons[layer].Length * Neurons[layer - 1].Length];
                for(int weightID = 0; weightID < NeuronWeights[layer - 1].Length; weightID++) 
                {
                    NeuronWeights[layer - 1][weightID] = NextFloat(-0.5f, 0.5f);
                }
            }
        }
        // Получает значения выходных нейронов.
        public float[] GetOutputValues()
        {
            float[] outputs = new float[Neurons.Length];
            int counter = 0;
            foreach (var i in Neurons[Neurons.Length - 1]) 
            {
                outputs[counter] = i.Value;
                counter++;
            }
            return outputs;
        }
        // Устанавливает значения входных нейронов.
        public void SetNetworkInputValues(params float[] arrayOfInputValues) 
        {
            if (arrayOfInputValues.Length != Neurons[0].Length)
            {
                throw new ArgumentException("The number of input values does not match the number of input neurons.");
            }
            for (int i = 0; i < Neurons[0].Length; i++)
            {
                Neurons[0][i].Value = arrayOfInputValues[i];
            }
        }
        // Считает размер нейронов основываясь на весах и входных данных.
        public void CalculateNetwork() 
        {
            for (int layer = 1; layer < Neurons.Length; layer++)
            {
                for (int neuron = 0; neuron < Neurons[layer].Length; neuron++)
                {
                    Neurons[layer][neuron].Value = 0;
                    int startId = neuron * Neurons[layer - 1].Length;
                    for (int previousLayerNeuron = 0; previousLayerNeuron < Neurons[layer - 1].Length; previousLayerNeuron++)
                    {
                        Neurons[layer][neuron].Value += Neurons[layer - 1][previousLayerNeuron].Value * NeuronWeights[layer - 1][startId + previousLayerNeuron];
                    }
                    Neurons[layer][neuron].Value = _activationFunction.GetSimple(Neurons[layer][neuron].Value);
                }
            }
        }
        // Ищет ошибки обратным распространением.
        public void CalculateErrorsByBackpropagating(float[] trueOutputValues)
        {
            SetupOutputNeuronsError(trueOutputValues);
            ClearNeuronsError();
            for (int layer = Neurons.Length - 1; layer > 0; layer--) // Обратное распространение ошибки
            {
                for (int neuron = 0; neuron < Neurons[layer].Length; neuron++)
                {
                    int startId = neuron * Neurons[layer - 1].Length;
                    for (int previousLayerNeuron = 0; previousLayerNeuron < Neurons[layer - 1].Length; previousLayerNeuron++)
                    {
                        Neurons[layer - 1][previousLayerNeuron].Error += Neurons[layer][neuron].Error * NeuronWeights[layer - 1][startId + previousLayerNeuron];
                    }
                }
            }
        }
        // Коректирует веса относительно ошибок.
        public void CorrectWeights(float step = 0.1f, float extraWeight = 0)
        {
            for (int layer = 1; layer < Neurons.Length; layer++)
            {
                for (int neuron = 0; neuron < Neurons[layer].Length; neuron++)
                {
                    int startId = neuron * Neurons[layer - 1].Length;
                    for (int previousLayerNeuron = 0; previousLayerNeuron < Neurons[layer - 1].Length; previousLayerNeuron++)
                    {

                        NeuronWeights[layer - 1][startId + previousLayerNeuron] += 
                            (Neurons[layer][neuron].Error
                             * _activationFunction.GetDerivative(Neurons[layer][neuron].Value)
                             * Neurons[layer - 1][previousLayerNeuron].Value
                             * step);
                    }
                }
            }
        }
        // Устанавливает все ошибки нейронов в 0
        public void ClearNeuronsError()
        {
            for (int layer = 0; layer < Neurons.Length; layer++)
            {
                for (int neuron = 0; neuron < Neurons[layer].Length; neuron++)
                {
                    Neurons[layer][neuron].Error = 0;
                }
            }
        }
        // Добавляет к весам случайное значение от -mutationRange до mutationRange
        public void MutateWeights(float mutationRange)
        {
            for (int layer = 0; layer < NeuronWeights.Length; layer++)
            {
                for (int weightID = 0; weightID < NeuronWeights[layer].Length; weightID++)
                {
                    NeuronWeights[layer][weightID] += NextFloat(-mutationRange, mutationRange);
                }
            }
        }
        public float GetSize() => Neurons.GetLength(0);
        public object Clone()
        {
            Neuron[][] neuronsMap = s_neuronsMapCopier.CreateCopy(Neurons);
            float[][] weightsMap = s_weightsMapCopier.CreateCopy(NeuronWeights);

            NeuralNetwork cloneNet = new NeuralNetwork(neuronsMap, weightsMap, _activationFunction);
            return cloneNet;
        }

        private float NextFloat(float min, float max) => (float)(_random.NextDouble() * (max - min) + min);
        // Устанавливает ошибку выходных нейронов
        private void SetupOutputNeuronsError(float[] trueOutputValues)
        {
            for (int c = 0; c < Neurons[Neurons.Length - 1].Length; c++)
            {
                Neurons[Neurons.Length - 1][c].Error = trueOutputValues[c] - Neurons[Neurons.Length - 1][c].Value;
            }
        }     
    }
}
