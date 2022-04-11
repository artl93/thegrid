using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TheGrid
{
    abstract class ControlPage : GamePage
    {
        public List<Control> Controls = new List<Control>();

        private Control FindControl(int x, int y, Control button)
        {
            //search control list
            foreach (var buttonT in Controls)
            {
                if (buttonT.HitTest(x, y))
                {
                    button = buttonT;
                    break;
                }
            }
            return button;
        }
        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            foreach (var control in Controls)
            {
                control.Draw(batch);
            }
            
        }

        public override void Initialize(Microsoft.Xna.Framework.Rectangle viewSafeArea, Microsoft.Xna.Framework.Rectangle background, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime time)
        {
            foreach (var control in Controls)
                control.Update(time);
        }
        public override void HandleInput(Main.State _currentState, Main.State _previousState)
        {
            foreach (var control in Controls)
            {
                if (control.HitTest(_currentState.MouseState.X, _currentState.MouseState.Y))
                    control.HandleInput(_currentState, _previousState);
            }
            
        }
    }
}
