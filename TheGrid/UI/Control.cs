using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TheGrid
{
    internal class Control
    {
        protected Vector2 Position;
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public Control(Vector2 position, int width, int height) 
        {
            Position = position;
            Width = width;
            Height = height;
        }
        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
        }

        public virtual void Update(Microsoft.Xna.Framework.GameTime time) { }

        public bool HitTest(int x, int y)
        {
            return ((x > Position.X && x < Position.X + Width) &&
                (y > Position.Y && y < Position.Y + Height));
        }


        protected void FireAfterClicked(EventArgs args)
        {
            var evt = AfterClicked;
            if (evt != null)
            {
                evt(this, args);
            }
        }

        public event EventHandler AfterClicked;


        virtual internal void HandleInput(Main.State currentState, Main.State previousState)
        {
            if (currentState.MouseState.LeftButton != ButtonState.Pressed && previousState.MouseState.LeftButton == ButtonState.Pressed)
                OnClicked(currentState.MouseState.X, currentState.MouseState.Y);
            if (currentState.MouseState.LeftButton == ButtonState.Pressed)
                OnPressed(currentState.MouseState.X, currentState.MouseState.Y);
        }


        virtual protected void OnPressed(int x, int y)
        {
            IsPressed = true;
        }

        virtual protected void OnClicked(int x, int y)
        {
            // IsActive = !IsActive;
            FireAfterClicked(null);
        }


        public bool IsPressed
        {
            get;
            set;
        }
    }
}
