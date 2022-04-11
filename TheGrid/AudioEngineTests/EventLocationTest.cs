using AudioLibrary;
using Xunit;
using System;

namespace AudioLibraryTests
{
    
    
    /// <summary>
    ///This is a test class for EventLocationTest and is intended
    ///to contain all EventLocationTest Unit Tests
    ///</summary>
    public class EventLocationTest
    {
        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [Fact()]
        public void CompareToBeatTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 }; 
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            int expected = 0; 
            int actual;
            actual = target.CompareTo(other);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [Fact()]
        public void CompareToMeasureTest()
        {
            EventLocation target = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 1 };
            EventLocation other = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 1 };
            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [Fact()]
        public void CompareToDivTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.1f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.1f, Measure = 0 };
            int expected = 0;
            int actual;
            actual = target.CompareTo(other);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [Fact()]
        public void CompareToBeatGreaterTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [Fact()]
        public void CompareToBeatLessTest()
        {
            EventLocation target = new EventLocation() { Beat = 2, Div = 0.0f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [Fact()]
        public void CompareToMeasureGreaterTest()
        {
            EventLocation target = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 1 };
            EventLocation other = new EventLocation() { Beat = 0, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.Equal(expected, actual);
        }

        /// <summary>
        ///A test for CompareTo
        ///</summary>
        [Fact()]
        public void CompareToDivGreaterTest()
        {
            EventLocation target = new EventLocation() { Beat = 1, Div = 0.1f, Measure = 0 };
            EventLocation other = new EventLocation() { Beat = 1, Div = 0.0f, Measure = 0 };
            int expected = 1;
            int actual;
            actual = target.CompareTo(other);
            Assert.Equal(expected, actual);
        }


        /// <summary>
        ///A test for AddBeats
        ///</summary>
        [Fact()]
        public void AddBeatsWholeTest()
        {
            EventLocation target = new EventLocation(); // TODO: Initialize to an appropriate value
            double spanBeats = 1.0D; // TODO: Initialize to an appropriate value
            int beatsPerMeasure = 4; // TODO: Initialize to an appropriate value
            target.AddBeats(spanBeats, beatsPerMeasure);
            int expectedBeats = 1;
            Assert.Equal(expectedBeats, target.Beat);
        }


        /// <summary>
        ///A test for AddBeats
        ///</summary>
        [Fact()]
        public void AddBeatsPartialTest()
        {
            EventLocation target = new EventLocation(); // TODO: Initialize to an appropriate value
            double spanBeats = 0.5D; // TODO: Initialize to an appropriate value
            int beatsPerMeasure = 4; // TODO: Initialize to an appropriate value
            target.Div = 0.5F;
            target.AddBeats(spanBeats, beatsPerMeasure);
            int expectedBeats = 1;
            double expectedRemainder = 0.0D;
            Assert.Equal(target.Beat, expectedBeats);
            Assert.Equal(target.Div, expectedRemainder);
        }

        /// <summary>
        ///A test for AddBeats
        ///</summary>
        [Fact()]
        public void AddBeatsPartialWithRemainderTest()
        {
            EventLocation target = new EventLocation(); 
            double spanBeats = 0.5D; 
            int beatsPerMeasure = 4; 
            target.Div = 0.6F;
            target.AddBeats(spanBeats, beatsPerMeasure);
            int expectedBeats = 1;
            double expectedRemainder = 0.1D;
            Assert.Equal(target.Beat, expectedBeats);
            Assert.True(target.Div >= expectedRemainder);
        }
    }
}
