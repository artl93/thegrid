using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGrid
{
    class SimpleTextButton : TexturedButton
    {
        public string Text { get; private set; }

        public SimpleTextButton(Vector2 position, string text)
            : base(position, Color.White)
        {
            Text = text;
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Utilities.DrawCenteredText(spriteBatch, Utilities.SpriteFont, Text, Width, Height, Position);
        }

    }
}
