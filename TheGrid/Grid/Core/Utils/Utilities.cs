using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGrid
{

    static class Utilities
    {
        public static void DrawCenteredText(SpriteBatch spriteBatch, SpriteFont font, string text, int width, int height, Vector2 position)
        {
            var textRect = font.MeasureString(text);
            position.X = position.X + ((width - textRect.X) / 2);
            // position.Y = position.Y + ((height - textRect.Y) / 2) + 2;
            position.Y = position.Y + ((height - textRect.Y));
            spriteBatch.DrawString(font, text, position, Color.AntiqueWhite);
        }

        public static SpriteFont SpriteFont
        {
            get;
            private set;
        }

        public static void LoadGlobalContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            SpriteFont = Content.Load<SpriteFont>("SpriteFont");
            LoadingScreen = Content.Load<Texture2D>("LoadingScreen");
        }


        public static Texture2D LoadingScreen { get; private set; }
    }
}
