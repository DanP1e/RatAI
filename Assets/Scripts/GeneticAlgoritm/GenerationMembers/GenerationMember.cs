using UnityEngine;
using NeuralNetworkLibrary;

public abstract class GenerationMember : MonoBehaviour
{
    public NeuralNetwork NeuralNet { get; private set; }
    public float Score;

    public virtual void SetNeuralNetwork(NeuralNetwork neuralNetwork)
    {
        NeuralNet = neuralNetwork;
    }
}
