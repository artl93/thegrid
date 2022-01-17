using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioLibrary
{
    public interface IAudioInsert
    {
        void SetBlockSize(int channels, int sampleRate);
        bool WriteToOutput(short[] outData, int sampleOffset, int sampleCount, int frame);
    }
}
