using System;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheGrid
{
    abstract class GamePage
    {

        public abstract void Draw(SpriteBatch batch);
        public abstract void Initialize(Rectangle viewSafeArea, Rectangle background, ContentManager Content);
        public abstract void Update(GameTime time);
        public abstract void HandleInput(Main.State _currentState, Main.State _previousState);

        abstract public string Name { get; }

    }
}

