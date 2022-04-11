using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace AudioLibrary
{
    public class Patch
    {
        ContentManager _content;
        private List<string> _list;
        private Dictionary<int, SampleFile> _fileMap = new Dictionary<int,SampleFile>(); 


        public Patch(List<string> list, ContentManager content)
        {
            _content = content;
            _list = list;
        }

        internal SampleFile GetSampleFile(int note)
        {
            if (_fileMap.ContainsKey(note))
                return _fileMap[note];

            Debug.WriteLine(String.Format("Start loading: {0}",  _list[note]));
            var waveFile = _content.Load<SampleFile>(_list[note]);
            Debug.WriteLine(String.Format("End loading: {0}",  _list[note]));
            _fileMap.Add(note, waveFile);
            return waveFile;
        }

        public int CountOfSamples
        {
            get
            {
                return _list.Count;
            }
        }

        internal bool IsSampleLoaded(int note)
        {
            return (_fileMap.ContainsKey(note));
        }
    }
}
