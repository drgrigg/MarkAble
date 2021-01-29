using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
//using System.Xml.Serialization;

namespace MarkAble2
{
    [System.Xml.Serialization.XmlInclude(typeof(CDTrackRecord))]
    [System.Xml.Serialization.XmlInclude(typeof(SeparateRecord))]
    public abstract class FileRecord
    {       
        [System.Xml.Serialization.XmlAttribute]
        public Global.FileTypes FileType = Global.FileTypes.AAC;

        //this is what we'll use for chapter stops
        [System.Xml.Serialization.XmlAttribute]
        public string ChapterName = "";

        [System.Xml.Serialization.XmlAttribute]
        public string FilePath { get; set; }

        [System.Xml.Serialization.XmlElement]
        public long DurationTicks;

        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan Duration
        {
            get { return new TimeSpan(DurationTicks); }
            set
            {
                DurationTicks = value.Ticks;
            }
        }

        [System.Xml.Serialization.XmlIgnore]
        public long CumulativeTicks = 0;

        [System.Xml.Serialization.XmlIgnore]
        public TimeSpan CumulativeDuration
        {
            get { return new TimeSpan(CumulativeTicks); }
            set
            {
                CumulativeTicks = value.Ticks;
            }
        }

        [System.Xml.Serialization.XmlElement]
        public long Size = 0;

        public FileRecord()
        {
            FilePath = "";
        }

        public enum StringTypes
        {
            FullPath,
            FileName,
            Trimmed,
            ChapterName
        }

        [System.Xml.Serialization.XmlIgnore]
        public StringTypes StringType = StringTypes.FullPath;


        public override string ToString()
        {
            switch(StringType)
            {
                case StringTypes.FullPath:
                    return FilePath;
                case StringTypes.FileName:
                    return Global.GetFilename(FilePath, true);
                case StringTypes.Trimmed:
                    return TrimmedPath(85);
                case StringTypes.ChapterName:
                    return ChapterName;
                default:
                    return base.ToString();
            }
        }


        private string TrimmedPath(int maxlen)
        {
            if ((maxlen < 23) || (FilePath.Length <= maxlen))
            {
                return FilePath;
            }

            string path = Global.GetPath(FilePath);
            string fname = @"\" + Global.GetFilename(FilePath, true);

            if (fname.Length >= (maxlen - 3))
                return "..." + fname;

            int maxpath = maxlen - fname.Length - 3;

            if ((maxpath % 2 == 1) && (maxpath > 2))
                maxpath--;

            return path.Substring(0, (maxpath/2)) + "..." + path.Substring(path.Length - (maxpath/2)) + fname;
        }
    }
}
