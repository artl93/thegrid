using TheGrid.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TheGrid;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using AudioLibrary;

namespace TheGridTests
{
    
    
    /// <summary>
    ///This is a test class for TrackGridFacadeTest and is intended
    ///to contain all TrackGridFacadeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TrackGridFacadeTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private void AddEightQuarterNotesToTrack(SequencerTrack track, int note)
        {
            for (int meas = 0; meas < 2; meas++)
            {
                for (int beat = 0; beat < 4; beat++)
                {
                    var loc = new EventLocation();
                    loc.Beat = beat;
                    loc.Measure = meas;
                    track.AddEvent(loc, note);
                }
            }
        }

        /// <summary>
        ///A test for GetButtonState
        ///</summary>
        [TestMethod()]
        public void GetButtonStateTest()
        {
            var track = SequencerTrack.Create(null, 4, 2); 
            var sequencer = new Sequencer(44100, 120);
            var map = new NoteEventMap(Scales.HappinessScale, 8, 3);
            int column = 0;
            AddEightQuarterNotesToTrack(track, map.GetNote(column));
            int columnsPerQuarter = 2;
            TrackGridFacade target = new TrackGridFacade(track, sequencer, map, columnsPerQuarter);
            int row = 0;
            bool expected = true; 
            bool actual;
            actual = target.GetButtonState(row, column);
            Assert.AreEqual(expected, actual);
            column = 1;
            expected = false;
            actual = target.GetButtonState(row, column);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetActiveButtons
        ///</summary>
        [TestMethod()]
        public void GetActiveButtonsTest()
        {
            var track = SequencerTrack.Create(null, 4, 2);
            var sequencer = new Sequencer(44100, 120);
            var map = new NoteEventMap(Scales.HappinessScale, 8, 3);
            int column = 0;
            AddEightQuarterNotesToTrack(track, map.GetNote(column));
            int columnsPerQuarter = 2;
            TrackGridFacade target = new TrackGridFacade(track, sequencer, map, columnsPerQuarter);
            // row, column, is present in actual
            Dictionary<Point, bool> expected = new Dictionary<Point, bool>
            {
                { new Point(0, 0), false},
                { new Point(0, 2), false},
                { new Point(0, 4), false},
                { new Point(0, 6), false},
                { new Point(0, 8), false},
                { new Point(0, 10), false},
                { new Point(0, 12), false},
                { new Point(0, 14), false},
            };

            var actual = target.GetActiveButtons();
            foreach (var result in actual)
            {
                var actualGridLocation = new Point(result.Y, result.X);
                Assert.IsTrue(expected.ContainsKey(actualGridLocation));
                Assert.IsFalse(expected[actualGridLocation], "Result already present");
                expected[actualGridLocation] = true;
            }

            foreach (var isPresent in expected.Values)
                Assert.IsTrue(isPresent, "Actual was missing some values"); 
            
        }

        ///// <summary>
        /////A test for GetColumnFromLocation
        /////</summary>
        //[TestMethod()]
        //public void GetColumnFromLocationTest()
        //{
        //    EventLocation eventLocation = new EventLocation(div: 0.5d); 
        //    int actual;
        //    var columnsPerQuarter = 2;
        //    var expected = 1;
        //    actual = TrackGridFacade.GetColumnFromLocation(eventLocation);
        //    Assert.AreEqual(expected, actual);
        //}
    }
}
