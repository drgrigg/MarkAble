using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iTunesLib;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmSeparateProcess : Form
    {

        private int myDoneCount;

        private iTunesApp myITunes;

        public frmSeparateProcess()
        {
            InitializeComponent();
        }

        private void frmSeparateProcess_Load(object sender, EventArgs e)
        {
            var prompt = new StringBuilder(Resources.Ripping_booktitle_by_author);
            prompt.Replace("<booktitle>", Global.UnRippedFiles.BookTitle);
            prompt.Replace("<author>", Global.UnRippedFiles.Author);
            labBookTitle.Text = prompt.ToString();

            prompt = new StringBuilder(Resources.We_re_now_converting_all_the_audio_files_into_fileformat_format);
            prompt.Replace("<fileformat>", Global.Options.Encoder.ToString());
            labPrologue.Text = prompt.ToString();
            Global.CurrentDiscNum = 1;
        }

        private void ConvertNextFile(string filename)
        {
            if (Global.Options.MuteSoundWhileRipping)
            {
                SoundPlayer.MuteVolume();
            }

            try
            {
                progTrack.Maximum = 100;
                progTrack.Value = 0;

                var prompt = new StringBuilder(Resources.Processing_file_path);
                prompt.Replace("<path>", Global.GetFilename(filename));
                labPrompt.Text = prompt.ToString();

                switch(Global.Options.Encoder)
                {
                    case Global.EncodeToTypes.AAC:
                        myITunes.ConvertFileToAAC(filename);
                        break;
                    case Global.EncodeToTypes.MP3:
                        myITunes.ConvertFileToMP3(filename);
                        break;
                }

                Application.DoEvents();

            }
            catch (Exception ex)
            {
                Global.LogIt("ConvertNextFile", ex.Message);
                TryAgain(ex.Message);
            }
        }

        private bool OpenITunes()
        {
            labPrompt.Text = Resources.Please_wait__opening_iTunes;

            if (TryToOpenITunes())
            {
                return true;
            }
            else //try again
            {
                if (MessageBox.Show(Resources.iTunes_is_not_responding_to_MarkAble_control, Resources.Unable_to_open_iTunes, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    if (TryToOpenITunes())
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(Resources.iTunes_is_still_not_responding, Resources.iTunes_won_t_respond, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        return false;
                    }
                }
                return false;
            }
        }

        private bool TryToOpenITunes()
        {
            //if (myITunes != null)
            //    return true;

            try
            {
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                myITunes = new iTunesApp();

                myITunes.AACConvertedAll += myITunes_AACConvertComplete;
                myITunes.MP3ConvertedAll += myITunes_MP3ConvertComplete;
                myITunes.ReportProgress += myITunes_ReportProgress;
                myITunes.Waiting += myITunes_Waiting;
                myITunes.OnCOMCallsDisabledEvent += OnDisabled;
                myITunes.OnCOMCallsEnabledEvent += OnEnabled;
                Cursor = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                Global.LogIt(ex.Message);
                return false;
            }
        }

        private void myITunes_Waiting(int count)
        {
            Application.DoEvents();
        }

        void myITunes_ReportProgress(string name, int progress, int maxprogress)
        {
            ShowProgressHandler showProgress = ShowProgress;
            BeginInvoke(showProgress, new object[] {name, progress, maxprogress});
        }


        private delegate void ShowMessageHandler(string message);
        private void ShowMessage(string message)
        {
            MessageBox.Show(message, Resources.iTunes_error);
        }

        private delegate void TryAgainHandler(string message);
        private void TryAgain(string message)
        {
            DialogResult result = MessageBox.Show(message + "\r\n" + Resources.Try_again_query, Resources.Problems, MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);
            //try again on same track
            if (result == DialogResult.Yes)
            {
                try
                {
                    if (myITunes.AnyConvertInProgress())
                        myITunes.StopConversion();
                }
                catch
                {
                    //do nowt
                }
                ConvertNextFile(Global.UnRippedFiles.GetFirstUnconverted().FilePath);
            }
        }

        void myITunes_MP3ConvertComplete(iTunesConvertOperationStatus status)
        {
            var trackHandler = new FinishedTrackHandler(FinishedTrack);
            var showMessage = new ShowMessageHandler(ShowMessage);
            var tryAgain = new TryAgainHandler(TryAgain);

            if (status != null)
            {
                if ((status.Tracks != null) && (status.Tracks.Count > 0))
                {
                    try
                    {
                        var track = (IITFileOrCDTrack)status.Tracks[1];
                        if (track != null)
                            BeginInvoke(trackHandler, new object[] { track });
                        else
                        {
                            Global.LogImmediate("myITunes_MP3ConvertComplete", "iTunes returned a null track");
                            BeginInvoke(tryAgain, new object[] { "iTunes returned a null track" });
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.LogImmediate("myITunes_MP3ConvertComplete", ex.Message);
                        this.BeginInvoke(showMessage, new object[] { "Error on MP3 convert: " + ex.Message });
                    }
                }
                else
                {
                    Global.LogImmediate("myITunes_MP3ConvertComplete", "Status tracks were null or no tracks");
                    this.BeginInvoke(tryAgain, new object[] { "Error on MP3 convert: " + "Status tracks were null or no tracks" });
                }
            }
            else
            {
                Global.LogImmediate("myITunes_MP3ConvertComplete", "Status was null");
                this.BeginInvoke(tryAgain, new object[] { "Error on MP3 convert: " + "Status was null" });
            }
        }

        private delegate void FinishedTrackHandler(IITFileOrCDTrack track);
        void FinishedTrack(IITFileOrCDTrack convertedTrack)
        {
            //grab this info as soon as possible, as iTunes may delete the track on us.
            TrackInfo trackinf = new TrackInfo();


            trackinf.Location = convertedTrack.Location;

            trackinf.Duration = convertedTrack.Duration;
            trackinf.Name = convertedTrack.Name;
            trackinf.SourceID = convertedTrack.sourceID;
            trackinf.PlaylistID = convertedTrack.playlistID;
            trackinf.TrackID = convertedTrack.trackID;
            trackinf.DatabaseID = convertedTrack.TrackDatabaseID;


            try //to update metadata, which iTunes will write into physical file
            {
                convertedTrack.Artist = Global.UnRippedFiles.Author;
                convertedTrack.Album = Global.UnRippedFiles.BookTitle;
                trackinf.Location = convertedTrack.Location;
            }
            catch (Exception ex)
            {
                Global.LogIt("FinishedTrack", "Unable to update track metadata:" + ex.Message);
            }


            if (Global.Options.MuteSoundWhileRipping)
            {
                SoundPlayer.RestoreVolume();
            }

            WaitSecs(2);

            try
            {
                convertedTrack.Artist = Global.UnRippedFiles.Author;
                convertedTrack.Album = Global.UnRippedFiles.BookTitle;
            }
            catch (Exception ex)
            {
                Global.LogIt("FinishedTrack", "Unable to update track metadata:" + ex.Message);
            }

            string itlocation = convertedTrack.Location;

            //var oldfrec = Global.UnRippedFiles.GetFirstUnconverted();
            var frec = Global.UnRippedFiles.GetFirstUnconverted();

            int tracknum = Global.ConvertedSeparateFiles.Count + 1;

            string newFilePath = "";
            
            switch(Global.Options.Encoder)
            {
                case Global.EncodeToTypes.AAC:
                    newFilePath = Global.Options.RipFolder + @"\" + Global.SafeFile(Global.UnRippedFiles.BookTitle) + " - " + tracknum.ToString("000") + ".m4a";
                    frec.FileType = Global.FileTypes.AAC;
                    break;
                case Global.EncodeToTypes.MP3:
                    newFilePath = Global.Options.RipFolder + @"\" + Global.SafeFile(Global.UnRippedFiles.BookTitle) + " - " + tracknum.ToString("000") + ".mp3";
                    frec.FileType = Global.FileTypes.MP3;
                    break;
            }

            int tried = 0;
            bool success = false;
            const int maxTryCount = 5;

            while ((success == false) && (tried < maxTryCount))
            {
                if (!File.Exists(trackinf.Location))
                {
                    try
                    {
                        var wantedTrack = myITunes.iTune.GetITObjectByID(trackinf.SourceID, trackinf.PlaylistID, trackinf.TrackID, trackinf.DatabaseID) as IITFileOrCDTrack;
                        if (wantedTrack != null)
                        {
                            trackinf.Location = wantedTrack.Location;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (tried >= maxTryCount)
                        { MessageBox.Show(ex.Message); }
                    }
                }

                try
                {
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);

                    File.Move(trackinf.Location, newFilePath);
                    success = true;
                }
                catch (Exception ex)
                {
                    WaitSecs(2);
                    tried++;

                    if (tried >= maxTryCount)
                    {
                        if (MessageBox.Show(
                                                Resources.Unable_to_move_converted_file_to_path.Replace("<path>",
                                                                                                        newFilePath) +
                                                "\r\n\r\n" + ex.Message + "\r\n\r\n" + 
                                                Resources.Try_again_query, "Error",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            tried = 0; //try again
                        }
                        else // DialogResult.NO
                        {
                            newFilePath = trackinf.Location; //give up, point to original location
                            success = true; //not really!
                            tried = 0; //also need this to escape the loop.
                        }
                    }
                }
            }

            frec.FilePath = newFilePath;
            frec.Duration = new TimeSpan(trackinf.Duration * TimeSpan.TicksPerSecond);


            try
            {
                //attempt a clean up of itunes music folder
                Global.KillEmptyDirectories(Global.GetParentPath(trackinf.Location));
            }
            catch
            {
                //do nowt
            }

           //oldfrec.Converted = true;
            frec.Converted = true;
            Global.ConvertedSeparateFiles.Add(frec);

            myDoneCount++;
            progBook.Value = myDoneCount;
            progTrack.Value = 0;

            //attempt to remove the converted track from iTunes library
            try
            {
                var wantedTrack = myITunes.iTune.GetITObjectByID(trackinf.SourceID, trackinf.PlaylistID, trackinf.TrackID, trackinf.DatabaseID) as IITFileOrCDTrack;
                if (wantedTrack != null) { wantedTrack.Delete(); }
                //rippedTrack.Track.Delete();
            }
            catch
            {
                //do nowt; iTunes sometimes dereferences track early.
                Global.LogImmediate("FinishedTrack", "Unable to delete track from iTunes");
            }


            if (Global.UnRippedFiles.AllFilesAreConverted())
            {
                //we are really all done!
                Global.WriteSeparateList(SeparateFileList.ProcessStages.UnMerged);

                //do this just to trigger the right process back in frmIntro.
                Global.UnRippedFiles.ProcessStage = SeparateFileList.ProcessStages.UnMerged;

                WindowState = FormWindowState.Normal;
                DialogResult = DialogResult.OK;
            }
            else
            {
                //do next track.
                Global.WriteSeparateList(SeparateFileList.ProcessStages.Converting);
                ConvertNextFile(Global.UnRippedFiles.GetFirstUnconverted().FilePath);
            }
        }

        private static void WaitSecs(double secs)
        {
            int millisecs = Convert.ToInt32(secs * 1000);
            Application.DoEvents();
            System.Threading.Thread.Sleep(millisecs);
            Application.DoEvents();
        }


        void myITunes_AACConvertComplete(iTunesConvertOperationStatus status)
        {
            var trackHandler = new FinishedTrackHandler(FinishedTrack);
            var showMessage = new ShowMessageHandler(ShowMessage);
            var tryAgain = new TryAgainHandler(TryAgain);

            if (status != null)
            {
                if ((status.Tracks != null) && (status.Tracks.Count > 0))
                {
                    try
                    {
                        var track = (IITFileOrCDTrack)status.Tracks[1];
                        if (track != null)
                            BeginInvoke(trackHandler, new object[] { track });
                        else
                        {
                            Global.LogImmediate("myITunes_AACConvertComplete", "iTunes returned a null track");
                            BeginInvoke(tryAgain, new object[] { "iTunes returned a null track" });
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.LogImmediate("myITunes_AACConvertComplete", ex.Message);
                        this.BeginInvoke(showMessage, new object[] { "Error on AAC convert: " + ex.Message });
                    }
                }
                else
                {
                    Global.LogImmediate("myITunes_AACConvertComplete", "Status tracks were null or no tracks");
                    this.BeginInvoke(tryAgain, new object[] { "Error on AAC convert: " + "Status tracks were null or no tracks" });
                }
            }
            else
            {
                Global.LogImmediate("myITunes_AACConvertComplete", "Status was null");
                this.BeginInvoke(tryAgain, new object[] { "Error on AAC convert: " + "Status was null" });
            }

        }

        private delegate void ShowProgressHandler(string name, int progress, int maxprogress);
        private void ShowProgress(string name, int progress, int maxprogress)
        {
            progTrack.Maximum = maxprogress;
            progTrack.Value = progress;
        }

        private void OnDisabled(ITCOMDisabledReason reason)
        {
//            ShowMessageHandler showMessage = new ShowMessageHandler(ShowMessage);
            Global.Disabled = true;
//            BeginInvoke(showMessage, new object[] {"iTunes interface was disabled!"});
        }

        private void OnEnabled()
        {
            if (Global.Disabled)
            {
                Global.Disabled = false;
            }
        }

        private bool DoneIt = false;
        private void frmSeparateProcess_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;
                var prompt = new StringBuilder(Resources.Converting_booktitle_by_author);
                prompt.Replace("<booktitle>", Global.UnRippedFiles.BookTitle);
                prompt.Replace("<author>", Global.UnRippedFiles.Author);

                labBookTitle.Text = prompt.ToString();

                Global.ConvertedSeparateFiles = new SeparateFileList();
                Global.ConvertedSeparateFiles.Clone(Global.UnRippedFiles); //just copies the metadata, not the files.

                //trigger off the whole process
                timerStart.Start();
            }

        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            timerStart.Stop();
            progBook.Maximum = Global.UnRippedFiles.Count;

            bool skipConversion = false;

            if (AllFilesAlreadyInFormat())
            {
                if (Global.ProcessMode == Global.ProcessModes.ProcessingBatch) //don't want to get the prompt
                {
                    skipConversion = Global.CurrentBatch.SkipConversion;
                }
                else
                {
                    var prompt = new StringBuilder(Resources.All_files_to_be_processed_appear_to_be_fileformat_files);
                    prompt.Replace("<fileformat>", Global.Options.Encoder.ToString());

                    string showMessage = prompt.ToString() 
                        + "\r\n\r\n" + Resources.If_all_of_these_files_have_been_encoded_the_same_way__you_can_skip_the_conversion_process 
                        + "\r\n\r\n" + Resources.Skip_conversion_query;

                   prompt = new StringBuilder(Resources.All_files_are_fileformat);
                   prompt.Replace("<fileformat>", Global.Options.Encoder.ToString());

                    string headMessage = prompt.ToString();

                    if (MessageBox.Show(showMessage,headMessage,MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        skipConversion = true;
                    }
                }
            }

            if (skipConversion)
            {
                foreach (SeparateRecord frec in Global.UnRippedFiles.Files)
                {
                    frec.Converted = true;
                    Global.ConvertedSeparateFiles.Files.Add(frec);
                }
                Global.WriteSeparateList(SeparateFileList.ProcessStages.UnMerged);

                WindowState = FormWindowState.Normal;
                DialogResult = DialogResult.OK;
            }
            else
            {
                Global.WriteSeparateList(SeparateFileList.ProcessStages.Converting);
                if (OpenITunes())
                {
                    ConvertNextFile(Global.UnRippedFiles.GetFirstUnconverted().FilePath);
                }
            }
        }

        private static bool AllFilesAlreadyInFormat()
        {
            if (
                (
                    (Global.Options.Encoder == Global.EncodeToTypes.MP3)
                    &&
                    (Global.UnRippedFiles.AllFilesAreMP3())
                )
                ||
                (
                    (Global.Options.Encoder == Global.EncodeToTypes.AAC)
                    &&
                    (Global.UnRippedFiles.AllFilesAreAAC())
                )
                )
            {
                return true;
            }
            return false;
        }


        private void frmSeparateProcess_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.Options.MuteSoundWhileRipping)
            {
                SoundPlayer.RestoreVolume();
            }
        }

        private void butCancel_Click_1(object sender, EventArgs e)
        {
            Global.WriteSeparateList(SeparateFileList.ProcessStages.Converting);
            
            //report stuff about resume
            WindowState = FormWindowState.Normal;
            MessageBox.Show(Resources.Cancelled + " " + Resources.However_you_can_resume_converting_this_book_at_a_later_time_if_you_wish,
                Resources.Cancelled);

            DialogResult = DialogResult.Cancel;
        }

    }
}
