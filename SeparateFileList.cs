using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class SeparateFileList: FileListBase
    {
        public enum ProcessStages
        {
            None,
            Selected,
            Sorted,
            MetaDataAdded,
            ParametersSet,
            Converting,
            UnMerged,
            Converted
        }

        [System.Xml.Serialization.XmlElement]
        public ProcessStages ProcessStage = ProcessStages.None;

        public void MoveUp(FileRecord frec1)
        {
            int pos1 = Files.IndexOf(frec1);

            if (pos1 >= 1)
            {
                FileRecord previous = Files[pos1 - 1];
                Files.Remove(previous); //removes previous
                Files.Insert(pos1, previous); //puts it back after frec1
            }
        }

        public void MoveDown(FileRecord frec1)
        {
            int pos1 = Files.IndexOf(frec1);

            if (pos1 < (Files.Count - 1))
            {
                FileRecord following = Files[pos1 + 1];
                Files.Remove(following); //removes following
                Files.Insert(pos1, following); //puts it back before frec1
            }

        }


        public void SortByFileName()
        {
            Files.Sort(new FileNameComparer());
        }

        public void SortByPath()
        {
            Files.Sort(new PathComparer());
        }

        public bool AllFilesAreAAC()
        {
            foreach (var frec in Files)
            {
                if (frec.FileType != Global.FileTypes.AAC)
                    return false;
            }
            return true;
        }


        public bool AllFilesAreMP3()
        {
            foreach (var frec in Files)
            {
                if (frec.FileType != Global.FileTypes.MP3)
                    return false;
            }
            return true;
        }

        public bool AllFilesAreProtected()
        {
            foreach (SeparateRecord frec in Files)
            {
                if (!frec.Protected)
                    return false;
            }
            return true;
        }

        public bool AnyFileIsProtected()
        {
            foreach (SeparateRecord frec in Files)
            {
                if (frec.Protected)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return BookTitle + " by " + Author;
        }

        public SeparateRecord GetFirstUnconverted()
        {
            foreach(SeparateRecord frec in Files)
            {
                if (frec.Converted == false)
                {
                    return frec;
                }
            }

            return null;
        }

        public bool AllFilesAreConverted()
        {
            foreach (SeparateRecord frec in Files)
            {
                if (!frec.Converted)
                    return false;
            }
            return true;
        }

        //public SeparateRecord CloneFirstUnconverted()
        //{
        //    var oldfrec = GetFirstUnconverted();
        //    var newfrec = new SeparateRecord
        //                      {
        //                          ChapterName = oldfrec.ChapterName,
        //                          DurationTicks = oldfrec.DurationTicks,
        //                          FilePath = oldfrec.FilePath,
        //                          FileType = oldfrec.FileType,
        //                          Protected = oldfrec.Protected,
        //                          Size = oldfrec.Size,
        //                          StringType = oldfrec.StringType
        //                      };

        //    return newfrec;
        //}
    }
}
