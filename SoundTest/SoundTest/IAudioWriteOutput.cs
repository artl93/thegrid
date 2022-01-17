using System;
namespace SoundTest
{
    interface IAudioBlockWriter
    {
        void SetBlockSize(int blockSize, int channels);
        void WriteToOutput(byte[] outData, int offset, int length, int frame);
    }
}
