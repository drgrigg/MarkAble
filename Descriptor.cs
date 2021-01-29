using System;
using System.Collections.Generic;
using System.Text;

namespace MarkAble2
{
    public class Descriptor
    {
        //these are for use in making example strings
        private const int exampleDiscNum = 3;
        private const int exampleTrackNum = 5;
        private const string exampleArtist = "H.G.Wells";
        private const string exampleAlbum = "The Time Machine";
        private const string exampleSong = "The White Sphinx";
        private const string exampleFile = "tm_chap3";
        private const string exampleParent = "folder1";

        public string Artist = "";
        public string Album = "";
        public string SongTitle = "";
        public string FileName = "";
        public string ParentFolder = "";
        public int DiscNumber = 1;
        public int TrackNumber = 1;

        public Descriptor()
        {
            
        }

        public void UseAudioCDExampleData()
        {
            Artist = exampleArtist;
            Album = exampleAlbum;
            DiscNumber = exampleDiscNum;
            TrackNumber = exampleTrackNum;
            FileName = "";
            SongTitle = "Track 05";
            ParentFolder = "";
        }

        public void UseSeparateExampleData()
        {
            Artist = exampleArtist;
            Album = exampleAlbum;
            SongTitle = exampleSong;
            DiscNumber = 1;
            TrackNumber = 3;
            FileName = exampleFile;
            ParentFolder = exampleParent;
        }


        public string QualifyString(string astring)
        {
            var sb = new StringBuilder();
            sb.Append(astring);

            sb.Replace("%S", SongTitle);
            sb.Replace("%s", SongTitle);

            sb.Replace("%A", Artist);
            sb.Replace("%a", Artist);
            
            sb.Replace("%B", Album);
            sb.Replace("%b", Album);

            sb.Replace("%F", FileName);
            sb.Replace("%f", FileName);

            sb.Replace("%P", ParentFolder);
            sb.Replace("%p", ParentFolder);

            sb.Replace("%D", DiscNumber.ToString());
            sb.Replace("%d", DiscNumber.ToString());

            sb.Replace("%T", TrackNumber.ToString());
            sb.Replace("%t", TrackNumber.ToString());

            return sb.ToString();
        }

        //this version doesn't insert book title (album) or author (artist), so we can defer this until we have it.
        public string PartQualifyString(string astring)
        {
            var sb = new StringBuilder();
            sb.Append(astring);

            sb.Replace("%S", SongTitle);
            sb.Replace("%s", SongTitle);

            sb.Replace("%F", FileName);
            sb.Replace("%f", FileName);

            sb.Replace("%P", ParentFolder);
            sb.Replace("%p", ParentFolder);

            sb.Replace("%D", DiscNumber.ToString());
            sb.Replace("%d", DiscNumber.ToString());

            sb.Replace("%T", TrackNumber.ToString());
            sb.Replace("%t", TrackNumber.ToString());

            return sb.ToString();
        }

    }
}
