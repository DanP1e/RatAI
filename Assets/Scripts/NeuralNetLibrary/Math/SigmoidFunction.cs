namespace NeuralNetworkLibrary.Math
{
    public class SigmoidFunction : IActivationFunction
    {      
        public float GetSimple(float value)// Функция активации.
        {
            return (float)System.Math.Pow(1 + System.Math.Exp(-value), -1);
        } 
        public float GetDerivative(float value) // Функция производной от функции активации.
        {
            return (value * (1 - value));
        }
    }
}
