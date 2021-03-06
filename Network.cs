﻿using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpNeuralNetBackkwardPropagation
{
    public class Network
    {
        private readonly int _hiddenDims = 2;        // Number of hidden neurons.
        private readonly int _inputDims = 2;        // Number of input neurons.
        private int _iteration;            // Current training iteration.
        private readonly int _restartAfter = 2000;   // Restart training if iterations exceed this.
        private Layer _hidden;              // Collection of hidden neurons.
        private Layer _inputs;              // Collection of input neurons.
        private List<Pattern> _patterns;    // Collection of training patterns.
        private Neuron _output;            // Output neuron.
        private readonly Random _rnd = new Random(); // Global random number generator.

        public Network()
        {
            LoadPatterns();
            Initialise();
            Train();
            Test();
        }

        private void Train()
        {
            double error;
            do
            {
                error = 0;
                foreach (Pattern pattern in _patterns)
                {
                    double delta = pattern.Output - Activate(pattern);
                    AdjustWeights(delta);
                    error += Math.Pow(delta, 2);
                }
                Console.WriteLine("Iteration {0}\t Error {1:0.000}", _iteration, error);
                _iteration++;
                if (_iteration > _restartAfter) Initialise();
            } while (error > 0.1);
        }

        private void Test()
        {
            Console.WriteLine("\nBegin network testing\nPress Ctrl C to exit\n");
            while (true)
            {
                try
                {
                    Console.Write("Input x, y: ");
                    var values = Console.ReadLine() + ",0";
                    Console.WriteLine("{ 0:0}n", Activate(new Pattern(values, _inputDims)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private double Activate(Pattern pattern)
        {
            for (int i = 0; i < pattern.Inputs.Length; i++)
            {
                _inputs[i].Output = pattern.Inputs[i];
            }
            foreach (Neuron neuron in _hidden)
            {
                neuron.Activate();
            }
            _output.Activate();
            return _output.Output;
        }

        private void AdjustWeights(double delta)
        {
            _output.AdjustWeights(delta);
            foreach (Neuron neuron in _hidden)
            {
                neuron.AdjustWeights(_output.ErrorFeedback(neuron));
            }
        }

        private void Initialise()
        {
            _inputs = new Layer(_inputDims);
            _hidden = new Layer(_hiddenDims, _inputs, _rnd);
            _output = new Neuron(_hidden, _rnd);
            _iteration = 0;
            Console.WriteLine("Network Initialised");
        }

        private void LoadPatterns()
        {
            _patterns = new List<Pattern>();
            StreamReader file = File.OpenText("Patterns.csv");
            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                _patterns.Add(new Pattern(line, _inputDims));
            }
            file.Close();
        }
    }
}







