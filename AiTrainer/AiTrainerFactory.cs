using System;
using System.Collections.Generic;
using Driving2D.Controllers;
using FlappyBird.Control;
using FlappyBird.Controllers;
using MachineLearningGames.Framework.Ai;
using MachineLearningGames.Framework.GeneticTraining;
using Microsoft.Xna.Framework;
using Snake.Controllers;

namespace AiTrainer
{
    internal enum Games
    {
        FlappyBird,
        Snake,
        Driving2D
    }

    internal static class AiTrainerFactory
    {
        private static readonly TrainingSpecification DefaultTrainingSpecification = new TrainingSpecification
        {
            PopulationSize = 100,
            TopFractionToKeep = 0.2,
            ChildrenFractionToGenerate = 0.75,
            MutationRate = 0.03,
            FittestKeptScoreThreshold = 0
        };

        public static Game CreateAiTrainer(Games game)
        {
            switch (game)
            {
                case Games.FlappyBird:
                    return CreateFlappyBirdAiTrainer();
                case Games.Snake:
                    return CreateSnakeAiTrainer();
                case Games.Driving2D:
                    return CreateDriving2DAiTrainer();
                default:
                    throw new ArgumentOutOfRangeException(nameof(game), game, null);
            }
        }

        private static FlappyBirdAiTrainer CreateFlappyBirdAiTrainer()
        {
            var neuralNetworkSpecification = FlappyBirdNeuralNetworkSpecification.Specification;

            var trainingSpec = DefaultTrainingSpecification;
            trainingSpec.FittestKeptScoreThreshold = 500;

            var geneticAlgorithm = CreateGeneticAlgorithm(neuralNetworkSpecification, trainingSpec);

            var trainingTextPosition = new Vector2(25, 25);
            var trainingTextColor = Color.Red;
            IFlappyBirdController createController(int id, NeuralNetwork neuralNetwork) => new FlappyBirdAiController(id, neuralNetwork);
            var aiTrainingFramework = new AiTrainingFramework<IFlappyBirdController>(neuralNetworkSpecification, geneticAlgorithm, trainingTextPosition, trainingTextColor, createController);
            var controllers = aiTrainingFramework.CreateNewControllers(new List<(int id, int score)>());

            return new FlappyBirdAiTrainer(aiTrainingFramework, controllers);
        }

        private static SnakeAiTrainer CreateSnakeAiTrainer()
        {
            var neuralNetworkSpecification = SnakeNeuralNetworkSpecification.Specification;
            var geneticAlgorithm = CreateGeneticAlgorithm(neuralNetworkSpecification, DefaultTrainingSpecification);

            var trainingTextPosition = new Vector2(25, 25);
            var trainingTextColor = Color.Green;
            ISnakeController createController(int id, NeuralNetwork neuralNetwork) => new SnakeAiController(id, neuralNetwork);
            var aiTrainingFramework = new AiTrainingFramework<ISnakeController>(neuralNetworkSpecification, geneticAlgorithm, trainingTextPosition, trainingTextColor, createController);
            var controllers = aiTrainingFramework.CreateNewControllers(new List<(int id, int score)>());

            return new SnakeAiTrainer(aiTrainingFramework, controllers);
        }

        private static Driving2DAiTrainer CreateDriving2DAiTrainer()
        {
            var neuralNetworkSpecification = Driving2DNeuralNetworkSpecification.Specification;

            var trainingSpec = new TrainingSpecification
            {
                PopulationSize = 100,
                TopFractionToKeep = 0.05,
                ChildrenFractionToGenerate = 0.92,
                MutationRate = 0.05,
                FittestKeptScoreThreshold = 0
            };
            
            var geneticAlgorithm = CreateGeneticAlgorithm(neuralNetworkSpecification, trainingSpec);

            var trainingTextPosition = new Vector2(10, 75);
            var trainingTextColor = Color.Blue;
            IDriving2DController createController(int id, NeuralNetwork neuralNetwork) => new Driving2DAiController(id, neuralNetwork);
            var aiTrainingFramework = new AiTrainingFramework<IDriving2DController>(neuralNetworkSpecification, geneticAlgorithm, trainingTextPosition, trainingTextColor, createController);
            var controllers = aiTrainingFramework.CreateNewControllers(new List<(int id, int score)>());

            return new Driving2DAiTrainer(aiTrainingFramework, controllers);
        }

        private static GeneticAlgorithm CreateGeneticAlgorithm(NeuralNetworkSpecification neuralNetworkSpecification, TrainingSpecification trainingSpecification)
        {
            var randomWeightsGenerator = new RandomWeightsGenerator(neuralNetworkSpecification);
            return new GeneticAlgorithm(randomWeightsGenerator, trainingSpecification, neuralNetworkSpecification);
        }
    }
}