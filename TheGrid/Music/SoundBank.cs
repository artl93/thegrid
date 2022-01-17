using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using AudioLibrary;

namespace TheGrid
{
    static public class SoundSetNames
    {
        public static string HelioSounds = "Glock";
        public static string GlitchDrums = "RealDrums";
        public static string Bass1 = "Bass2";
    }

    static class SoundBank
    {


        private static Dictionary<string, Patch> _soundSets = new Dictionary<string, Patch>();


        internal static void InitStaticContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            
            lock (_soundSets)
            {
                if ( _soundSets.Count > 0 )
                    return;
                var soundSetNames = new string[] { SoundSetNames.HelioSounds, SoundSetNames.GlitchDrums, SoundSetNames.Bass1 };

                foreach (var soundSetName in soundSetNames)
                {
                    Patch set = new Patch(Content.LoadContentList("Sounds\\" + soundSetName), Content);
                    _soundSets.Add(soundSetName, set);
                }
            }
        }

        internal static Patch GetPatch(string soundSet)
        {
            return _soundSets[soundSet];
        }

        internal static void UnloadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            _soundSets = null;
        }
    }
}
