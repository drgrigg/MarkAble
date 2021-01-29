using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class CDTrackRecord:FileRecord
    {
        [System.Xml.Serialization.XmlElement]
        public int DiscNumber = 0;

        [System.Xml.Serialization.XmlElement]
        public int TrackNumber = 0;
    }
}
