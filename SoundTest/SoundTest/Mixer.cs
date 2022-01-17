using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    class Mixer : AudioComponentContainer
    {
        protected Mixer() { }
        static public Mixer Create() { return new Mixer();  }
    }
}
