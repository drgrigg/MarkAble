using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iTunesLib;

namespace MarkAble2
{
    public delegate void WaitingHandler(int count);
    public delegate void PercentHandler(double ConvertPercent);
    public delegate void ProgressHandler(string name, int progress, int maxprogress);
    public delegate void ConvertHandler(IITTrack myTrack);
    public delegate void ConversionCompleteHandler(iTunesConvertOperationStatus status);

    public class iTunesApp : iTunesAppClass
    {
        public iTunesApp()
        {
        }

        //		public event PercentHandler ReportPercent;

        public event WaitingHandler Waiting;
        public event ProgressHandler ReportProgress;

        public event ConversionCompleteHandler MP3ConvertedAll;
        public event ConversionCompleteHandler AACConvertedAll;

        private iTunesConvertOperationStatus MP3ConvertStatus;
        private iTunesConvertOperationStatus AACConvertStatus;


        public string GetSources()
        {
            string sourcelist = "";
            foreach (IITSource aSource in this.Sources)
            {
                sourcelist += aSource.Kind.ToString() + "(" + aSource.Name + ");\r\n";
            }
            return sourcelist;
        }

        private IITPlaylist GetCDPlaylist()
        {
            try
            {
                foreach (IITSource aSource in this.Sources)
                {
                    if (aSource.Kind == ITSourceKind.ITSourceKindAudioCD)
                    {
                        if (aSource.Playlists.Count > 0)
                        {
                            return aSource.Playlists[1];
                        }
                    }
                }
                //if we get here, no valid AudioCD
                // Global.LogImmediate("GetCDPlaylist","No CD playlist found");
            }
            catch 
            {
                // Global.LogImmediate("GetCDPlaylist",ex.Message);
            }
            return null;
        }

        public int CountCDs()
        {
            int count = 0;
            try
            {
                foreach (IITSource aSource in this.Sources)
                {
                    if (aSource.Kind == ITSourceKind.ITSourceKindAudioCD)
                    {
                        if (aSource.Playlists.Count > 0)
                        {
                            count++;
                        }
                    }
                }
            }
            catch 
            {
                // probably iTunes was busy, assume one CD drive.
                // Global.LogIt("CountCDs",ex.Message); 
                count = 1;
            }
            return count;
        }

        public bool NoCD()
        {
            return (CountCDs() == 0);
        }

        public TrackList GetCDTracks()
        {
            var tracklist = new TrackList();
            IITPlaylist cdPlaylist = GetCDPlaylist();
            if (cdPlaylist != null)
            {
                foreach (IITFileOrCDTrack track in cdPlaylist.Tracks)
                {
                    tracklist.Add(track);
                }
            }
            return tracklist;
        }

        public string GetEncoders()
        {
            string encodelist = "";
            foreach (IITEncoder aCoder in this.Encoders)
            {
                encodelist += aCoder.Name + ";\r\n";
            }
            return encodelist;
        }

        public bool SetEncoder(string EncoderName)
        {
            try
            {
                // is the current encoder the one we want?
                if (this.CurrentEncoder.Format == EncoderName) { return true; }
                // if not, have to search for it
                foreach (IITEncoder aCoder in this.Encoders)
                {
                    if (aCoder.Format == EncoderName)
                    {
                        this.CurrentEncoder = aCoder;
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool SetAACEncoder()
        {
            try
            {
                // is the current encoder the one we want?
                if (this.CurrentEncoder.Format.IndexOf("AAC") >= 0) { return true; }
                // if not, have to search for it
                foreach (IITEncoder aCoder in this.Encoders)
                {
                    if (aCoder.Format.IndexOf("AAC") >= 0)
                    {
                        this.CurrentEncoder = aCoder;
                        return true;
                    }
                }
                Global.LogIt("SetAACEncoder", "Unable to set AAC encoder.\r\n");
                return false;
            }
            catch
            {
                Global.LogIt("SetAACEncoder", "Unable to set AAC encoder.\r\n");
                return false;
            }
        }

        private bool SetMP3Encoder()
        {
            try
            {
                // is the current encoder the one we want?
                if (this.CurrentEncoder.Format.IndexOf("MP3") >= 0) { return true; }
                // if not, have to search for it
                foreach (IITEncoder aCoder in this.Encoders)
                {
                    if (aCoder.Format.IndexOf("MP3") >= 0)
                    {
                        this.CurrentEncoder = aCoder;
                        return true;
                    }
                }
                Global.LogIt("SetMP3Encoder", "Unable to set MP3 encoder.\r\n");
                return false;
            }
            catch
            {
                Global.LogIt("SetMP3Encoder", "Unable to set MP3 encoder.\r\n");
                return false;
            }
        }


        public bool ConvertFileToAAC(string aFile)
        {
            if (SetAACEncoder())
            {
                try
                {
                    AACConvertStatus = this.ConvertFile2(aFile);
                    AACConvertStatus.OnConvertOperationCompleteEvent += AACConvertStatus_OnConvertOperationCompleteEvent;
                    AACConvertStatus.OnConvertOperationStatusChangedEvent += AACConvertStatus_OnConvertOperationStatusChangedEvent;
                    return true;
                }
                catch
                {
                    Global.LogIt("ConvertFileToAAC", "Unable to begin conversion of file " + aFile + ". (iTunes.ConvertFile2 failed).\r\n");
                    return false;
                }
            }
            return false;
        }

        public bool ConvertFileToMP3(string aFile)
        {
            if (SetMP3Encoder())
            {
                try
                {
                    MP3ConvertStatus = this.ConvertFile2(aFile);
                    MP3ConvertStatus.OnConvertOperationCompleteEvent += MP3ConvertStatus_OnConvertOperationCompleteEvent;
                    MP3ConvertStatus.OnConvertOperationStatusChangedEvent += MP3ConvertStatus_OnConvertOperationStatusChangedEvent;
                    return true;
                }
                catch
                {
                    Global.LogIt("ConvertFileToMP3", "Unable to begin conversion of track.\r\n");
                    return false;
                }
            }
            return false;
        }

        public bool ConvertTrackToAAC(IITTrack track)
        {
            if (SetAACEncoder())
            {
                try
                {
                    object obj = track;
                    AACConvertStatus = this.ConvertTrack2(ref obj);
                    AACConvertStatus.OnConvertOperationCompleteEvent += AACConvertStatus_OnConvertOperationCompleteEvent;
                    AACConvertStatus.OnConvertOperationStatusChangedEvent += AACConvertStatus_OnConvertOperationStatusChangedEvent;
                    return true;
                }
                catch
                {
                    Global.LogIt("ConvertTrackToAAC", "Unable to begin conversion of track.\r\n");
                    return false;
                }
            }
            return false;
        }

        public bool ConvertTrackToMP3(IITTrack track)
        {
            if (SetMP3Encoder())
            {
                try
                {
                    object obj = track;
                    MP3ConvertStatus = this.ConvertTrack2(ref obj);
                    MP3ConvertStatus.OnConvertOperationCompleteEvent += MP3ConvertStatus_OnConvertOperationCompleteEvent;
                    MP3ConvertStatus.OnConvertOperationStatusChangedEvent += MP3ConvertStatus_OnConvertOperationStatusChangedEvent;
                    return true;
                }
                catch
                {
                    Global.LogIt("ConvertTrackToMP3", "Unable to begin conversion of track.\r\n");
                    return false;
                }
            }
            return false;
        }

        public bool RipCDTrackToMP3(IITFileOrCDTrack aTrack)
        {
            if (SetMP3Encoder())
            {
                try
                {
                    Object o = aTrack;
                    MP3ConvertStatus = this.ConvertTrack2(ref o);
                    MP3ConvertStatus.OnConvertOperationCompleteEvent += MP3ConvertStatus_OnConvertOperationCompleteEvent;
                    MP3ConvertStatus.OnConvertOperationStatusChangedEvent += MP3ConvertStatus_OnConvertOperationStatusChangedEvent;
                    return true;
                }
                catch
                {
                    Global.LogIt("RipCDTrackToMP3", "Unable to begin conversion of track. (iTunes.ConvertTrack2 failed).\r\n");
                    return false;
                }
            }
            return false;
        }

        void MP3ConvertStatus_OnConvertOperationCompleteEvent()
        {
            MP3ConvertedAll(MP3ConvertStatus);
//            MP3ConvertStatus = null;
        }

        void MP3ConvertStatus_OnConvertOperationStatusChangedEvent(string trackName, int progressValue, int maxProgressValue)
        {
            ReportProgress(trackName, progressValue, maxProgressValue);
        }

        void AACConvertStatus_OnConvertOperationStatusChangedEvent(string trackName, int progressValue, int maxProgressValue)
        {
            ReportProgress(trackName, progressValue, maxProgressValue);
        }

        void AACConvertStatus_OnConvertOperationCompleteEvent()
        {
            AACConvertedAll(AACConvertStatus);
//            AACConvertStatus = null;
        }

        public void StopConversion()
        {
            if (MP3ConvertStatus != null)
            {
                MP3ConvertStatus.StopConversion();
            }
            if (AACConvertStatus != null)
            {
                AACConvertStatus.StopConversion();
            }
        }

        public bool AACConvertInProgress()
        {
            if ((AACConvertStatus != null) && (AACConvertStatus.InProgress))
            { return true; }
            return false;
        }

        public bool MP3ConvertInProgress()
        {
            if ((MP3ConvertStatus != null) && (MP3ConvertStatus.InProgress))
            { return true; }
            return false;
        }

        public bool AnyConvertInProgress()
        {
            return (AACConvertInProgress() || MP3ConvertInProgress());
        }

        public IITTrack AddFileToLibrary(string aFile, int waitSecs)
        {
            try
            {
                IITOperationStatus myOpStatus;

                if (this.LibraryPlaylist == null)
                {
                    return null;
                }

                try
                {
                    myOpStatus = this.LibraryPlaylist.AddFile(aFile);
                }
                catch
                {
                    Global.LogIt("AddFileToLibrary - .AddFile call failed on " + aFile);
                    return null;
                }

                //there's no 'AddComplete' event in iTunes, so have to wait and keep testing.
                int tries = 0;

                while ((myOpStatus != null) && (myOpStatus.InProgress) && (tries < (waitSecs * 10)) )
                {
                    Global.WaitSecs(0.1);
                    tries++;
                    Waiting(tries);
                }

                if ((myOpStatus != null) && (myOpStatus.Tracks != null))
                    return myOpStatus.Tracks[1];

                Global.LogIt("AddFileToLibrary - Unable to add file " + aFile + " to Library after " + waitSecs + " seconds");
                return null;
            }
            catch(Exception ex)
            {
                Global.LogIt("AddFileToLibrary - Unable to add file " + aFile + " to Library: " + ex.Message);
                return null;
            }
        }

        //public IITTrack AddFileToLibrary(string aFile, string anAlbum, string anArtist, string aGenre, int aDiscNum, int aTrackNum, int waitSecs)
        //{
        //    IITTrack retTrack = AddFileToLibrary(aFile, waitSecs);
        //    if (retTrack == null)
        //        return null;

        //    try //file may be read-only, so can't change data
        //    {
        //        retTrack.Album = anAlbum;
        //        retTrack.Name = anAlbum + " " + aTrackNum.ToString("00");
        //        retTrack.Artist = anArtist;
        //        retTrack.Genre = aGenre;
        //        retTrack.TrackNumber = aTrackNum;
        //        retTrack.DiscNumber = aDiscNum;
        //    }
        //    catch //still want to return track
        //    {
        //        //do nowt
        //    }

        //    return retTrack;
        //}

        public static string GetMusicFolder(string defaultITunesFolder)
        {
            StreamReader iTunesXML;
            string FileName = defaultITunesFolder + "iTunes Music Library.xml";
            if (!File.Exists(FileName))
                return "";
            try
            {
                iTunesXML = File.OpenText(FileName);
                string thisLine;
                int n = 0;
                bool found = false;
                thisLine = iTunesXML.ReadLine();
                while ((!found) && (n < 100))
                {
                    found = (thisLine.IndexOf("<key>Music Folder</key>") >= 0);
                    if (!found)
                    {
                        thisLine = iTunesXML.ReadLine();
                        n++;
                    }
                }
                if (found)
                {
                    //next line should be the folder
                    thisLine = thisLine.Substring(thisLine.IndexOf("<string>"));
                    thisLine = thisLine.Trim();
                    thisLine = thisLine.Replace("<string>file://localhost/", "");
                    thisLine = thisLine.Replace("</string>", "");
                    thisLine = thisLine.Replace("/", @"\");
                    iTunesXML.Close();
                    return thisLine;
                }
                
                iTunesXML.Close();
                return "";
            }
            catch
            {
                return "";
            }
        }

        public bool RipCDTrackToAAC(IITFileOrCDTrack aTrack)
        {
            if (SetAACEncoder())
            {
                try
                {
                    Object o = aTrack;
                    AACConvertStatus = ConvertTrack2(ref o);
                    AACConvertStatus.OnConvertOperationCompleteEvent += AACConvertStatus_OnConvertOperationCompleteEvent;
                    AACConvertStatus.OnConvertOperationStatusChangedEvent += AACConvertStatus_OnConvertOperationStatusChangedEvent;
                    return true;
                }
                catch
                {
                    Global.LogIt("RipCDTrackToAAC", "Unable to begin conversion of track. (iTunes.ConvertTrack2 failed).\r\n");
                    return false;
                }
            }
            return false;
        }
    }
}
