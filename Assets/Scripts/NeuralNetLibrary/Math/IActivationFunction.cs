namespace NeuralNetworkLibrary.Math
{
    public interface IActivationFunction
    {
         float GetDerivative(float _in);
         float GetSimple(float _in);
    }
}