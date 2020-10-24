using System;
using Microsoft.Xna.Framework;

namespace FlappyBird
{
    internal sealed class RandomPipeGenerator
    {
        private readonly int screenHeight;
        private readonly int screenWidth;
        private readonly int minPipeLength;
        private readonly int minPipeGap;
        private readonly int maxPipeGap;
        private readonly Random randomGenerator;

        public RandomPipeGenerator(int screenHeight, int screenWidth)
        {
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            
            minPipeLength = (int) (0.2 * screenHeight);
            minPipeGap = (int) (0.25 * screenHeight);
            maxPipeGap = (int) (0.35 * screenHeight);
            randomGenerator = new Random();
        }

        public Pipe GenerateNewPipe(int pipeWidth)
        {
            var topLength = randomGenerator.Next(minPipeLength, screenHeight - minPipeLength - minPipeGap);
            var gap = randomGenerator.Next(minPipeGap, maxPipeGap);
            var bottomLength = screenHeight - topLength - gap;

            var top = new Rectangle(screenWidth, 0, pipeWidth, topLength);
            var bottom = new Rectangle(screenWidth, screenHeight - bottomLength, pipeWidth, bottomLength);
            return new Pipe(top, bottom);
        }
    }
}
