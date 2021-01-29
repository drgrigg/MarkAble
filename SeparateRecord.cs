using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class SeparateRecord:FileRecord
    {
        [System.Xml.Serialization.XmlElement]
        public bool Converted = false;

        [System.Xml.Serialization.XmlElement]
        public bool Protected = false; //this is whether the file has DRM protection (mostly for WMA files)

    }
}
