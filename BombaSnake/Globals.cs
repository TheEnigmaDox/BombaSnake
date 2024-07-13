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
    }
}
