using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGrid
{
    class NavigationButton : TexturedButton
    {
        private static bool _loadedStaticVars;
        private static Texture2D _buttonTexture;
        private static SpriteFont _spriteFont;
        private string _text;
        private NavigationEventArgs _args;

        protected override Microsoft.Xna.Framework.Graphics.Texture2D Texture
        {
            get
            {
                return _buttonTexture;
            }
        }

        public NavigationButton(Vector2 position, string text, NavigationEventArgs args)
            : base(position, Color.White)
        {
            Width = 100;
            Height = 60;
            _text = text;
            _args = args;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Utilities.DrawCenteredText(spriteBatch, _spriteFont, _text, Width, Height, Position);
        }

        internal static void InitStaticContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            if (!_loadedStaticVars)
            {
                _loadedStaticVars = true;

                _buttonTexture = Content.Load<Texture2D>("Buttons\\NavButtonStates");
                _spriteFont = Utilities.SpriteFont;

            }
        }

        internal static void UnloadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            _buttonTexture = null;
            _spriteFont = null;
        }

        protected override void OnClicked(int x, int y)
        {
            FireAfterClicked(_args);
        }

    }

}
