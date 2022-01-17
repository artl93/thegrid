using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    class SimpleSamplePlayer : IAudioComponent, INotePlayer
    {

        private int _position = 0;
        private SampleFile _sampleData;
        private int _blockSize = 0;
        private int _channels;

        private SimpleSamplePlayer() { }
        static public SimpleSamplePlayer Create(SampleFile sampleData)
        {
            var player = new SimpleSamplePlayer();
            player._sampleData = sampleData;
            player.Stop();
            return player;
        }


        Dictionary<int, Queue<int>> _queue = new Dictionary<int, Queue<int>>(16);

        struct RenderInfo
        {
            public int sourceOffsetBytes;
            public int outputOffsetBytes;
            public int writeLenthBytes;
        };

        void Stop()
        {
            _position = this._sampleData._data.Length;
        }

        public void WriteToOutput(byte[] outData, int outputOffsetInBytes, int outputLengthInBytes, int frame)
        {
            int currentWriteOffsetBytes = 0;
            int nextOffsetInBytes = outputLengthInBytes;
            RenderInfo info;

            while (_queue.ContainsKey(frame))
            {
                nextOffsetInBytes = _queue[frame].Dequeue() * _blockSize * _channels;
                if (!EOF)
                {
                    // now that you know how long the last one was, render it! 
                    info.outputOffsetBytes = outputOffsetInBytes + currentWriteOffsetBytes;
                    info.sourceOffsetBytes = _position;
                    info.writeLenthBytes = Math.Min(nextOffsetInBytes, _sampleData._data.Length - _position);
                    Render(outData, info);
                }
                Reset();
                currentWriteOffsetBytes = nextOffsetInBytes;
                if (_queue[frame].Count == 0)
                {
                    _queue.Remove(frame);
                }
            }
            if (!EOF)
            {
                info.outputOffsetBytes = outputOffsetInBytes + currentWriteOffsetBytes;
                info.sourceOffsetBytes = _position;
                info.writeLenthBytes = Math.Min(outputLengthInBytes - currentWriteOffsetBytes, _sampleData._data.Length - _position);
                Render(outData, info);
            }
        }

        private void Render(byte[] outData, RenderInfo renderInfo)
        {
            Buffer.BlockCopy(_sampleData._data, _position, outData, renderInfo.outputOffsetBytes, renderInfo.writeLenthBytes);
            _position += renderInfo.writeLenthBytes;
            //for (int i = renderInfo.outputOffsetBytes; i < renderInfo.writeLenthBytes; i++)
            //{
            //    outData[i] = _sampleData._data[_position];
            //    _position++;
            //}
        }


        internal bool EOF
        {
            get
            {
                return _position >= this._sampleData._data.Length;
            }
        }

        internal void Reset()
        {
            _position = 0;
        }

        public void SetBlockSize(int blockSize, int channels)
        {
            _blockSize = blockSize;
            _channels = channels;
        }

        public void Play(AudioEventInfo info)
        {
            // System.Diagnostics.Trace.TraceWarning("Play: {0}, {1}", info.frame, info.sampleOffset);
            if (!_queue.ContainsKey(info.frame))
                _queue.Add(info.frame, new Queue<int>());
            _queue[info.frame].Enqueue(info.sampleOffset);
            Utilities.Log(String.Format("Note Start: Frame={0}\tFrameSize={1}\tFrameOffset={2}\tOverallOffset={3}",
                info.frame,
                Game1._sampleSize,
                info.sampleOffset,
                info.frame * Game1._sampleSize + info.sampleOffset));
                
        }

    }
    
}
