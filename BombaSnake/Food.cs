using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace BombaSnake
{
    internal class Food
    {
        Vector2 _position;

        Rectangle _sourceRect;
        public Rectangle _colRect;

        Texture2D _texture;
        Texture2D _debugPixel;

        public Food(Texture2D texture, Rectangle source, Texture2D debugPixel)
        {
            _texture = texture;
            _debugPixel = debugPixel;

            _sourceRect = source;

            _position = new Vector2(Globals._rng.Next(0, Globals._windowSize.X), Globals._rng.Next(0, Globals._windowSize.Y));

            _position = SnapToGrid(_position, 32);
        }

        public void UpdateFood()
        {
            UpdateCollision();
        }

        public static Vector2 SnapToGrid(Vector2 position, int gridSize)
        {
            //Round the X and Y positions to the nearest multiple of gridSize
            position.X = (float)Math.Round(position.X / gridSize) * gridSize;
            position.Y = (float)Math.Round(position.Y / gridSize) * gridSize;
            return position;
        }

        public void RandomisePosition()
        {
            _position = new Vector2(Globals._rng.Next(0, Globals._windowSize.X), Globals._rng.Next(0, Globals._windowSize.Y));

            _position = SnapToGrid(_position, 32);
        }

        void UpdateCollision()
        {
            _colRect = new Rectangle((int)_position.X + 3, (int)_position.Y + 3, _sourceRect.Width - 6, _sourceRect.Height - 6);
        }

        public void DrawFood()
        {
            //Globals._spriteBatch.Draw(_debugPixel, _colRect, Color.White);
            Globals._spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
        }
    }
}
