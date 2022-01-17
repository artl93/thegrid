using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using TheGrid.UI;

namespace TheGrid
{
    abstract class ButtonSelectBehavior
    {
        public static ButtonSelectBehavior Paint = new PaintSelectionBehavior();
        public static ButtonSelectBehavior OnlyOne = new OnlyOneSelectionBehavior();

        abstract public void HandleInput(GridPage gridPage, TrackGridFacade model, Main.State currentState, Main.State previousState);
    }

    class PaintSelectionBehavior : ButtonSelectBehavior
    {
        bool? _isActiveSet = null;
        public override void HandleInput(GridPage gridPage, TrackGridFacade model, Main.State currentState, Main.State previousState)
        {
            if (currentState.MouseState.LeftButton != ButtonState.Pressed)
                _isActiveSet = null;
            var x = currentState.MouseState.X;
            var y = currentState.MouseState.Y;
            var coord = gridPage.GetButtonLocation(x, y);
            if (coord.HasValue)
            {
                if ((!_isActiveSet.HasValue) &&
                    (currentState.MouseState.LeftButton == ButtonState.Pressed && previousState.MouseState.LeftButton != ButtonState.Pressed))
                {
                    _isActiveSet = !model.GetButtonState(coord.Value.Y, coord.Value.X);
                }
                else if (_isActiveSet.HasValue)
                    model.SetButtonState(coord.Value.Y, coord.Value.X, _isActiveSet.Value);
            }
        }
    }



    class OnlyOneSelectionBehavior : ButtonSelectBehavior
    {

        bool? _isActiveSet = null;
        public override void HandleInput(GridPage gridPage, TrackGridFacade model, Main.State currentState, Main.State previousState)
        {
            if (currentState.MouseState.LeftButton != ButtonState.Pressed)
                _isActiveSet = null;
            var x = currentState.MouseState.X;
            var y = currentState.MouseState.Y;
            var coord = gridPage.GetButtonLocation(x, y);
            if (coord.HasValue)
            {
                if ((!_isActiveSet.HasValue) &&
                    (currentState.MouseState.LeftButton == ButtonState.Pressed && previousState.MouseState.LeftButton != ButtonState.Pressed))
                    _isActiveSet = !model.GetButtonState(coord.Value.Y, coord.Value.X);
                else if (_isActiveSet != null)
                {
                    if (_isActiveSet.Value)
                        gridPage.ClearColumn(x);
                    model.SetButtonState(coord.Value.Y, coord.Value.X, _isActiveSet.Value);
                }

            }
        }
    }


}
