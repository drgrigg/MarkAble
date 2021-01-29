using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class CDFileList : FileListBase
    {
        [System.Xml.Serialization.XmlAttribute]
        public int CurrentDisc = 0;

        [System.Xml.Serialization.XmlAttribute]
        public int TotalDiscs = 0;



        public CDFileList()
            : base()
        {
        }


    }
}
