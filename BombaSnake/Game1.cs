using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BombaSnake
{
    public class Game1 : Game
    {
        public enum GameState
        {
            Title,
            Game,
            GameOver
        }

        private GraphicsDeviceManager _graphics;

        //The starting game state.
        public static GameState gameState = GameState.Game;

        //A boolean accessable to all classes to decide whether to add a body part or not.
        public static bool addPart = false;

        //A boolean accessable to all classes to decide whether to add a body part or not.
        public static bool addBomb = false;

        public static bool hasBomb = false;

        //A list to hold all the parts of the snake.
        List<Snake> parts = new List<Snake>();

        //The main snake Texture.
        Texture2D texture;
        //A pixel texture to debug rectangles.
        Texture2D debugPixel;

        //A variable to store the keyboard state.
        KeyboardState keyboardState;

        Food food;

        Bomb bomb;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Globals._spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: Add your initialization logic here

            //Set the game window size.
            _graphics.PreferredBackBufferWidth = Globals._windowSize.X;
            _graphics.PreferredBackBufferHeight = Globals._windowSize.Y;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

            //Load texture sheet.
            texture = Content.Load<Texture2D>("Textures/Snake");

            //Load the debug texture.
            debugPixel = Content.Load<Texture2D>("Textures/DebugPixel");

            //Add the head to the parts list.
            parts.Add(new Snake(texture, new Rectangle(0, 0, 32, 32), debugPixel, Color.Lavender, true, false));
            //Add two body parts to the snake.
            parts.Add(new Snake(texture, new Rectangle(32, 0, 32, 32), debugPixel, Color.Lavender, true, false));
            parts.Add(new Snake(texture, new Rectangle(32, 0, 32, 32), debugPixel, Color.Lavender, true, false));

            parts[0].SetUpBody(parts);

            food = new Food(texture, new Rectangle(0, 32, 32, 32), debugPixel);

            bomb = new Bomb(texture, new Rectangle(32, 32, 32, 32), debugPixel);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameState)
            {
                case GameState.Title:
                    UpdateTitle();
                    break;
                case GameState.Game:
                    UpdateGame(gameTime, keyboardState);
                    break;
                case GameState.GameOver:
                    UpdateGameOver();
                    break;
            }

            //Store the state of the keyboard.
            keyboardState = Keyboard.GetState();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        void UpdateTitle()
        {

        }

        void UpdateGame(GameTime gameTime, KeyboardState keyboardState)
        {
            for(int i = 0; i < parts.Count; i++)
            {
                parts[i].UpdateSnake(gameTime, keyboardState, parts);
            }

            //Update the food.
            food.UpdateFood();

            foreach(Snake eachPart in parts)
            {
                if (eachPart._isBomb)
                {
                    hasBomb = true;
                    bomb._UpdateColRect = false;
                }
                else if(!eachPart._isBomb && parts.Count >= 5)
                {
                    hasBomb = false;
                    bomb._UpdateColRect = true;
                }
            }

            bomb.UpdateBomb();

            foreach (Snake eachPart in parts)
            {
                if (eachPart == parts[0] && parts[0]._colRect.Intersects(food._colRect))
                {
                    //Call the Randomise position function inside the food class.
                    food.RandomisePosition();
                    //Set the add part boolean to true.
                    addPart = true;
                    addBomb = false;
                }
                else if (eachPart != parts[0] && eachPart._colRect.Intersects(food._colRect))
                {
                    //Call the Randomise position function inside the food class.
                    food.RandomisePosition();
                }

                if (bomb._colRect.HasValue)
                {
                    if (eachPart == parts[0] && parts[0]._colRect.Intersects(bomb._colRect.Value))
                    {
                        addPart = false;
                        addBomb = true;
                        bomb._UpdateColRect = false;
                    } 
                }
            }

            //If the timer is less than or equal to zero...
            if (Globals._timer <= 0)
            {
                //Update the head positioin of the snake.
                //Which in turn updates the other parts of the snake.
                parts[0].UpdateSnakePosition(gameTime, parts, addPart, addBomb);
                Globals._timer = Globals._stepTimer;
            }
            //Or else...
            else
            {
                //Decrement the timer by the elapsed game time.
                Globals._timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        void UpdateGameOver()
        {

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            // TODO: Add your drawing code here

            Globals._spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Title:
                    DrawTitle();
                    break;
                case GameState.Game:
                    DrawGame(gameTime);
                    break;
                case GameState.GameOver:
                    DrawGameOver();
                    break;
            }

            Globals._spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawTitle()
        {

        }

        void DrawGame(GameTime gameTime)
        {
            //Draw the food to the screen.
            food.DrawFood();

            foreach(Snake eachPart in parts)
            {
                if (eachPart._isBomb)
                {

                }
            }

            if (parts.Count >= 5 && !hasBomb)
            {
                
            }

            //For each part of the snake...
            foreach (Snake eachPart in parts)
            {
                //Draw each part of the snake...
                eachPart.DrawSnake(parts, gameTime);
            }
        }

        void DrawGameOver()
        {
            GraphicsDevice.Clear(Color.Blue);
        }
    }
}