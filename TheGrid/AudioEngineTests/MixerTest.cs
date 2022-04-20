using AudioLibrary;
using Xunit;
using System;

namespace AudioLibraryTests
{
    
    
    /// <summary>
    ///This is a test class for AudioComponentContainerTest and is intended
    ///to contain all AudioComponentContainerTest Unit Tests
    ///</summary>
    public class MixerTest
    {

        internal virtual Mixer CreateAudioComponentContainer()
        {
            // TODO: Instantiate an appropriate concrete class.
            var target = Mixer.Create();
            target.AddComponent("sine generator1", WaveformGenerator.Create(Waveform.Sine));
            target.AddComponent("sine generator2", WaveformGenerator.Create(Waveform.Sine));
            return target;
        }

        /// <summary>
        ///A test for WriteToOutput
        ///</summary>
        [Fact]
        public void WriteToOutputSummingSineTest()
        {
            const int sampleRate = 44100;
            const int channels = 1;
            int offset = 0; 
            int frame = 0;
            var outData = new short[256 * channels];
            int length = outData.Length;

            var target = Mixer.Create();

            var insert1 = CreateWaveformInsert(0.5F);
            var insert2 = CreateWaveformInsert(0.5F);
            target.AddComponent("sine generator1", insert1);
            target.AddComponent("sine generator2", insert2);
            target.SetBlockSize(channels, sampleRate);


            var expectedResultGenerator = WaveformGenerator.Create(Waveform.Sine);
            expectedResultGenerator.SetBlockSize(channels, sampleRate);
            expectedResultGenerator.Play = true;

            var expectedData = new short[256 * channels];
            expectedResultGenerator.WriteToOutput(expectedData, 0, expectedData.Length, 0);

            target.WriteToOutput(outData, offset, length, frame);

            for (int i = 0; i < (expectedData.Length); i += sizeof(short))
            {
                var expected = expectedData[i];
                var actual = outData[i];
                System.Diagnostics.Debug.WriteLine(i);
                Assert.True((expected >= actual - 2) &&
                        (expected <= actual + 2),
                        String.Format($"expected = {expected}; actual = {actual}"));
            }
        }

        private static InsertChain CreateWaveformInsert(float attenuation)
        {
            var generator = WaveformGenerator.Create(Waveform.Sine);
            var attenuator = new Attenuator() { Level = attenuation };
            var insertChain = InsertChain.Create();
            insertChain.AddInsert(generator);
            insertChain.AddInsert(attenuator);
            generator.Play = true;
            return insertChain;
        }
    }
}
