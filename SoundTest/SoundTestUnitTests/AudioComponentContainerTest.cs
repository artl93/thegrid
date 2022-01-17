using SoundTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace SoundTestUnitTests
{
    
    
    /// <summary>
    ///This is a test class for AudioComponentContainerTest and is intended
    ///to contain all AudioComponentContainerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AudioComponentContainerTest
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


        internal virtual AudioComponentContainer CreateAudioComponentContainer()
        {
            // TODO: Instantiate an appropriate concrete class.
            AudioComponentContainer target = Mixer.Create();
            target.AddComponent("sine generator1", new WaveformGenerator(Waveform.Sine, 41000, 2, 16));
            target.AddComponent("sine generator2", new WaveformGenerator(Waveform.Sine, 41000, 2, 16));
            return target;
        }

        /// <summary>
        ///A test for WriteToOutput
        ///</summary>
        [TestMethod()]
        public void WriteToOutputSummingSineTest()
        {
            const int sampleRate = 41000;
            const int channels = 2;
            const int bitsPerSample = sizeof(short) * 8;
            AudioComponentContainer target = Mixer.Create();
            target.AddComponent("sine generator1", new WaveformGenerator(Waveform.Sine, sampleRate, channels, bitsPerSample));
            target.AddComponent("sine generator2", new WaveformGenerator(Waveform.Sine, sampleRate, channels, bitsPerSample));
            var expectedResultGenerator = new WaveformGenerator(Waveform.Sine, sampleRate, channels, bitsPerSample);

            byte[] expectedData = new byte[256 * channels * sizeof(short)];
            expectedResultGenerator.WriteToOutput(expectedData, 0, expectedData.Length, 0);

            byte[] outData = new byte[256 * channels * sizeof(short)]; // TODO: Initialize to an appropriate value
            int offset = 0; // TODO: Initialize to an appropriate value
            int length = outData.Length; // TODO: Initialize to an appropriate value
            int frame = 0; // TODO: Initialize to an appropriate value
            target.WriteToOutput(outData, offset, length, frame);

            for (int i = 0; i < (expectedData.Length); i+=sizeof(short))
            {
                var expected = BitConverter.ToInt16(expectedData, i);
                var actual = BitConverter.ToInt16(outData, i);
                Assert.IsTrue(expected == actual,String.Format("expected = {0}; actual = {1}", expected, actual));
            }
        }
    }
}
