using System;
using System.Collections.Generic;
using System.Linq;
using Driving2D;
using Driving2D.Controllers;
using MachineLearningGames.Framework.GeneticTraining;
using Microsoft.Xna.Framework;

namespace AiTrainer
{
    internal class Driving2DAiTrainer : Driving2DGame
    {
        private TimeSpan maxLapTime; 
        private readonly AiTrainingFramework<IDriving2DController> aiTrainingFramework;
        private int lastGenerationMaxLaps;

        internal Driving2DAiTrainer(AiTrainingFramework<IDriving2DController> aiTrainingFramework, IReadOnlyList<IDriving2DController> controllers)
            : base(controllers)
        {
            this.aiTrainingFramework = aiTrainingFramework;
            maxLapTime = TimeSpan.FromSeconds(15);
        }

        protected override void Restart()
        {
            if (Cars != null)
            {
                var maxLaps = Cars.Select(car => car.AllLapTimes.Count).Max();
                if (maxLaps > lastGenerationMaxLaps && maxLapTime > TimeSpan.FromSeconds(10))
                {
                    maxLapTime -= TimeSpan.FromSeconds(1);
                    lastGenerationMaxLaps = maxLaps;
                }

                var scores = Cars.Select(car => (ControllerId: car.Controller.Id, CalculateFitness(car))).ToList();
                Controllers = aiTrainingFramework.CreateNewControllers(scores);
            }

            base.Restart();
        }

        protected override void LoadContent()
        {
            aiTrainingFramework.LoadContent(GraphicsDevice, Content);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (var car in LivingCars)
            {
                if (car.CurrentLapTime >= maxLapTime)
                {
                    car.IsAlive = false;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            aiTrainingFramework.DrawTrainingText();
        }

        private int CalculateFitness(Car car)
        {
            var distanceScore = (car.TargetCheckPoint - 1) + TrackLength * car.AllLapTimes.Count;
            var timeMultiplier = car.AllLapTimes.Any()
                ? (int) car.AllLapTimes.Average(lapTime => (maxLapTime - lapTime).Seconds)
                : 0;

            return distanceScore * 10 * (timeMultiplier + 1);
        }
    }
}