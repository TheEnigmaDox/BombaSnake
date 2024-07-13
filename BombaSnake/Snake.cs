using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace BombaSnake
{
    internal class Snake
    {
        int _gridSize = 32;

        Vector2 _position;

        Vector2 _direction = new Vector2(0, 1);

        Rectangle _sourceRect;
        public Rectangle _colRect;

        Texture2D _texture;
        Texture2D _debugPixel;

        public Snake(Texture2D texture, Rectangle sourceRect, Texture2D debugPixel) 
        {
            _texture = texture;
            _debugPixel = debugPixel;

            _sourceRect = sourceRect;

            _position = new Vector2(Globals._windowSize.X / 2 - _sourceRect.Width / 2,
                 Globals._windowSize.Y / 2 - _sourceRect.Height / 2);

            _position = SnapToGrid(_position, _gridSize);
        }

        public void UpdateSnake(GameTime gameTime, KeyboardState keyboardState, List<Snake> parts)
        {
            //Update the collision for all parts of the snake.
            UpdateCollision(parts);

            //Check the inputs made by the player.
            CheckInputs(keyboardState);

            for(int i = 0; i < parts.Count - 1; i++)
            {
                for(int j = 0; j < parts.Count - 1; j++)
                {
                    if (parts[i] != parts[j] && parts[i]._colRect.Intersects(parts[j]._colRect))
                    {
                        Debug.WriteLine("I collided");
                    }
                }
            }
        }

        public static Vector2 SnapToGrid(Vector2 position, int gridSize)
        {
            //Round the X and Y positions to the nearest multiple of gridSize.
            position.X = (float)Math.Round(position.X / gridSize) * gridSize;
            position.Y = (float)Math.Round(position.Y / gridSize) * gridSize;
            return position;
        }

        //Check the inputs and only change the direction if a viable direction is chosen.
        void CheckInputs(KeyboardState keyboardState) 
        {
            //If W is pressed and direction is not equal to down...
            if (keyboardState.IsKeyDown(Keys.W) && _direction != new Vector2(0, 1))
            {
                //...Set direction to up.
                _direction = new Vector2(0, -1);
            }
            //If S is pressed and direction is not equal to up...
            else if (keyboardState.IsKeyDown(Keys.S) && _direction != new Vector2(0, -1))
            {
                //...Set the direction to down.
                _direction = new Vector2(0, 1);
            }
            //If A is pressed and dirtection is not equal to right...
            else if (keyboardState.IsKeyDown(Keys.A) && _direction != new Vector2(1, 0))
            {
                //...Set direction to left.
                _direction = new Vector2(-1, 0);
            }
            //If D is pressed and direction is not equal to left...
            else if (keyboardState.IsKeyDown(Keys.D) && _direction != new Vector2(-1, 0))
            {
                //...Set direction to right.
                _direction = new Vector2(1, 0);
            }
        }

        //Function to update the snakes position.
        public void UpdateSnakePosition(GameTime gameTime, List<Snake> parts, bool addPart)
        {
            //Create a position for the head to move to.
            Vector2 newPosition = _position + (_direction * _gridSize);

            if(addPart)
            {
                parts.Add(new Snake(_texture, new Rectangle(32, 0, 32, 32), _debugPixel));
                Game1.addPart = false;
            }

            //Loop through the list of parts backwards...
            for (int i = parts.Count - 1; i > 0; i--)
            {
                //...Set each position to the one before it.
                parts[i]._position = parts[i - 1]._position;
            }

            //Move the head to the new position.
            parts[0]._position = newPosition;
        }

        //Function to update the snakes collision boxes.
        void UpdateCollision(List<Snake> parts)
        {
            foreach (Snake eachPart in parts)
            {
                eachPart._colRect = new Rectangle((int)eachPart._position.X + 4,
                    (int)eachPart._position.Y + 4,
                    eachPart._sourceRect.Width - 8,
                    eachPart._sourceRect.Height - 8);
            }
        }

        public void DrawSnake(List<Snake> parts)
        {
            Globals._spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
            //Globals._spriteBatch.Draw(_debugPixel, _colRect, Color.White);
        }
    }
}
