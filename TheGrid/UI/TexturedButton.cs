using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace TheGrid
{


    enum ButtonTexture
    {
        Unlit,
        UnlitPressed,
        Lit,
        LitPressed,
        Dim
    }

    class TexturedButton : Control
    {
        virtual protected Texture2D Texture
        {
            get
            {
                return null;
            }

        }
        protected Color Color;

        public TexturedButton(Vector2 position, Color color)
            : base(position, 50, GridPage.GridButtonHeight)
        {
            this.IsActive = false;
            this.Color = color;
        }

        override public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            ButtonTexture textureIndex = GetButtonTexture();

            spriteBatch.Draw(Texture, Position, new Rectangle(0 + Width * (int)textureIndex, 0, Width, Height), Color);
            IsPressed = false;
        }

        virtual protected ButtonTexture GetButtonTexture()
        {
            if (IsActive)
                return IsPressed ? ButtonTexture.LitPressed : ButtonTexture.Lit;
            else
                return IsPressed ? ButtonTexture.UnlitPressed : ButtonTexture.Unlit;
        }


        public bool IsActive
        {
            get;
            set;
        }
    }
}
