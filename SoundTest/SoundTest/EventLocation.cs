using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    struct EventLocation : IComparable<EventLocation>, IEqualityComparer<EventLocation>, IEquatable<EventLocation>
    {
        public int Measure;
        public int Beat;
        public double Div;

        internal void AddBeats(double spanBeats, int beatsPerMeasure)
        {
            Div += spanBeats;
            if (Div >= 1)
            {
                var wholeBeats = (int)Div;
                Beat += wholeBeats;
                Div -= wholeBeats;
            }
            if (Beat >= beatsPerMeasure)
            {
                var newMeasures = Beat / 4;
                Beat = Beat % beatsPerMeasure;
                Measure += newMeasures;
            }

        }

        public static double GetBeatsSpan(EventLocation a, EventLocation b, int beatsPerMeasure)
        {
            var measBeats = (a.Measure - b.Measure) * beatsPerMeasure;
            var beats = a.Beat - b.Beat;
            var div = (a.Div - b.Div);

            return beats + div + measBeats;
        }

        public int CompareTo(EventLocation other)
        {
            if (this.Measure != other.Measure)
                return this.Measure > other.Measure ? 1 : -1;
            if (this.Beat != other.Beat)
                return this.Beat > other.Beat ? 1 : -1;
            if (this.Div != other.Div)
                return this.Div > other.Div ? 1 : -1;
            return 0;
        }
        
        public static bool operator >(EventLocation a, EventLocation b)
        {
            return (a.CompareTo(b) == 1);
        }
        public static bool operator <(EventLocation a, EventLocation b)
        {
            return (a.CompareTo(b) == -1);
        }
        public static bool operator >=(EventLocation a, EventLocation b)
        {
            var result = a.CompareTo(b);
            return (result == 1 || result == 0);
        }
        public static bool operator <=(EventLocation a, EventLocation b)
        {
            var result = a.CompareTo(b);
            return (result == -1 || result == 0);
        }

        public bool Equals(EventLocation x, EventLocation y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public int GetHashCode(EventLocation obj)
        {
            var hash = obj.Measure << 3 +  obj.Beat << 2 + (int)obj.Div;
            return hash;
        }

        public bool Equals(EventLocation other)
        {
            return this.GetHashCode() == other.GetHashCode();
        }
    }
}
