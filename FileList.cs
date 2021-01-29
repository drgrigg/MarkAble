using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MarkAble2
{
    public class FileListBase
    {
        [System.Xml.Serialization.XmlAttribute]
        public string BookTitle = "Unknown";

        [System.Xml.Serialization.XmlAttribute]
        public string Author = "Unknown";

        [System.Xml.Serialization.XmlAttribute]
        public string BookImage = "";

        [System.Xml.Serialization.XmlAttribute]
        public double RegularMins = 15.0;

        [System.Xml.Serialization.XmlAttribute]
        public int NumParts = 1;

        [System.Xml.Serialization.XmlAttribute]
        public Global.ChapterTypes ChapterType = Global.ChapterTypes.None;

        [System.Xml.Serialization.XmlAttribute]
        public int TrackSpacing = 1;
        
        //----------not to be serialized, temporary values.
        
        [System.Xml.Serialization.XmlIgnore]
        public bool MergedOK = false;

        [System.Xml.Serialization.XmlIgnore]
        public string MergedPath = "";

        [System.Xml.Serialization.XmlIgnore]
        public string FinalPath = "";




        [System.Xml.Serialization.XmlArrayItem("File", typeof(FileRecord))]
        public List<FileRecord> Files;

        public FileListBase()
        {
            Files = new List<FileRecord>();
        }

        [System.Xml.Serialization.XmlIgnore]
        public int Count
        {
            get { return Files.Count; }
        }

        public void Clone(FileListBase source)
        {
            Author = source.Author;
            BookTitle = source.BookTitle;
            BookImage = source.BookImage;
            ChapterType = source.ChapterType;
            NumParts = source.NumParts;
            RegularMins = source.RegularMins;
            TrackSpacing = source.TrackSpacing;
        }

        public long TotalSize()
        {
            long total = 0;
            IEnumerator<FileRecord> enumerator = Files.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current.FileType == Global.FileTypes.WAV)
                {
                    total += 0; //can't rely on WAV file size - it will be far bigger than converted AAC or MP3
                }
                else
                {
                    total += enumerator.Current.Size;
                }
            }

            return total;
        }

        public TimeSpan TotalDuration()
        {
            long total = 0;
            //debug
            //TimeSpan temp = new TimeSpan();
            //end debug

            IEnumerator<FileRecord> enumerator = Files.GetEnumerator();
            //int n = 0;
            while (enumerator.MoveNext())
            {
                long ticks = enumerator.Current.DurationTicks;
                total += ticks;
                //n++;
                //debug
                //temp = new TimeSpan(total);
                //end debug
            }

            return new TimeSpan(total);
//            return temp.Add(new TimeSpan(n));
        }

        public virtual void Add(FileRecord frec)
        {
            Files.Add(frec);
        }

        public virtual FileRecord this[int i1]
        {
            get { return Files[i1]; }
        }

        public virtual void RemoveAt(int pos)
        {
            Files.RemoveAt(pos);
        }

        public virtual void Remove(FileRecord frec)
        {
            Files.Remove(frec);
        }

        public bool AllFilesExist()
        {
            foreach(var filerec in Files)
            {
                if (!File.Exists(filerec.FilePath))
                    return false;
            }

            return true;
        }

    }


}
