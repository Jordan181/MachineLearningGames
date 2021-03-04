using System;
using System.Collections.Generic;
using System.Linq;
using Driving2D.Controllers;
using MachineLearningGames.Framework.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Driving2D
{
    public class Driving2DGame : Game
    {
        private const int ScreenWidth = 1280;
        private const int ScreenHeight = 960;

        private readonly GraphicsDeviceManager graphics;
        private readonly Vector2 currentLapTimePosition;
        private readonly Vector2 bestLapTimePosition;
        private readonly Dictionary<int, TimeSpan> bestLapTimes;
        private TimeSpan overallBestLapTime;

        private TrackManager trackManager;
        private SpriteBatch spriteBatch;
        private Texture2D carTexture;
        private Texture2D carBrakingTexture;
        private Texture2D track;
        private SpriteFont lapTimeFont;

        protected IReadOnlyList<Car> Cars { get; private set; }
        protected List<Car> LivingCars { get; private set; }
        protected IReadOnlyList<IDriving2DController> Controllers { get; set; }
        protected int TrackLength => trackManager.NumberOfCheckpoints;

        public Driving2DGame(IReadOnlyList<IDriving2DController> controllers)
        {
            Controllers = controllers;
            graphics = new GraphicsDeviceManager(this);
            currentLapTimePosition = new Vector2(10, 10);
            bestLapTimePosition = new Vector2(10, 30);
            bestLapTimes = Controllers.ToDictionary(x => x.Id, x => TimeSpan.Zero);

            Content.RootDirectory = "Content";
            Window.Title = "Driving 2D";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            base.Initialize();
            Restart();
        }

        protected virtual void Restart()
        {
            Cars = CreateCars();
            LivingCars = Cars.ToList();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lapTimeFont = Content.Load<SpriteFont>("LapTimeFont");
            carTexture = Content.Load<Texture2D>("car");
            carBrakingTexture = Content.Load<Texture2D>("carBraking");
            track = Content.Load<Texture2D>("simpleTrack");
            
            var trackBounds = Content.Load<Texture2D>("simpleTrackBounds");
            var checkPoints = JsonFileReader.ReadJson<List<float[]>>(@"Content\simpleTrackCheckpoints.json");
            trackManager = TrackManager.Create(trackBounds, checkPoints);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!LivingCars.Any())
                Restart();

            foreach (var car in LivingCars)
            {
                var (speedAction, turningAction) = car.Controller.GetDrivingActions(car, trackManager);
                car.Update(gameTime, speedAction, turningAction);

                if (!car.Corners.Any(corner => trackManager.IsOnTrack(corner)))
                {
                    car.IsAlive = false;
                    continue;
                }

                if (trackManager.HasPassedTargetCheckpoint(car))
                {
                    car.TargetCheckPoint = (car.TargetCheckPoint + 1) % trackManager.NumberOfCheckpoints;

                    if (car.TargetCheckPoint == 1)
                    {
                        if (car.CurrentLapTime < bestLapTimes[car.Controller.Id] ||
                            bestLapTimes[car.Controller.Id] == TimeSpan.Zero)
                        {
                            bestLapTimes[car.Controller.Id] = car.CurrentLapTime;

                            if (car.CurrentLapTime < overallBestLapTime || overallBestLapTime == TimeSpan.Zero)
                                overallBestLapTime = car.CurrentLapTime;
                        }

                        car.AllLapTimes.Add(car.CurrentLapTime);
                        car.CurrentLapTime = TimeSpan.Zero;
                    }
                }
            }

            LivingCars = Cars.Where(car => car.IsAlive).ToList();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(track, Vector2.Zero, Color.White);

            var lastLapTime = TimeSpan.Zero;
            foreach (var car in LivingCars)
            {
                var carSprite = car.IsBraking ? carBrakingTexture : carTexture;
                spriteBatch.Draw(carSprite, car.Position, null, Color.White, car.Direction, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);

                if (car.CurrentLapTime > lastLapTime)
                    lastLapTime = car.CurrentLapTime;
            }

            var currentLapTimeString = $"Current Lap Time: {lastLapTime}";
            spriteBatch.DrawString(lapTimeFont, currentLapTimeString, currentLapTimePosition, Color.Black);
            
            var bestLapTimeString = $"Best Lap Time: {overallBestLapTime}";
            spriteBatch.DrawString(lapTimeFont, bestLapTimeString, bestLapTimePosition, Color.Black);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
        
        private IReadOnlyList<Car> CreateCars()
        {
            var (initialPosition, initialDirection) = trackManager.GetCarStartingPosition(); 
            return Controllers.Select(controller => new Car(initialPosition, initialDirection, controller, carTexture.Width, carTexture.Height)).ToList();
        }
    }
}
