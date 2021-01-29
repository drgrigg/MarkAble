using System.Collections.Generic;

namespace MarkAble2
{
    public class PathComparer : IComparer<FileRecord>
    {
        public int Compare(FileRecord x, FileRecord y)
        {
            //string pathx = Global.GetPath(x.FilePath);
            //string pathy = Global.GetPath(y.FilePath);

            //int result = pathx.CompareTo(pathy);

            //if (result != 0)
            //    return result;

            string xs = Global.CreateSortable(x.FilePath);
            string ys = Global.CreateSortable(y.FilePath);
            return xs.CompareTo(ys);
        }
    }
}
