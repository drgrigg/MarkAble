using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class FileProcess
    {
        [System.Xml.Serialization.XmlAttribute]
        public string BookTitle = "";

        [System.Xml.Serialization.XmlAttribute]
        public string AuthorName = "";

        [System.Xml.Serialization.XmlAttribute]
        public int CurrentDisc = 0;

        [System.Xml.Serialization.XmlAttribute]
        public int TotalDiscs = 0;

        [System.Xml.Serialization.XmlIgnore]
        public bool MergedOK = false;

        [System.Xml.Serialization.XmlIgnore]
        public string MergedPath = "";

        [System.Xml.Serialization.XmlIgnore]
        public string FinalPath = "";

        [System.Xml.Serialization.XmlElementAttribute()]
        public string BookImage = "";

        [System.Xml.Serialization.XmlElementAttribute()]
        public double RegularMins;

        [System.Xml.Serialization.XmlElementAttribute()]
        public int NumParts = 1;

        [System.Xml.Serialization.XmlElementAttribute()]
        public Global.ChapterTypes ChapterType = Global.ChapterTypes.None;

        [System.Xml.Serialization.XmlElementAttribute()]
        public int TrackSpacing = 1;


    }
}
