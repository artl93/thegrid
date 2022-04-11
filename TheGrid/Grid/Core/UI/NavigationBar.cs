using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TheGrid
{
    class NavigationBar : ControlPage
    {
       
        TexturedButton _drumsButton;
        TexturedButton _synthButton;
#if OPTIONS_PAGE
        TexturedButton _utilButton;
#endif
        TexturedButton _clearButton;
        TexturedButton _bassButton;
        public override string Name
        {
            get { return "Nav"; }
        }

        override public void Initialize(Microsoft.Xna.Framework.Rectangle viewSafeArea, Microsoft.Xna.Framework.Rectangle background, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            const int buttonHeight = 60;
#if OPTIONS_PAGE
            Controls.Add(_utilButton = new NavigationButton(new Vector2(viewSafeArea.Left + 300, viewSafeArea.Y + viewSafeArea.Bottom - buttonHeight), Strings.ButtonUtil, new NavigationEventArgs(Main.Pages.Options)));
#endif
            Controls.Add(_synthButton = new NavigationButton(new Vector2(viewSafeArea.Left + 100, viewSafeArea.Y + viewSafeArea.Bottom - buttonHeight), Grid.Core.Strings.ButtonSynth, new NavigationEventArgs(Main.Pages.Synth)));
            Controls.Add(_bassButton = new NavigationButton(new Vector2(viewSafeArea.Left + 200, viewSafeArea.Y + viewSafeArea.Bottom - buttonHeight), Grid.Core.Strings.ButtonBass, new NavigationEventArgs(Main.Pages.Bass)));
            Controls.Add(_drumsButton = new NavigationButton(new Vector2(viewSafeArea.Left, viewSafeArea.Y + viewSafeArea.Bottom - buttonHeight), Grid.Core.Strings.ButtonDrums, new NavigationEventArgs(Main.Pages.Drums)));
            Controls.Add(_clearButton = new NavigationButton(new Vector2(viewSafeArea.Left + viewSafeArea.Width - 100, viewSafeArea.Y + viewSafeArea.Bottom - buttonHeight), "Clear" /* Strings.ButtonClear */, null));

            _drumsButton.AfterClicked += new EventHandler(OnClicked);
            _bassButton.AfterClicked += new EventHandler(OnClicked);
            _synthButton.AfterClicked += new EventHandler(OnClicked);
#if OPTIONS_PAGE
            _utilButton.AfterClicked += new EventHandler(OnClicked);
#endif
            _clearButton.AfterClicked += new EventHandler(_clearButton_AfterClicked);
        }


        void _clearButton_AfterClicked(object sender, EventArgs e)
        {
            FireClearRequest();
        }

        void FireClearRequest()
        {
            var evt = ClearRequest;
            if (evt != null)
                evt(this, null);
        }

        public void SetActivePage(Main.Pages page)
        {
            foreach (NavigationButton control in Controls)
                control.IsActive = false;
            // TODO this is a total CHEAT based on the order in Initalize
            ((NavigationButton)Controls[(int)page]).IsActive = true;
        }

        void OnClicked(object sender, EventArgs e)
        {
            FireRequestNavigation(((NavigationEventArgs)e).Page);
        }

        void FireRequestNavigation(Main.Pages page)
        {
            var evt = NavigationRequest;
            if (evt != null)
                evt(this, new NavigationEventArgs(page));

        }

        public event EventHandler<NavigationEventArgs> NavigationRequest;
        public event EventHandler ClearRequest;

    }

    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(Main.Pages page)
        {
            Page = page;
        }

        public Main.Pages Page { get; private set; }
    }

}
