using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using FlappyBird.Control;

namespace FlappyBird
{
    internal class FlappyBirdGame : Game
    {
        private const int ScreenWidth = 800;
        private const int ScreenHeight = 480;
        private const int BirdX = 100;
        private const float Gravity = 0.5f;
        private const int MaxSpeed = 10;
        private const int MinNewPipeTime = 500;

        private readonly RandomPipeGenerator randomPipeGenerator;
        private readonly GraphicsDeviceManager graphics;
        private readonly Vector2 scorePosition;

        private SpriteBatch spriteBatch;
        private Texture2D birdTexture;
        private Texture2D pipeTexture;
        private SpriteFont font;

        private List<Pipe> pipes;
        private Pipe nextPipe;
        private int pipeSpeed;
        private int nextPipeTime;
        private int timeSinceLastPipe;
        private int maxScore;
        
        protected IReadOnlyList<Bird> Birds { get; private set; }
        protected List<Bird> LivingBirds { get; private set; }
        protected IReadOnlyList<IFlappyBirdController> Controllers { get; set; }

        internal FlappyBirdGame(IReadOnlyList<IFlappyBirdController> controllers)
        {
            Controllers = controllers;
            graphics = new GraphicsDeviceManager(this);
            scorePosition = new Vector2(ScreenWidth / 2f, 50);
            randomPipeGenerator = new RandomPipeGenerator(ScreenHeight, ScreenWidth);

            Content.RootDirectory = "Content";
            Window.Title = "Flappy Bird";
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.ApplyChanges();

            base.Initialize();
            Restart();
        }

        protected virtual void Restart()
        {
            Birds = CreateBirds();
            LivingBirds = Birds.ToList();

            maxScore = 0;
            pipes = new List<Pipe>();
            pipeSpeed = 5;
            nextPipeTime = 2000;
            timeSinceLastPipe = 0;
            nextPipe = randomPipeGenerator.GenerateNewPipe(pipeTexture.Width);
            pipes.Add(nextPipe);
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            birdTexture = Content.Load<Texture2D>("BirdTexture");
            pipeTexture = Content.Load<Texture2D>("PipeTexture");
            font = Content.Load<SpriteFont>("Score");
        }

        protected override void UnloadContent()
        {
            birdTexture.Dispose();
            pipeTexture.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!LivingBirds.Any())
                Restart();

            foreach (var bird in LivingBirds)
                bird.Update(nextPipe);

            LivingBirds = Birds.Where(bird => bird.IsAlive).ToList();

            UpdatePipes(gameTime);
            
            base.Update(gameTime);   
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var bird in LivingBirds)
                spriteBatch.Draw(birdTexture, new Vector2(BirdX, bird.Y), Color.White);

            foreach (var pipe in pipes)
            {
                spriteBatch.Draw(pipeTexture, pipe.Top, Color.White);
                spriteBatch.Draw(pipeTexture, pipe.Bottom, Color.White);
            }

            spriteBatch.DrawString(font, maxScore.ToString(), scorePosition, Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private IReadOnlyList<Bird> CreateBirds()
        {
            return Controllers.Select(controller => new Bird(controller, BirdX, birdTexture.Width, Gravity, ScreenHeight)).ToList();
        }

        private void UpdatePipes(GameTime gameTime)
        {
            timeSinceLastPipe += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastPipe >= nextPipeTime)
            {
                var newPipe = randomPipeGenerator.GenerateNewPipe(pipeTexture.Width);
                pipes.Add(newPipe);
                nextPipe ??= newPipe;
                timeSinceLastPipe = 0;
            }

            foreach (var pipe in pipes)
                pipe.Update(pipeSpeed);

            if (nextPipe?.RightSide < BirdX)
            {
                nextPipe = pipes.FirstOrDefault(pipe => pipe.RightSide > BirdX);
                maxScore++;

                if (maxScore % 10 == 0)
                {
                    if (pipeSpeed < MaxSpeed)
                        pipeSpeed++;

                    if (nextPipeTime > MinNewPipeTime)
                        nextPipeTime -= 100;
                }
            }

            if (pipes.FirstOrDefault()?.IsOffScreen() == true)
                pipes.RemoveAt(0);
        }
    }
}  