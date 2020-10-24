using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Driving2D.Controllers;
using MachineLearningGames.Framework.Ai;

[assembly: InternalsVisibleTo("AiTrainer")]
[assembly: InternalsVisibleTo("TestDriving2D")]

namespace Driving2D
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // AI Controller
            var neuralNetwork = NeuralNetwork.CreateFromFile(Driving2DNeuralNetworkSpecification.Specification, @"Content\driving2DWeights.json");
            var aiController = new Driving2DAiController(1, neuralNetwork);

            // Keyboard Controller
            var keyboardController = new Driving2DKeyboardController(1);

            // Change value of selectedController to play game with keyboard
            var selectedController = aiController;

            using (var game = new Driving2DGame(new List<IDriving2DController>{ selectedController }))
                game.Run();
        }
    }
}
