using System;
using System.Collections.Generic;
using System.Text;
using AAClib;

namespace MarkAble2
{
    public class BadFiles:List<BadFile>
    {
    }

    public class BadFile
    {
        public string FileName = "";
        public AACfile.MergeResults FileProblem = AACfile.MergeResults.Success;

        public override string ToString()
        {
            return FileProblem.ToString() + ": " + Global.GetFilename(FileName);
        }
    }
}
