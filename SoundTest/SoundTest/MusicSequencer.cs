using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    struct TrackEvent
    {
        public EventLocation Location;
        public SequencerTrack Track; 
    }

    class MusicSequencer
    {
        const int BeatsPerMeasure = 4;
        const int TotalMeasures = 2;

        EventLocation _start = new EventLocation();
        EventLocation _end = new EventLocation() { Measure = TotalMeasures };

        Dictionary<string, SequencerTrack> _tracks = new Dictionary<string, SequencerTrack>();
        List<TrackEvent> _events = new List<TrackEvent>();
        private short _channelCount;

        readonly public bool Looped = true;
        double _samplesPerBeat;
        EventLocation _eventPosition = new EventLocation();

        public EventLocation CurrentPosition
        {
            get
            {
                return _eventPosition;
            }
        }

        void SetTempo(double tempo)
        {
            Tempo = tempo;
            _samplesPerBeat = (_sampleRate * 60) / tempo;
        }

        public MusicSequencer(int sampleRate, double tempo)
        {
            _sampleRate = sampleRate;
            SetTempo(tempo);
        }

        internal void AddTrack(string name, SequencerTrack track)
        {
            _tracks.Add(name, track);
            _channelCount = (short)_tracks.Count;
        }

        internal void AddEvent(SequencerTrack track, EventLocation location)
        {
            int index = -1;

            foreach(var evt in _events)
            {
                if (evt.Location > location)
                    break;
                index++;
            }
            _events.Insert(index + 1, new TrackEvent(){Location = location, Track = track});
        }

        internal void UpdatePosition(int frame, int samples)
        {
            var startEventPosition = _eventPosition;

            UpdateLocation(samples);

            if (_eventPosition >= _end)
            {
                PlayEvents(frame, startEventPosition, _end, 0);
                // TODO: Conditional for loop
                // get the offset from the start of the frame to the end. The end now becomes the beginning, and we 
                // start playing again from there. 
                var frameSampleOffset = (int)Math.Round(EventLocation.GetBeatsSpan(_end, startEventPosition, BeatsPerMeasure) * _samplesPerBeat, 0);
                _eventPosition.Measure = 0;
                _nextEvent = 0;
                PlayEvents(frame, _start, _eventPosition, frameSampleOffset);
            }
            else
            {
                PlayEvents(frame, startEventPosition, _eventPosition, 0);
            }
        }

        private void PlayEvents(int frame, EventLocation startSpan, EventLocation endSpan, int sampleOffset) 
        {
            while (_nextEvent < _events.Count && _events[_nextEvent].Location < endSpan)
            {
                var e = _events[_nextEvent];

                var noteStartLocation = e.Location;
                AudioEventInfo info;
                info.frame = frame;
                info.sampleOffset = (int)Math.Round(EventLocation.GetBeatsSpan(noteStartLocation, startSpan, BeatsPerMeasure) * _samplesPerBeat, 0) + sampleOffset;
                
                e.Track.Play(info);
                _nextEvent++;
            }
        }

        private void UpdateLocation(int samples)
        {
            var spanBeats = samples / _samplesPerBeat;
            _eventPosition.AddBeats(spanBeats, BeatsPerMeasure);
        }

        public double Tempo { get; private set; }

        int _sampleRate;
        private int _nextEvent;
    }
}
