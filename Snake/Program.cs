using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MachineLearningGames.Framework.Ai;
using Snake.Controllers;

[assembly: InternalsVisibleTo("AiTrainer")]
[assembly: InternalsVisibleTo("TestSnake")]

namespace Snake
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // AI Controller
            var neuralNetwork = NeuralNetwork.CreateFromFile(SnakeNeuralNetworkSpecification.Specification, @"Content\snakeWeights.json");
            var aiController = new SnakeAiController(1, neuralNetwork);

            // Keyboard Controller
            var keyboardController = new SnakeKeyboardController(1);

            // Change value of selectedController to play game with keyboard
            var selectedController = aiController;

            using (var game = new SnakeGame(new List<ISnakeController>{selectedController}))
                game.Run();
        }
    }
}
