using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    interface INotePlayer
    {
        void Play(AudioEventInfo eventInfo);
    }
}
