using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace BombaSnake
{
    internal static class Globals
    {
        public static float _timer = 0f;
        
        public static float _stepTimer = 0.1f;

        public static Random _rng = new Random();

        public static Point _windowSize = new Point(800, 800);

        public static SpriteBatch _spriteBatch;

        public static Vector2 SnapToGrid(Vector2 position, int gridSize)
        {
            //Round the X and Y positions to the nearest multiple of gridSize.
            position.X = (float)Math.Round(position.X / gridSize) * gridSize;
            position.Y = (float)Math.Round(position.Y / gridSize) * gridSize;
            return position;
        }
    }
}
