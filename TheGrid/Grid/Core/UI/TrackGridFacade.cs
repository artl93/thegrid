using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioLibrary;
using Microsoft.Xna.Framework;

namespace TheGrid.UI
{
    class TrackGridFacade
    {
        private Sequencer _sequencer;
        private SequencerTrack _track;
        private NoteEventMap _map;
        private int columnsPerQuarter;

        public TrackGridFacade(SequencerTrack track, Sequencer sequencer, NoteEventMap map, int columnsPerQuater)
        {
            _track = track;
            _map = map;
            _sequencer = sequencer;
            _track.NoteStarted += new EventHandler<NoteStartedEventArg>(_track_NoteStarted);
            this.columnsPerQuarter = columnsPerQuater;
        }

        public event EventHandler<ButtonStartEventArgs> StartButtonAnimation;

        void _track_NoteStarted(object sender, NoteStartedEventArg e)
        {
            var row = _map.GetGridRow(e.Note);
            var column = GetColumnFromLocation(e.Location);
            FireNotStarted(row, column);
        }

        private void FireNotStarted(int row, int column)
        {
            var handler = StartButtonAnimation;
            if (handler != null)
            {
                var args = new TheGrid.UI.ButtonStartEventArgs(row, column);
                handler(this, args);
            }
        }

        public bool GetButtonState(int row, int column)
        {
            // row => event time
            // column => pitch / sample

            EventLocation location = new EventLocation();
            location.AddBeats(((double)column / columnsPerQuarter), 4);
            int note = _map.GetNote(row);
            var events = from e in _track.GetEvents()
                         where e.Location == location && e.Note == note
                         select e;
            return events.Count() > 0;
        }

        public IEnumerable<Point> GetActiveButtons()
        {
            EventLocation lastLocation = new EventLocation();

            // note: events in this state are already SORTED
            foreach (var evt in _track.GetEvents())
            {
                System.Diagnostics.Debug.Assert(lastLocation <= evt.Location);
                int row = _map.GetGridRow(evt.Note);
                int column = GetColumnFromLocation(evt.Location);
                yield return new Point(column, row);
                lastLocation = evt.Location;
            }
        }

        public int GetCurrentColumn()
        {
            var currentLocation = _sequencer.CurrentPosition;
            var column = ((currentLocation.Measure * 4) + (currentLocation.Beat + currentLocation.Div)) * columnsPerQuarter;
            return (int)column;
        }

        internal int GetColumnFromLocation(EventLocation eventLocation)
        {
            var column = ((eventLocation.Measure * 4) + (eventLocation.Beat + eventLocation.Div)) * columnsPerQuarter;
            return (int)column;
        }

        internal EventLocation GetLocationFromColumn(int column)
        {
            EventLocation location = new EventLocation();
            location.AddBeats(column / (double)columnsPerQuarter, 4);
            return location ;
        }

        internal void SetButtonState(int row, int column, bool value)
        {
            var currentState = GetButtonState(row, column);
            if (currentState == value)
                return;
            var noteValue = _map.GetNote(row);
            var location = GetLocationFromColumn(column);
            if (value)
                _track.AddEvent(location, noteValue);
            else
                _track.RemoveEvent(location, noteValue);
        }

        internal void Clear()
        {
            _sequencer.Clear();
        }

        internal void ClearEventsAtColumn(int col, int rows)
        {
            var location = GetLocationFromColumn(col);
            for (int row = 0; row < rows; row++)
            {
                var noteValue = _map.GetNote(row);
                _track.RemoveEvent(location, noteValue);
            }
        }

        internal void PreloadPatches()
        {
            _track.PreloadPatch(_map.GetNotes());
        }


    }

}
