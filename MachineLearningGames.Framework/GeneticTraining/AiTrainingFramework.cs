using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.Ai;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MachineLearningGames.Framework.GeneticTraining
{
    public sealed class AiTrainingFramework<TController>
    {
        private readonly NeuralNetworkSpecification neuralNetworkSpecification;
        private readonly GeneticAlgorithm geneticAlgorithm;
        private readonly Vector2 trainingTextPosition;
        private readonly Color trainingTextColor;
        private readonly Func<int, NeuralNetwork, TController> createController;
        private SpriteBatch trainingSpriteBatch;
        private SpriteFont trainingFont;
        private int currentGeneration;
        
        public IReadOnlyDictionary<int, Weights> CurrentGenWeights { get; private set; }

        public AiTrainingFramework(
            NeuralNetworkSpecification neuralNetworkSpecification, 
            GeneticAlgorithm geneticAlgorithm,
            Vector2 trainingTextPosition,
            Color trainingTextColor,
            Func<int, NeuralNetwork, TController> createController)
        {
            this.neuralNetworkSpecification = neuralNetworkSpecification;
            this.geneticAlgorithm = geneticAlgorithm;
            this.trainingTextPosition = trainingTextPosition;
            this.trainingTextColor = trainingTextColor;
            this.createController = createController;
            currentGeneration = 0;
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager content)
        {
            trainingSpriteBatch = new SpriteBatch(graphics);
            trainingFont = content.Load<SpriteFont>("Training");
        }

        public IReadOnlyList<TController> CreateNewControllers(IReadOnlyList<(int id, int score)> fitnessScores)
        {
            currentGeneration++;

            CurrentGenWeights = geneticAlgorithm.EvolveWeights(fitnessScores);
            return CurrentGenWeights.Select(kvp =>
            {
                var neuralNetwork = new NeuralNetwork(kvp.Value, neuralNetworkSpecification.HiddenLayersActivationFunction, neuralNetworkSpecification.OutputLayerActivationFunction);
                return createController(kvp.Key, neuralNetwork);
            }).ToList();
        }

        public void DrawTrainingText()
        {
            var trainingMessage = $"TRAINING{Environment.NewLine}Current Generation: {currentGeneration}";
            trainingSpriteBatch.Begin();
            trainingSpriteBatch.DrawString(trainingFont, trainingMessage, trainingTextPosition, trainingTextColor);
            trainingSpriteBatch.End();
        }
    }
}