using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    class SequencerTrack : INotePlayer
    {
        private INotePlayer device;

        public SequencerTrack(INotePlayer device)
        {
            this.device = device;
        }

        public void Play(AudioEventInfo eventInfo)
        {
            device.Play(eventInfo);
        }
    }
}
