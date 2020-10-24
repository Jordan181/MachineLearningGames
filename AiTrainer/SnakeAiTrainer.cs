using System;
using System.Collections.Generic;
using System.Linq;
using MachineLearningGames.Framework.FileIO;
using MachineLearningGames.Framework.GeneticTraining;
using Microsoft.Xna.Framework;
using Snake;
using Snake.Controllers;

namespace AiTrainer
{
    internal class SnakeAiTrainer : SnakeGame
    {
        private readonly AiTrainingFramework<ISnakeController> aiTrainingFramework;

        private const int FoodLifeBonus = 15000;
        private Dictionary<int, int> aliveTime;
        private Dictionary<int, int> timeLife;
        private Dictionary<int, int> oldTickScores;
        
        internal SnakeAiTrainer(AiTrainingFramework<ISnakeController> aiTrainingFramework, IReadOnlyList<ISnakeController> controllers) 
            : base(controllers)
        {
            this.aiTrainingFramework = aiTrainingFramework;
        }

        protected override void Restart()
        {
            if (GameInstances != null)
            {
                var scores = GameInstances.Select(game => (game.Id, CalculateFitness(game))).ToList();
                Controllers = aiTrainingFramework.CreateNewControllers(scores);
            }
            
            aliveTime = Controllers.ToDictionary(c => c.Id, c => 0);
            timeLife = Controllers.ToDictionary(c => c.Id, c => FoodLifeBonus);
            oldTickScores = Controllers.ToDictionary(c => c.Id, c => 0);

            base.Restart();
        }

        protected override void LoadContent()
        {
            aiTrainingFramework.LoadContent(GraphicsDevice, Content);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var gameInstance in ActiveGameInstances)
            {
                var id = gameInstance.Id;
                var elapsedMilliseconds = gameTime.ElapsedGameTime.Milliseconds;

                aliveTime[id] += elapsedMilliseconds;

                if (gameInstance.Score > oldTickScores[id])
                {
                    oldTickScores[id] = gameInstance.Score;
                    timeLife[id] = FoodLifeBonus;
                }
                else
                {
                    timeLife[id] -= elapsedMilliseconds;

                    if (timeLife[id] <= 0)
                        gameInstance.IsSnakeAlive = false;
                }
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            aiTrainingFramework.DrawTrainingText();
        }

        private int CalculateFitness(GameInstance game)
        {
            var timeMultiplier = 1f;

            if (game.Score < 10)
                timeMultiplier = 0.1f;

            return game.Score * 10000 + (int)(aliveTime[game.Id] * timeMultiplier);
        }
    }
}
