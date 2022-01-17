using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    enum Waveform
    {
        Sine, 
        Triangle, 
        Square, 
        SawUp, 
        SawDown
    }

    class WaveformGenerator : IAudioComponent
    {
        private int _channels;
        private int _sampleRate;
        private double _position;
        private int _bytesPerSample;
        private Waveform _waveform;


        public WaveformGenerator(Waveform waveform, int sampleRate, int channels, int bits)
        {
            _waveform = waveform;
            _sampleRate = sampleRate;
            _channels = channels;
            _bytesPerSample = bits / 8;

        }

        const double pitchFrequency = 440.0;


        public void WriteToOutput(byte[] outData, int offset, int length, int frame)
        {
            double samplesPerCycle = _sampleRate / pitchFrequency;

            for (int sample = offset; sample < offset + length; sample += (_channels * _bytesPerSample))
            {
                if (_position > samplesPerCycle) 
                    _position -= samplesPerCycle;
                short val = GetPCMValue(samplesPerCycle);
                _position++;

                var byte1 = (byte)(val);
                var byte2 = (byte)(val >> 8);


                for (int channel = 0; channel < _channels; ++channel)
                {
                    outData[sample + (channel * _bytesPerSample)] = byte1;
                    outData[sample + (channel * _bytesPerSample) + 1] = byte2;
                }
            }
        }

        private short GetPCMValue(double samplesPerCycle)
        {
            switch (_waveform)
            {
                case Waveform.Sine:
                    return (short)(Math.Sin(2 * Math.PI * _position / samplesPerCycle) * short.MaxValue);
                case Waveform.Square:
                    return _position < samplesPerCycle ? short.MaxValue : short.MinValue;
                case Waveform.Triangle:
                    double tempPosition = _position + (samplesPerCycle / 4);
                    if (tempPosition < (samplesPerCycle / 2))
                        return (short)((ushort)(tempPosition / (samplesPerCycle / 2)) * ushort.MaxValue);
                    else
                        return (short)(ushort.MaxValue - ((ushort)(tempPosition / (samplesPerCycle / 2)) * ushort.MaxValue));
                case Waveform.SawUp:
                    return (short)((ushort)(_position / (samplesPerCycle)) * ushort.MaxValue);
                case Waveform.SawDown:
                    return (short)(ushort.MaxValue - ((ushort)(_position / (samplesPerCycle)) * ushort.MaxValue));
            }
            throw new Exception("Invalid waveform selection");
        }

        public void SetBlockSize(int blockSize, int channels)
        {

        }


    }
}
