using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace TheGrid
{

    class MusicalButton : TexturedButton
    {


        public MusicalButton(Vector2 position, Color color)
            : base(position, color)
        {
            Height = GridPage.GridButtonHeight;
            Width = 50;
            // _soundSet = soundSet;
            _animation.Initialize(_animationTexture, Position, Width, Height, 3, 250 / 3, Color, 1.0f, false);
            _radiateAnimation.Initialize(_radiateAnimationTexture, new Vector2(Position.X - Width, Position.Y - Height), Width * 3, Height * 3, 5, 250 / 5, Color, 1.0f, false);
        }

        override public void Update(GameTime time)
        {
            _animation.Update(time);
            _radiateAnimation.Update(time);
            base.Update(time);
        }

        override public void Draw(SpriteBatch spriteBatch)
        {
            if (_animation.Active && !IsPressed)
                _animation.Draw(spriteBatch);
            else
                base.Draw(spriteBatch);
            if (_radiateAnimation.Active)
                _radiateAnimation.Draw(spriteBatch);


        }

        protected override ButtonTexture GetButtonTexture()
        {
            if (IsOnBeat && !IsActive)
                return ButtonTexture.Dim;
            return base.GetButtonTexture();
        }

        public bool IsOnBeat
        {
            get;
            set;
        }

        private static bool _loadedStaticVars = false;

        private static Texture2D _animationTexture;
        private static Texture2D _radiateAnimationTexture;

        private Animation _animation = new Animation();
        private Animation _radiateAnimation = new Animation();
        private static Texture2D _buttonTexture;

        public void Play()
        {

            _animation.Start();
            _radiateAnimation.Start();
        }

        protected override Texture2D Texture
        {
            get
            {
                return _buttonTexture;
            }
        }

        internal static void InitStaticContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            if (!_loadedStaticVars)
            {
                _loadedStaticVars = true;
                _buttonTexture = Content.Load<Texture2D>("Buttons\\ButtonStates");


                _animationTexture = Content.Load<Texture2D>("Buttons\\ButtonLitAnimation");
                _radiateAnimationTexture = Content.Load<Texture2D>("Buttons\\ButtonRadiateAnimation");
            }
        }

        internal static void UnloadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            _buttonTexture = null;
           _animationTexture = null;
            _radiateAnimationTexture = null;
        }

        protected override void OnClicked(int x, int y)
        {
            IsActive = !IsActive;
            base.OnClicked(x, y);
        }
    }
}
