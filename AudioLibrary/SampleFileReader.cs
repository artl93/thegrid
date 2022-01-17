using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioLibrary
{
    public class SampleFileReader : Microsoft.Xna.Framework.Content.ContentTypeReader<SampleFile>
    {
        protected override SampleFile Read(Microsoft.Xna.Framework.Content.ContentReader input, SampleFile existingInstance)
        {
            return SampleFile.OpenWAVFile(input.BaseStream);
        }
    }
}
