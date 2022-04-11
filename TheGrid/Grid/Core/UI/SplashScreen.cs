using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGrid.UI
{
    class SplashScreen : Microsoft.Xna.Framework.DrawableGameComponent
    {

        private Texture2D texture;
        private int fadeTime;
        private Color _color = Color.White;
        private bool _play;


        public SplashScreen(Main game, Texture2D texture, int fadeTime)
            : base(game)
        {
            this.texture = texture;
            this.fadeTime = fadeTime;
        }

        public void StartFade()
        {
            _play = true;
        }

        static public SplashScreen Create(Main game, Texture2D texture, int fadeTime)
        {
            return new SplashScreen(game, texture, fadeTime);
        }

        public override void Update(GameTime gameTime)
        {
            int dimAmt = (int)((gameTime.ElapsedGameTime.TotalMilliseconds / fadeTime) * byte.MaxValue);
            if (this.Enabled && _play)
            {
                _color.A = (byte) Math.Max(_color.A - dimAmt, 0);
                _color.B = _color.G = _color.R = _color.A;
                if (_color.A == 0)
                    this.Enabled = false;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var game = this.Game as Main;
            game.SpriteBatch.Draw(Utilities.LoadingScreen, GraphicsDevice.Viewport.Bounds, _color);

            base.Draw(gameTime);
        }

    }
}
