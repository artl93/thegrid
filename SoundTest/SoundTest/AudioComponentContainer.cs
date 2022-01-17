using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    abstract class AudioComponentContainer : IAudioComponent
    {
        Dictionary<string, IAudioComponent> _channels = new Dictionary<string, IAudioComponent>();
        private short _channelCount;
        int _tempBufferLen = 0;
        byte[] _tempBuffer;
        int[] _renderBuffer;


        protected AudioComponentContainer() {
        }

        void EnsureTempBuffers(int length)
        {
            if (length > _tempBufferLen)
            {
                _tempBufferLen = length;
                _tempBuffer = new byte[_tempBufferLen];
                _renderBuffer = new int[_tempBufferLen / 2];
            }
            for (int i = 0; i < _tempBufferLen / 2; i++)
                _renderBuffer[i] = 0;
        }


        public void WriteToOutput(byte[] outData, int offset, int length, int frame)
        {
            EnsureTempBuffers(length);

            foreach (var channel in _channels)
            {
                channel.Value.WriteToOutput(_tempBuffer, 0, length, frame);
                MixToOutputBuffer(outData, offset, length);
            }

        }

        private void MixToOutputBuffer(byte[] outData, int offset, int length)
        {
            for (int i = 0; i < length / 2; i++)
            {
                short value = (short)(((_tempBuffer[i * 2]) + (_tempBuffer[(i * 2) + 1] << 8)));

                _renderBuffer[i] += value;
            }

            for (int i = 0; i < length / 2; i++)
            {
                short value = (short)(_renderBuffer[i] / _channelCount);

                var byte1 = (byte)(value);
                var byte2 = (byte)(value >> 8);

                outData[offset + i * 2] = byte1;
                outData[offset + (i * 2) + 1] = byte2;
            }
        }

        public void AddComponent(string name, IAudioComponent component)
        {
            _channels.Add(name, component);
            _channelCount = (short)_channels.Count;
        }


        public IDictionary<string, IAudioComponent> GetComponents()
        {
            return _channels;
        }

        public void SetBlockSize(int size, int channels)
        {
            foreach (var channel in _channels)
            {
                channel.Value.SetBlockSize(size, channels);
            }
        }
    }
}
