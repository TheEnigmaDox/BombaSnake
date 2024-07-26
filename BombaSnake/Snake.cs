using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace BombaSnake
{
    internal class Snake
    {
        int _gridSize = 32;

        bool _isPart;
        bool _isBomb;

        float _bombTimer = 0f;
        float _maxBombTimer = 1f;

        Vector2 _position;

        Vector2 _direction = new Vector2(0, 1);

        Rectangle _sourceRect;
        public Rectangle _colRect;

        Color _bombColour;
        Color _partColour;

        Texture2D _texture;
        Texture2D _debugPixel;

        public Snake(Texture2D texture, Rectangle sourceRect, Texture2D debugPixel, Color partColour, bool isPart, bool isBomb) 
        {
            _texture = texture;
            _debugPixel = debugPixel;

            _sourceRect = sourceRect;

            _partColour = partColour;

            _bombColour = Color.Black;

            _isPart = isPart;
            _isBomb = isBomb;

            _position = new Vector2(Globals._windowSize.X / 2 - _sourceRect.Width / 2,
                 Globals._windowSize.Y / 2 - _sourceRect.Height / 2);

            _position = Globals.SnapToGrid(_position, _gridSize);
        }

        public void SetUpBody(List<Snake> parts)
        {
            //Create a position for the head to move to.
            Vector2 newPosition = _position + (_direction * _gridSize);

            //Loop through the list of parts backwards...
            for (int i = parts.Count - 1; i > 0; i--)
            {
                //...Set each position to the one before it.
                parts[i]._position = parts[i - 1]._position;
            }

            //Move the head to the new position.
            parts[0]._position = newPosition;
        }

        public void UpdateSnake(GameTime gameTime, KeyboardState keyboardState, List<Snake> parts)
        {
            //Update the collision for all parts of the snake.
            UpdateCollision(parts);

            //Check the inputs made by the player.
            CheckInputs(keyboardState);
            CheckPosition();

            for (int i = 0; i < parts.Count - 1; i++)
            {
                for (int j = 0; j < parts.Count - 1; j++)
                {
                    if (parts[i] != parts[j] && parts[i] == parts[0] && parts[i]._colRect.Intersects(parts[j]._colRect))
                    {
                        Game1.gameState = Game1.GameState.GameOver;
                        Debug.WriteLine("I collided");
                    }
                }
            }

            for(int i = 0; i < parts.Count; i++)
            {
                if (parts[i]._isBomb)
                {
                    int startIndex  = i - 2;

                    if (keyboardState.IsKeyDown(Keys.Space))
                    {
                        parts.RemoveRange(startIndex, parts.Count - startIndex);

                        //Debug.WriteLine("Bomb went Boom");
                    }
                }
            }

            Debug.WriteLine(_bombTimer);
        }

        void CheckPosition()
        {
            if (_position.X > Globals._windowSize.X)
            {
                _position.X = 0;
            }
            else if (_position.X < 0)
            {
                _position.X = Globals._windowSize.X - _sourceRect.Width;
            }

            if (_position.Y > Globals._windowSize.Y)
            {
                _position.Y = 0;
            }
            else if (_position.Y < 0)
            {
                _position.Y = Globals._windowSize.Y - _sourceRect.Height;
            }
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
        public void UpdateSnakePosition(GameTime gameTime, List<Snake> parts, bool addPart, bool addBomb)
        {
            //Create a position for the head to move to.
            Vector2 newPosition = _position + (_direction * _gridSize);

            if(addPart)
            {
                parts.Add(new Snake(_texture, new Rectangle(32, 0, 32, 32), _debugPixel, Color.White, true, false));
                Game1.addPart = false;
            }
            else if(addBomb)
            {
                parts.Add(new Snake(_texture, new Rectangle(32, 32, 32, 32), _debugPixel, Color.White, false, true));
                //Game1.addPart = false;
                Game1.addBomb = false;
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

        public void DrawSnake(List<Snake> parts, GameTime gameTime)
        {
            

            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i]._isPart)
                {
                    Globals._spriteBatch.Draw(_texture, _position, _sourceRect, _partColour);
                }
                else if (parts[i]._isBomb)
                {
                    if (parts[i]._isBomb && _bombColour == Color.Black && _bombTimer <= 0)
                    {
                        _bombColour = Color.Red;
                        _bombTimer = _maxBombTimer;
                    }
                    else if (parts[i]._isBomb && _bombColour == Color.Red && _bombTimer <= 0)
                    {
                        _bombColour = Color.Black;
                        _bombTimer = _maxBombTimer;
                    }
                    else
                    {
                        _bombTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    Globals._spriteBatch.Draw(_texture, _position, _sourceRect, _bombColour);
                }
            }

            //Globals._spriteBatch.Draw(_texture, _position, _sourceRect, _partColour);
            //Globals._spriteBatch.Draw(_debugPixel, _colRect, Color.White);
        }
    }
}
