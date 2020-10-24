using System;
using System.Collections.Generic;
using System.Linq;
using FlappyBird;
using FlappyBird.Control;
using MachineLearningGames.Framework.FileIO;
using MachineLearningGames.Framework.GeneticTraining;
using Microsoft.Xna.Framework;

namespace AiTrainer
{
    internal class FlappyBirdAiTrainer : FlappyBirdGame
    {
        private readonly AiTrainingFramework<IFlappyBirdController> aiTrainingFramework;

        private Dictionary<int, int> aliveTime;

        internal FlappyBirdAiTrainer(AiTrainingFramework<IFlappyBirdController> aiTrainingFramework, IReadOnlyList<IFlappyBirdController> controllers)
            : base(controllers)
        {
            this.aiTrainingFramework = aiTrainingFramework;
        }

        protected override void Restart()
        {
            if (Birds != null)
            {
                var maxScore = Birds.Select(bird => bird.Score).Max();
                if (maxScore >= 100)
                {
                    var primeBird = Birds.First(bird => bird.Score == maxScore);
                    var weights = aiTrainingFramework.CurrentGenWeights[primeBird.Id];
                    JsonFileWriter.SerializeWeights(weights, @"D:\VS Projects\MachineLearningGames\flappyBirdWeights.json");
                    Environment.Exit(0);
                }

                var scores = Birds.Select(bird => (bird.Id, (bird.Score + 1) * aliveTime[bird.Id])).ToList();
                Controllers = aiTrainingFramework.CreateNewControllers(scores);
            }

            aliveTime = Controllers.ToDictionary(c => c.Id, c => 0);

            base.Restart();
        }

        protected override void LoadContent()
        {
            aiTrainingFramework.LoadContent(GraphicsDevice, Content);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var bird in LivingBirds)
                aliveTime[bird.Id] += gameTime.ElapsedGameTime.Milliseconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            aiTrainingFramework.DrawTrainingText();
        }
    }
}
