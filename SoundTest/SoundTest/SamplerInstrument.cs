using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoundTest
{
    class SamplerInstrument : AudioComponentContainer, INotePlayer, IAudioComponent
    {
        int _nextPlayer = 0;
        const int _maxVoices = 1;
        INotePlayer[] _voices = new INotePlayer[_maxVoices];

        private SamplerInstrument() { }

        public void Play(AudioEventInfo info)
        {
            INotePlayer player = GetNextAvailablePlayer();
            player.Play(info);
        }

        private INotePlayer GetNextAvailablePlayer()
        {
            var voice = _voices[_nextPlayer];
            _nextPlayer++;
            if (_nextPlayer >= _maxVoices)
                _nextPlayer = 0;
            return voice; 
        }

        internal static SamplerInstrument Create(SampleFile data)
        {
            var sampler = new SamplerInstrument();
            for (int i = 0; i < _maxVoices; i++)
            {
                
                var voice = SimpleSamplePlayer.Create(data);
                sampler._voices[i] = voice;
                sampler.AddComponent(i.ToString(), voice);
            }
            return sampler;
        }
    }
}
