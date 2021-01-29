using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class FileNameComparer:IComparer<FileRecord>
    {

        public int Compare(FileRecord x, FileRecord y)
        {
            string xs = Global.CreateSortable(Global.GetFilename(x.FilePath, false));
            string ys = Global.CreateSortable(Global.GetFilename(y.FilePath, false));
            return xs.CompareTo(ys);
        }
    }
}