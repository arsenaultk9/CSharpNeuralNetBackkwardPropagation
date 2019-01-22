using System;

namespace CSharpNeuralNetBackkwardPropagation
{
    public class Pattern
    {
        public Pattern(string value, int inputSize)
        {
            var line = value.Split(';');

            if (line.Length - 1 != inputSize)
                throw new Exception("Input does not match network configuration");
            Inputs = new double[inputSize];

            for (var i = 0; i < inputSize; i++)
            {
                Inputs[i] = double.Parse(line[i]);
            }

            Output = double.Parse(line[inputSize]);
        }

        public double[] Inputs { get; }
        public double Output { get; }
    }
}
