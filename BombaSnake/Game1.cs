using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BombaSnake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        //A boolean accessable to all classes to decide whether to add a body part or not.
        public static bool addPart = false;

        //A list to hold all the parts of the snake.
        List<Snake> parts = new List<Snake>();

        //The main snake Texture.
        Texture2D texture;
        //A pixel texture to debug rectangles.
        Texture2D debugpixel;

        //A variable to store the keyboard state.
        KeyboardState keyboardState;

        Food food;

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
            debugpixel = Content.Load<Texture2D>("Textures/DebugPixel");

            //Add the head to the parts list.
            parts.Add(new Snake(texture, new Rectangle(0, 0, 32, 32), debugpixel));
            //Add two body parts to the snake.
            //parts.Add(new Snake(texture, new Rectangle(32, 0, 32, 32), debugpixel));
            //parts.Add(new Snake(texture, new Rectangle(32, 0, 32, 32), debugpixel));

            food = new Food(texture, new Rectangle(0, 32, 32, 32), debugpixel);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Store the state of the keyboard.
            keyboardState = Keyboard.GetState();

            // TODO: Add your update logic here

            //For each part of the snake...
            foreach(Snake eachPart in parts)
            {
                //...Update that part of the snake.
                eachPart.UpdateSnake(gameTime, keyboardState, parts);
            }

            //Update the food.
            food.UpdateFood();

            //If the head of the snake collides with the food...
            if (parts[0]._colRect.Intersects(food._colRect))
            {
                //Call the Randomise position function inside the food class.
                food.RandomisePosition();
                //Set the add part boolean to true.
                addPart = true;
            }

            //If the timer is less than or equal to zero...
            if (Globals._timer <= 0)
            {
                //Update the head positioin of the snake.
                //Which in turn updates the other parts of the snake.
                parts[0].UpdateSnakePosition(gameTime, parts, addPart);
                Globals._timer = Globals._stepTimer;
            }
            //Or else...
            else
            {
                //Decrement the timer by the elapsed game time.
                Globals._timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            Globals._spriteBatch.Begin();

            //Draw the food to the screen.
            food.DrawFood();

            //For each part of the snake...
            foreach (Snake eachPart in parts)
            {
                //Draw each part of the snake...
                eachPart.DrawSnake(parts);
            }

            Globals._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}