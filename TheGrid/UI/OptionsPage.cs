using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AudioLibrary;


namespace TheGrid
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    internal class OptionsPage : ControlPage
    {

        public override void Initialize(Rectangle viewSafeArea, Rectangle background, ContentManager Content)
        {
#if WINDOWS
            foreach (var key in Enum.GetValues(typeof(MusicKey)))
            {
                var name = Enum.GetName(typeof(MusicKey), key);
                Controls.Add(new SimpleTextButton(new Vector2(viewSafeArea.Center.X, viewSafeArea.Center.Y), "Test"));
            }
#endif
            base.Initialize(viewSafeArea, background, Content);
        }

        public override string Name
        {
            get { return "Options"; }
        }
 
    }
}
