using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake.Controllers;

namespace Snake
{
    public sealed class GameInstance
    {
        private readonly GridDescription gridDescription;
        private readonly Snake snake;
        private readonly ISnakeController controller;
        private readonly RandomFoodGenerator randomFoodGenerator;

        private Vector2 foodPosition;
        private double elapsedTimeSinceMove;
        private double speed;

        public bool IsSnakeAlive { get; set; }
        public int Score { get; private set; }
        public int Id => controller.Id;

        internal GameInstance(GridDescription gridDescription, Snake snake, ISnakeController controller, RandomFoodGenerator randomFoodGenerator)
        {
            this.gridDescription = gridDescription;
            this.snake = snake;
            this.controller = controller;
            this.randomFoodGenerator = randomFoodGenerator;

            foodPosition = GenerateFoodPosition();
            elapsedTimeSinceMove = 0;
            speed = 10;
            IsSnakeAlive = true;
            Score = 0;
        }

        internal void Update(GameTime gameTime)
        {
            var newDirection = GetNewDirection();

            elapsedTimeSinceMove += gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedTimeSinceMove >= 1 / speed)
            {
                elapsedTimeSinceMove = 0.0;
                snake.Update(newDirection);

                if (snake.HasHitSelf() || snake.IsOutOfBounds(gridDescription))
                {
                    IsSnakeAlive = false;
                    return;
                }

                if (snake.HasEatenFood(foodPosition))
                {
                    foodPosition = GenerateFoodPosition();
                    Score++;
                    speed += 0.1f;
                    snake.AddBodyPart();
                }
            }
        }

        internal void Draw(SpriteBatch spriteBatch, Texture2D snakeHeadTexture, Texture2D snakeBodyTexture, Texture2D foodTexture)
        {
            spriteBatch.Draw(snakeHeadTexture, snake.Positions[0] * gridDescription.GridSize, Color.White);

            foreach (var body in snake.Positions.Skip(1))
                spriteBatch.Draw(snakeBodyTexture, body * gridDescription.GridSize, Color.White);

            spriteBatch.Draw(foodTexture, foodPosition * gridDescription.GridSize, Color.White);
        }

        private Direction GetNewDirection()
        {
            var controllerInputs = new SnakeControllerInputs(snake.Positions, foodPosition, gridDescription);
            var newDirection = controller.ChooseDirection(controllerInputs);

            if (snake.CurrentDirection == newDirection.Opposite() || newDirection == Direction.None)
                newDirection = snake.CurrentDirection;

            return newDirection;
        }

        private Vector2 GenerateFoodPosition()
        {
            var emptySquares = randomFoodGenerator.GetEmptyGridSquares(snake.Positions, foodPosition);
            return randomFoodGenerator.GenerateFoodPosition(emptySquares);
        }
    }
}
