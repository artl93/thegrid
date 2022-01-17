using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AudioLibrary
{
    public interface IAudioComponent 
    {
        void SetBlockSize(int channels, int sampleRate);
        bool WriteToOutput(short[] outData, int sampleOffset, int sampleCount, int frame);
    }
}
