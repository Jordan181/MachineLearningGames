using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FlappyBird.Control;
using FlappyBird.Controllers;
using MachineLearningGames.Framework.Ai;

[assembly: InternalsVisibleTo("AiTrainer")]

namespace FlappyBird
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // AI Controller
            var neuralNetwork = NeuralNetwork.CreateFromFile(FlappyBirdNeuralNetworkSpecification.Specification, @"Content\flappyBirdWeights.json");
            var aiController = new FlappyBirdAiController(1, neuralNetwork);

            // Keyboard Controller
            var keyboardController = new FlappyBirdKeyboardController(1);

            // Change value of selectedController to play game with keyboard
            var selectedController = aiController;

            using (var game = new FlappyBirdGame(new List<IFlappyBirdController> {selectedController}))
                game.Run();
        }
    }
}
