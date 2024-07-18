using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BombaSnake
{
    internal class Bomb
    {
        Vector2 _position;

        Rectangle _sourceRect;
        public Rectangle _colRect;

        Texture2D _texture;
        Texture2D _debugPixel;

        public Bomb(Texture2D texture, Rectangle source, Texture2D debugPixel)
        {
            _texture = texture;
            _debugPixel = debugPixel;

            _sourceRect = source;

            //Position the food at a random location in the window.
            _position = new Vector2(Globals._rng.Next(0, Globals._windowSize.X), Globals._rng.Next(0, Globals._windowSize.Y));

            //Make sure the food position is on the grid.
            _position = Globals.SnapToGrid(_position, 32);
        }

        public void UpdateBomb()
        {
            UpdateCollision();
        }

        public void RandomisePosition()
        {
            //Randomise the food position within the window.
            _position = new Vector2(Globals._rng.Next(0, Globals._windowSize.X - _sourceRect.Width),
                Globals._rng.Next(0, Globals._windowSize.Y - _sourceRect.Height));

            //Snap the food position to the grid.
            _position = Globals.SnapToGrid(_position, 32);
        }

        void UpdateCollision()
        {
            _colRect = new Rectangle((int)_position.X + 3, (int)_position.Y + 3, _sourceRect.Width - 6, _sourceRect.Height - 6);
        }

        public void DrawBomb()
        {
            //Globals._spriteBatch.Draw(_debugPixel, _colRect, Color.White);
            Globals._spriteBatch.Draw(_texture, _position, _sourceRect, Color.White);
        }
    }
}
