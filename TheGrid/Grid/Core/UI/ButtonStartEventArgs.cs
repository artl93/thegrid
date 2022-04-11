using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGrid.UI
{
    class ButtonStartEventArgs : EventArgs
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public ButtonStartEventArgs(int row, int column)
        {
            // TODO: Complete member initialization
            this.Row = row;
            this.Column = column;
        }
    }
}
