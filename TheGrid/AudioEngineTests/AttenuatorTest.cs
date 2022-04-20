using System;
using AudioLibrary;
using Xunit;

namespace AudioEngineTests
{

	public class AttenuatorTest
	{

		[Fact]
		void LevelTestParityZeroOne()
        {
            const short targetValue = 0;
            const short initialValue = 0;
            const float attenuatorValue = 1.0F;

            TestAttenuation(targetValue, initialValue, attenuatorValue);
        }

        [Fact]
        void LevelTestParityZeroZero()
        {
            const short targetValue = 0;
            const short initialValue = 0;
            const float attenuatorValue = 0.0F;

            TestAttenuation(targetValue, initialValue, attenuatorValue);
        }

        [Fact]
        void LevelTestParityMaxZero()
        {
            const short targetValue = 0;
            const short initialValue = short.MaxValue;
            const float attenuatorValue = 0.0F;

            TestAttenuation(targetValue, initialValue, attenuatorValue);
        }

        [Fact]
        void LevelTestParityMaxInverse()
        {
            const short targetValue = 1024;
            const short initialValue = -1024;
            const float attenuatorValue = -1.0F;

            TestAttenuation(targetValue, initialValue, attenuatorValue);
        }

        [Fact]
        void LevelTestParityHalf()
        {
            const short targetValue = 512;
            const short initialValue = 1024;
            const float attenuatorValue = 0.5F;

            TestAttenuation(targetValue, initialValue, attenuatorValue);
        }

        [Fact]
        void LevelTestParityDoubleInverse()
        {
            const short targetValue = -1024;
            const short initialValue = 512;
            const float attenuatorValue = -2.0F;

            TestAttenuation(targetValue, initialValue, attenuatorValue);
        }



        [Fact]
        void LevelTestSineHalf()
        {
            var generator = AudioLibrary.WaveformGenerator.Create(Waveform.Sine);
            const float attenuatorValue = 0.5F;

            TestAttenuation(targetValue, initialValue, attenuatorValue);
        }


        private static void TestAttenuation(short targetValue, short initialValue, float attenuatorValue)
        {
            const int channels = 1;
            int offset = 0;
            int frame = 0;
            const int length = 256;
            var outData = new short[length * channels];
            for (int i = 0; i < length; i++)
            {
                outData[i] = initialValue;
            }

            var attenuator = new Attenuator();

            attenuator.Level = attenuatorValue;

            var level = attenuator.Level;

            attenuator.WriteToOutput(outData, offset, length, frame);

            for (int i = 0; i < length; i++)
            {
                Assert.Equal(targetValue, outData[i]);
            }
        }
    }
}

