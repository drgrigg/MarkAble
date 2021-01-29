using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class BatchList
    {
        [System.Xml.Serialization.XmlArrayItem("FileList", typeof(SeparateFileList))]
        public FileListCollection Lists = new FileListCollection();

        [System.Xml.Serialization.XmlIgnore]
        public string SourceFile = "";

        public bool SkipConversion = true;
    }
}
