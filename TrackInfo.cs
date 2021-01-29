using System.Collections.Generic;
using iTunesLib;

namespace MarkAble2
{
    public class TrackInfoList : List<TrackInfo>
    {

    }

    public class TrackInfo
    {
        public string Location { get; internal set; }
        public int Duration { get; internal set; }
        public string Name { get; internal set; }

        public int DatabaseID { get; internal set; }
        public int SourceID { get; internal set; }
        public int PlaylistID { get; internal set; }
        public int TrackID { get; internal set; }
    }
}
