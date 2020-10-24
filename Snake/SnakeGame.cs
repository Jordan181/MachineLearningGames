using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake.Controllers;

namespace Snake
{
    internal class SnakeGame : Game
    {
        private const int ScreenWidth = 600;
        private const int ScreenHeight = 600;
        private const int GridSize = 20;
        private const int RowCount = ScreenHeight / GridSize;
        private const int ColumnCount = ScreenWidth / GridSize;

        private readonly GraphicsDeviceManager graphics;
        private readonly Vector2 scorePosition;
        private readonly Random rand;
        private readonly GridDescription gridDescription;

        private SpriteBatch spriteBatch;
        private SpriteFont scoreFont;
        private Texture2D snakeBodyTexture;
        private Texture2D snakeHeadTexture;
        private Texture2D foodTexture;
        private int maxScore;
        
        protected IReadOnlyList<GameInstance> GameInstances { get; private set; }
        protected List<GameInstance> ActiveGameInstances { get; private set; }
        protected IReadOnlyList<ISnakeController> Controllers { get; set; }
        
        public SnakeGame(IReadOnlyList<ISnakeController> controllers)
        {
            Controllers = controllers;
            graphics = new GraphicsDeviceManager(this);
            scorePosition = new Vector2(ScreenWidth / 2f, 50);
            rand = new Random();
            gridDescription = new GridDescription(GridSize, RowCount, ColumnCount);

            Content.RootDirectory = "Content";
            Window.Title = "Snake";
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
            GameInstances = CreateGameInstances();
            ActiveGameInstances = GameInstances.ToList();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scoreFont = Content.Load<SpriteFont>("Score");
            snakeBodyTexture = Content.Load<Texture2D>("snakeBody");
            snakeHeadTexture = Content.Load<Texture2D>("snakeHead");
            foodTexture = Content.Load<Texture2D>("food");
        }

        protected override void Update(GameTime gameTime)
        {
            if (!GameInstances.Any(game => game.IsSnakeAlive))
                Restart();

            foreach (var gameInstance in GameInstances)
                gameInstance.Update(gameTime);

            maxScore = ActiveGameInstances.Max(game => game.Score);
            ActiveGameInstances = GameInstances.Where(game => game.IsSnakeAlive).ToList();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            foreach (var game in ActiveGameInstances)
                game.Draw(spriteBatch, snakeHeadTexture, snakeBodyTexture, foodTexture);

            spriteBatch.DrawString(scoreFont, maxScore.ToString(), scorePosition, Color.Green);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private IReadOnlyList<GameInstance> CreateGameInstances()
        {
            var games = new List<GameInstance>(Controllers.Count);
            var seed = rand.Next();

            const int initialX = ScreenWidth / (2 * GridSize);
            const int initialY = ScreenHeight / (2 * GridSize);

            foreach (var controller in Controllers)
            {
                var snake = Snake.Create(initialX, initialY, 3);
                var randomFoodGenerator = new RandomFoodGenerator(gridDescription, new Random(seed));
                games.Add(new GameInstance(gridDescription, snake, controller, randomFoodGenerator));
            }

            return games;
        }
    }
}
