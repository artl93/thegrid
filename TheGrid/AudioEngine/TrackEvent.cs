using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioLibrary
{
    public struct TrackEvent
    {
        public EventLocation Location;
        public int Note;

        public TrackEvent(EventLocation location, int note)
        {
            Location = location;
            Note = note;
        }
    }
}
