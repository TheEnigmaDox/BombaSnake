using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BombaSnake
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

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
            parts.Add(new Snake(texture, new Rectangle(32, 0, 32, 32), debugpixel));
            parts.Add(new Snake(texture, new Rectangle(32, 0, 32, 32), debugpixel));

            food = new Food(texture, new Rectangle(0, 32, 32, 32), debugpixel);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Store the state of the keyboard.
            keyboardState = Keyboard.GetState();

            // TODO: Add your update logic here

            foreach(Snake eachPart in parts)
            {
                eachPart.UpdateSnake(gameTime, keyboardState, parts);
            }

            food.UpdateFood();

            if (parts[0]._colRect.Intersects(food._colRect))
            {
                food.RandomisePosition();
                parts.Add(new Snake(texture, new Rectangle(32, 0, 32, 32), debugpixel));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            Globals._spriteBatch.Begin();

            food.DrawFood();

            foreach (Snake eachPart in parts)
            {
                eachPart.DrawSnake(parts);
            }

            Globals._spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}