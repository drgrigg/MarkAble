using System;
using System.Collections.Generic;
using System.Text;
using iTunesLib;

namespace MarkAble2
{

    public class AudioFileItem
    {
        public enum AudioTypes
        {
            None,
            MP3,
            WAV,
            CDfile,
            CDtrack,
            AAC,
            WindowsMedia,
            Other
        }

        private string myFilename = "";
        private AudioTypes myAudioType;
        private long myLength = 0;
        private int myMergeOrder = 0;
        private string myDiscNum = "00";
        private string myTrackNum = "00";
        private string myAlbum = "";
        private string myArtist = "";
        private string myExportName = "";
        private string myTitle = "";
        private string myEncoder = "";
        private string myComment = "";
        private string myYear = "";
        private string myGenre = "Audiobook";
        private bool myPodcast = false;
        private long myNumFrames = 0;
        private double myDuration = 0;
        private int myChannels = 2;
        private int myFirstFrameLength = 0;
        private bool myHasXing = false;
        private int myOffset = 36;

        public int byte1;
        public int byte2;
        public int byte3;
        public int byte4;

        public IITFileOrCDTrack Track = null;

        public AudioFileItem()
        {
            myAudioType = AudioTypes.None;
        }

        public AudioTypes AudioType
        {
            get
            {
                return myAudioType;
            }
            set
            {
                myAudioType = value;
            }
        }

        public long Length
        {
            get
            {
                return myLength;
            }
            set
            {
                myLength = value;
            }
        }

        public string Filename
        {
            get
            {
                return myFilename;
            }
            set
            {
                myFilename = value;
            }
        }

        public override string ToString()
        {
            return myFilename;
        }

        public int MergeOrder
        {
            get
            {
                return myMergeOrder;
            }
            set
            {
                myMergeOrder = value;
            }
        }


        public string DiscNum
        {
            get
            {
                return myDiscNum;
            }
            set
            {
                myDiscNum = value;
            }
        }

        public string TrackNum
        {
            get
            {
                return myTrackNum;
            }
            set
            {
                myTrackNum = value;
            }
        }

        public string Album
        {
            get
            {
                return myAlbum;
            }
            set
            {
                myAlbum = value;
            }
        }

        public string Artist
        {
            get
            {
                return myArtist;
            }
            set
            {
                myArtist = value;
            }
        }

        public string Genre
        {
            get
            {
                return myGenre;
            }
            set
            {
                myGenre = value;
            }
        }

        public string Title
        {
            get
            {
                return myTitle;
            }
            set
            {
                myTitle = value;
            }
        }

        public bool Podcast
        {
            get
            {
                return myPodcast;
            }
            set
            {
                myPodcast = value;
            }
        }

        public long NumFrames
        {
            get
            {
                return myNumFrames;
            }
            set
            {
                if (value >= 0)
                    myNumFrames = value;
            }
        }

        public double Duration
        {
            get
            {
                return myDuration;
            }
            set
            {
                if (value >= 0)
                    myDuration = value;
            }
        }

        public int Channels
        {
            get
            {
                return myChannels;
            }
            set
            {
                if ((value == 1) || (value == 2))
                    myChannels = value;
            }
        }

        public int FirstFrameLength
        {
            get
            {
                return myFirstFrameLength;
            }
            set
            {
                myFirstFrameLength = value;
            }
        }

        public int Offset
        {
            get
            {
                return myOffset;
            }
            set
            {
                if (value > 0)
                    myOffset = value;
            }
        }

        public string Encoder
        {
            get
            {
                return myEncoder;
            }
            set
            {
                myEncoder = value;
            }
        }

        public string Comment
        {
            get
            {
                return myComment;
            }
            set
            {
                myComment = value;
            }
        }

        public string Year
        {
            get
            {
                return myYear;
            }
            set
            {
                myYear = value;
            }
        }

        public bool HasXing
        {
            get
            {
                return myHasXing;
            }
            set
            {
                myHasXing = value;
            }
        }

        public string ExportName
        {
            get { return myExportName; }
            set { myExportName = value; }
        }

    }

}

