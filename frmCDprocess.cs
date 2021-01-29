using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using iTunesLib;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmCDprocess : Form
    {
        private TrackList myCDtracks;

        private int myDoneCount = -1;

        private iTunesApp myITunes;

        public frmCDprocess()
        {
            InitializeComponent();
        }

        private void frmCDprocess_Load(object sender, EventArgs e)
        {
            //Global.RippedTracks = new TrackList();
            Global.RippedTracks = new TrackInfoList();
            chkAllowTrackSelection.Checked = Global.Options.AllowTrackSelection;
        }

        private void AskForNextDisc(int discNum)
        {
            butOK.Show();
            chkAllowTrackSelection.Show();
            
            Activate();
            WindowState = FormWindowState.Normal;

            Application.DoEvents();

            if (Global.ProcessingFirstCd)
            {
                Global.ProcessingFirstCd = false;
            }
            else //don't want sound on first CD
            {
                PlayCompleteSound();
            }

            Global.EjectCD(Global.Options.CDDrive);

            progDisc.Value = 0;

            progBook.Value = discNum - 1;
            labDiscNum.Text = Resources.discnum__of__totdiscs;
            labDiscNum.Text = labDiscNum.Text.Replace("<discnum>",discNum.ToString());
            labDiscNum.Text = labDiscNum.Text.Replace("<totdiscs>", Global.RippedCDFiles.TotalDiscs.ToString());

            labPrompt.Text =
                Resources.Please_put_Disc__discnum__of_the_set_into_your_CD_drive;
            labPrompt.Text = labPrompt.Text.Replace("<discnum>", discNum.ToString());

        }

        private SoundPlayer myMplayer;

        private void PlayCompleteSound()
        {
            if (Global.Options.PlaySoundOnDiscComplete)
            {
                if (File.Exists(Global.Options.DiscCompleteSound))
                {
                    try
                    {
                        myMplayer = new SoundPlayer();
                        myMplayer.Open(Global.Options.DiscCompleteSound);
                        myMplayer.Play();

                        //put this thread to sleep to allow audio file to play for a little while.
                        int sleeptime = (int) myMplayer.AudioLength;
                        if (sleeptime > 10000)
                            sleeptime = 10000; //ten secs maximum

                        System.Threading.Thread.Sleep(sleeptime);
                    }
                    catch (Exception)
                    {
                        //do nowt
                    }
                }
            }
        }

        private void butOK_Click(object sender, EventArgs e)
        {
            if (myMplayer != null)
            {
                myMplayer.Close();
                myMplayer = null;
            }
            ProcessOneDisc();
        }

        private void ProcessOneDisc()
        {
            butOK.Hide();
            chkAllowTrackSelection.Hide();

            labPrompt.Text = Resources.Processing_tracks;
            try
            {
                if (ReadCDtracks())
                {
                    if (Global.Options.AllowTrackSelection)
                    {
                        var F = new frmSelectTracks(myCDtracks);
                        var result = F.ShowDialog();
                        //reset this, may have been changed by the dialog
                        chkAllowTrackSelection.Checked = Global.Options.AllowTrackSelection;
                        if (result == DialogResult.Cancel)
                        {
                            CancelOut();
                            return;
                        }
                    }

                    if (myCDtracks.Count > 0)
                    {
                        myDoneCount = 0;
                        progDisc.Maximum = myCDtracks.Count;
                        progDisc.Value = 0;

                        //we will loop through all tracks, but need to wait for iTunes event to go on to next track.
                        Global.LogImmediate("Started Disc", Global.CurrentDiscNum.ToString());

                        RipATrack(myCDtracks[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.LogIt("ProcessOneDisc", ex.Message);
            }
        }

        private void RipATrack(IITFileOrCDTrack track)
        {
            timerPoll.Stop();

            if (Global.Options.MuteSoundWhileRipping)
            {
                SoundPlayer.MuteVolume();
            }

            try
            {
                progTrack.Maximum = 100;
                progTrack.Value = 0;

                labPrompt.Text = Resources.Processing_track_trackname;
                labPrompt.Text = labPrompt.Text.Replace("<trackname>", track.Name);

                Application.DoEvents();

                tryAgainRepeats = 5;
                int duration = track.Duration; //in secs
                timerPoll.Interval = duration * 1000; //should be able to rip in less than its play duration
                timerPoll.Start();
                switch (Global.Options.Encoder)
                {
                    case Global.EncodeToTypes.MP3:
                        myITunes.RipCDTrackToMP3(track);
                        break;
                    case Global.EncodeToTypes.AAC:
                        myITunes.RipCDTrackToAAC(track);
                        break;
                }
            }
            catch (Exception ex)
            {
                Global.LogIt("RipATrack",ex.Message);
                AskIfTryAgain(ex.Message);
            }
        }

        private bool ReadCDtracks()
        {
            DisableButtons();
            Global.ResumeDisc = Global.CurrentDiscNum;
            Global.SaveSetting("ResumeDisc", Global.ResumeDisc.ToString());

            while (true) //a return gets us out of this loop.
            {
                myCDtracks = myITunes.GetCDTracks();

                if ((myCDtracks != null) && (myCDtracks.Count != 0))
                {
                    EnableButtons();
                    return true;
                }

                //try again?            
                if (MessageBox.Show(
                       Resources.No_CD_tracks_could_be_read + "\r\n\r\n" + Resources.Try_again_query,
                        Resources.No_tracks, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    EnableButtons();
                    return false;
                }
            }
        }

        private void EnableButtons()
        {
            butOK.Enabled = true;
            butCancel.Enabled = true;
        }

        private void DisableButtons()
        {
            butOK.Enabled = false;
            butCancel.Enabled = false;
        }


        private bool OpenITunes()
        {
            labPrompt.Text = Resources.Please_wait__opening_iTunes;

            if (TryToOpenITunes())
            {
                return true;
            }

            if (MessageBox.Show(Resources.iTunes_is_not_responding_to_MarkAble_control + "\r\n\r\n" + Resources.Try_again_query, Resources.Unable_to_open_iTunes, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == DialogResult.Yes)
            {
                if (TryToOpenITunes())
                {
                    return true;
                }

                MessageBox.Show(Resources.iTunes_is_still_not_responding, Resources.iTunes_won_t_respond, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return false;
            }
            return false;
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
                myITunes.Waiting += myITunes_Waiting;
                myITunes.ReportProgress += myITunes_ReportProgress;
                myITunes.iTune.OnCOMCallsDisabledEvent += OnDisabled;
                myITunes.iTune.OnCOMCallsEnabledEvent += OnEnabled;
                Cursor = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                Global.LogIt(ex.Message);
                return false;
            }
        }

        void myITunes_Waiting(int count)
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

        private delegate void AskIfTryAgainHandler(string message);
        private void AskIfTryAgain(string message)
        {
            timerPoll.Stop();

            DialogResult result = MessageBox.Show(message + "\r\n" + Resources.Try_again_query, "Problem", MessageBoxButtons.YesNo,
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
                RipATrack(myCDtracks[0]);
            }
        }

        private void TryingAgain(string message)
        {
            timerPoll.Stop();

            tryAgainRepeats--;

            if (tryAgainRepeats >= 0)
            {
                labPrompt.Text = message;
                Application.DoEvents();
                RipATrack(myCDtracks[0]);
            }
            else
            {
                AskIfTryAgain(message);
            }
        }

        void myITunes_MP3ConvertComplete(iTunesConvertOperationStatus status)
        {
            var trackHandler = new FinishedTrackHandler(FinishedTrack);
            var showMessage = new ShowMessageHandler(ShowMessage);
            var askIfTryAgain = new AskIfTryAgainHandler(AskIfTryAgain);

            if (status != null)
            {
                if ((status.Tracks != null) && (status.Tracks.Count > 0))
                {
                    try
                    {
                        var track = (IITFileOrCDTrack)status.Tracks[1];
                        if (track != null)
                        {
                            if ((track.Duration > 0) && (track.Duration < 2840000)) //??? weird number 2843833 returned from empty track
                            {
                                BeginInvoke(trackHandler, new object[] {track});
                            }
                            else
                            {
                                Global.LogImmediate("myITunes_MP3ConvertComplete", "iTunes returned an empty track");
                                BeginInvoke(askIfTryAgain, new object[] { "iTunes returned an empty track" });
                            }
                        }
                        else
                        {
                            Global.LogImmediate("myITunes_MP3ConvertComplete", "iTunes returned a null track");
                            BeginInvoke(askIfTryAgain, new object[] { "iTunes returned a null track" });
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
                    this.BeginInvoke(askIfTryAgain, new object[] { "Error on MP3 convert: " + "Status tracks were null or no tracks" });
                }
            }
            else
            {
                Global.LogImmediate("myITunes_MP3ConvertComplete", "Status was null");
                this.BeginInvoke(askIfTryAgain, new object[] { "Error on MP3 convert: " + "Status was null" });
            }
        }

        private delegate void FinishedTrackHandler(IITFileOrCDTrack track);
        void FinishedTrack(IITFileOrCDTrack convertedTrack)
        {
            timerPoll.Stop();

            //capture possibly transient info about the track, in case iTunes deletes the reference
            TrackInfo trackinf = new TrackInfo();
            trackinf.Location = convertedTrack.Location;
            trackinf.Duration = convertedTrack.Duration;
            trackinf.Name = convertedTrack.Name;
            trackinf.SourceID = convertedTrack.sourceID;
            trackinf.PlaylistID = convertedTrack.playlistID;
            trackinf.TrackID = convertedTrack.trackID;
            trackinf.DatabaseID = convertedTrack.TrackDatabaseID;


            if (Global.Options.MuteSoundWhileRipping)
            {
                SoundPlayer.RestoreVolume();
            }

            WaitSecs(2);

            try
            {
                convertedTrack.Artist = Global.RippedCDFiles.Author;
                convertedTrack.Album = Global.RippedCDFiles.BookTitle;
                trackinf.Location = convertedTrack.Location;
                Global.LogImmediate("FinishedTrack", convertedTrack.Name);
            }
            catch (Exception ex)
            {      
               Global.LogIt("FinishedTrack","Unable to update track metadata:" + ex.Message);
            }

            try
            {
                if (convertedTrack.Duration > 5000) //longer than 83 minutes, must be something wrong
                {
                    Global.LogIt("FinishedTrack", "Invalid track duration on disc " + Global.CurrentDiscNum.ToString() + " : " + convertedTrack.Duration.ToString("#,##0") + " seconds!");

                    MessageBox.Show(
                        Resources.Invalid_track_duration__skipping_track + convertedTrack.TrackNumber.ToString(), "FinishedTrack");
                    return;
                }
            }
            catch (Exception ex)
            {
                 Global.LogIt("FinishedTrack", ex.Message);
                //do nowt
            }

            Global.RippedTracks.Add(trackinf);

            myDoneCount++;
            progDisc.Value = myDoneCount < progDisc.Maximum ? myDoneCount : progDisc.Maximum;

            if (myCDtracks.Count > 0)
            {
                //remove the first on the list
                myCDtracks.RemoveAt(0);
            }

            if (myCDtracks.Count > 0)
            {
                //now do next track.
                RipATrack(myCDtracks[0]);
            }
            else
            {
                progDisc.Value = progDisc.Maximum;
                Global.LogImmediate("Finished Disc", string.Format("{0} of {1}", Global.CurrentDiscNum, Global.RippedCDFiles.TotalDiscs));
                Global.CurrentDiscNum++;
                if (Global.CurrentDiscNum <= Global.RippedCDFiles.TotalDiscs)
                {
                    AskForNextDisc(Global.CurrentDiscNum);

                    if (!TransferRippedFiles())
                    {
                        WindowState = FormWindowState.Normal;
                        DialogResult = DialogResult.Cancel;
                    }
                }
                else
                {
                    PlayCompleteSound();

                    //we are really all done!
                    Global.EjectCD(Global.Options.CDDrive);

                    WindowState = FormWindowState.Normal;
                    DialogResult = TransferRippedFiles() ? DialogResult.OK : DialogResult.Cancel;
                }
            }
        }

        private void WaitSecs(double secs)
        {
            int millisecs = Convert.ToInt32(secs * 1000);
            Application.DoEvents();
            System.Threading.Thread.Sleep(millisecs);
            Application.DoEvents();
        }

        private bool TransferRippedFiles()
        {
            int tracknum = 1;

            if (Global.Options.DebugMode)
            {
                Global.LogImmediate("TransferRippedFiles", "Transferring " + Global.RippedTracks.Count.ToString() + " converted tracks.");
            }

            string itlocation = "";

            foreach (TrackInfo rippedTrack in Global.RippedTracks)
            {
                if (Global.Options.DebugMode)
                {
                    Global.LogImmediate("TransferRippedFiles","Track " + tracknum.ToString());
                }
                
                try
                {
                    itlocation = rippedTrack.Location;
                    if (Global.Options.DebugMode)
                    {
                        Global.LogImmediate("TransferRippedFiles", "Source Location: " + itlocation);
                    }
                    if (!File.Exists(itlocation))
                    {
                        try
                        {
                            var wantedTrack = myITunes.iTune.GetITObjectByID(rippedTrack.SourceID, rippedTrack.PlaylistID, rippedTrack.TrackID, rippedTrack.DatabaseID) as IITFileOrCDTrack;
                            if (wantedTrack != null)
                            {
                                itlocation = wantedTrack.Location;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(Resources.Failed_to_transfer_file + " :\r\n\r\n" + ex.Message, "Error");
                    return false;
                    
                } 
                var inf = new FileInfo(itlocation);
                var ext = Global.GetExtension(itlocation);

                //note CurrentDiscNum is 'off by one' because we've already incremented it for next disc.

                var frec = new CDTrackRecord
                               {
                                   Duration = new TimeSpan(rippedTrack.Duration*TimeSpan.TicksPerSecond),
                                   Size = inf.Length,
                                   FileType =
                                       ext.Equals("mp3", StringComparison.OrdinalIgnoreCase)
                                           ? Global.FileTypes.MP3
                                           : Global.FileTypes.AAC,
                                   FilePath =
                                       (Global.Options.RipFolder + @"\" +
                                        Global.SafeFile(Global.RippedCDFiles.BookTitle) +
                                        " - " + "Disc " + (Global.CurrentDiscNum - 1).ToString("00") + " Track " +
                                        tracknum.ToString("00") + "." + Global.GetExtension(itlocation)),
                                   DiscNumber = Global.CurrentDiscNum - 1,
                                   TrackNumber = tracknum,
                                   ChapterName = rippedTrack.Name
                               };

                if (Global.Options.DebugMode)
                {
                    Global.LogImmediate("TransferRippedFiles", "Moving " + itlocation + " to " + frec.FilePath);
                }

                myITunes.iTune.Stop(); //just in case it's trying to play the track

                try
                {
                    File.Copy(itlocation, frec.FilePath,true);
                }
                catch (Exception ex)
                {
                    //failing to copy the file is fatal

                    MessageBox.Show(Resources.Failed_to_transfer_file + ":" +frec.FilePath + ".\r\n\r\n" + ex.Message,
                                    Resources.Error_exclaim);

                    Global.LogImmediate("TransferRippedFiles", "Unable to transfer converted file " + frec.FilePath + ":" + ex.Message);
                    return false;
                }

                try
                {
                    File.Delete(itlocation);
                }
                catch (Exception ex)
                {
                    //failing to delete the original converted file is messy but not fatal.

                    Global.LogImmediate("TransferRippedFiles", "Unable to delete converted file " + frec.FilePath + ":" + ex.Message);
                }

                Global.RippedCDFiles.Add(frec);

                //attempt to remove the converted track from iTunes library
                try
                {
                    var wantedTrack = myITunes.iTune.GetITObjectByID(rippedTrack.SourceID, rippedTrack.PlaylistID, rippedTrack.TrackID, rippedTrack.DatabaseID) as IITFileOrCDTrack;
                    if (wantedTrack != null) { wantedTrack.Delete(); }
                }
                catch
                {
                    //do nowt; iTunes sometimes dereferences track early.
                    Global.LogImmediate("TransferRippedFiles", "Unable to delete track from iTunes");
                }

                tracknum++;
            }

            if (!String.IsNullOrEmpty(itlocation))
            {
                try
                {
                    //attempt a clean up of itunes music folder
                    Global.KillEmptyDirectories(Global.GetParentPath(itlocation));
                }
                catch (Exception ex)
                {
                    Global.LogImmediate("TransferRippedFiles", "Unable to clean up iTunes folders : " + ex.Message);
                }
            }

            Global.RippedCDFiles.CurrentDisc = Global.CurrentDiscNum - 1;

            Global.WriteRippedFilelist();
            Global.RippedTracks.Clear();

            return true;
        }



        void myITunes_AACConvertComplete(iTunesConvertOperationStatus status)
        {
            var trackHandler = new FinishedTrackHandler(FinishedTrack);
            var showMessage = new ShowMessageHandler(ShowMessage);
            var askIfTryAgain = new AskIfTryAgainHandler(AskIfTryAgain);

            if (status != null)
            {
                if ((status.Tracks != null) && (status.Tracks.Count > 0))
                {
                    try
                    {
                        var track = (IITFileOrCDTrack)status.Tracks[1];
                        if (track != null)
                        {
                            if ((track.Duration > 0) && (track.Duration < 2840000)) //??? weird number 2843833 returned from empty track
                            {
                                BeginInvoke(trackHandler, new object[] { track });
                            }
                            else
                            {
                                Global.LogImmediate("myITunes_AACConvertComplete", "iTunes returned an empty track");
                                BeginInvoke(askIfTryAgain, new object[] { "iTunes returned an empty track" });
                            }
                        }
                        else
                        {
                            Global.LogImmediate("myITunes_AACConvertComplete", "iTunes returned a null track");
                            BeginInvoke(askIfTryAgain, new object[] { "iTunes returned a null track" });
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.LogImmediate("myITunes_AACConvertComplete", ex.Message);
                        BeginInvoke(showMessage, new object[] { "Error on AAC convert: " + ex.Message });
                    }
                }
                else
                {
                    Global.LogImmediate("myITunes_AACConvertComplete", "Status tracks were null or no tracks");
                    BeginInvoke(askIfTryAgain, new object[] { "Error on AAC convert: " + "Status tracks were null or no tracks" });
                }
            }
            else
            {
                Global.LogImmediate("myITunes_AACConvertComplete", "Status was null");
                BeginInvoke(askIfTryAgain, new object[] { "Error on AAC convert: " + "Status was null" });
            }
        }


 

        private delegate void ShowProgressHandler(string name, int progress, int maxprogress);
        private void ShowProgress(string name, int progress, int maxprogress)
        {
            progTrack.Maximum = maxprogress;
            progTrack.Value = progress;
        }

        private void CloseITunes()
        {
            if (myITunes != null)
                myITunes.iTune.Quit();
        }

        private void OnDisabled(ITCOMDisabledReason reason)
        {
//            ShowMessageHandler showMessage = new ShowMessageHandler(ShowMessage);
            Global.Disabled = true;
//            BeginInvoke(showMessage, new object[] { "iTunes interface was disabled!" });
        }

        private void OnEnabled()
        {
            if (Global.Disabled)
            {
                Global.Disabled = false;
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            //report stuff about resume

            CancelOut();
        }

        private void CancelOut()
        {
            MessageBox.Show(Resources.Cancelled + " " + Resources.However_you_can_resume_converting_this_book_at_a_later_time_if_you_wish, Resources.Cancelled);

            //clean up already ripped tracks from current disc.
            for (int i = 0; i < Global.RippedTracks.Count; i++)
            {
                try
                {
                    File.Delete(Global.RippedTracks[i].Location);
                }
                catch (Exception)
                {
                    //do nowt
                }
            }
            DialogResult = DialogResult.Cancel;
        }

        private bool DoneIt = false;
        private int tryAgainRepeats = 5;

        private void frmCDprocess_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;
                //Global.ResizeForDPI(this);
                labBookTitle.Text = Resources.Ripping_booktitle_by_author;
                labBookTitle.Text = labBookTitle.Text.Replace("<booktitle>", Global.RippedCDFiles.BookTitle);
                labBookTitle.Text = labBookTitle.Text.Replace("<author>", Global.RippedCDFiles.Author);

                labPrompt.Text = Resources.Please_put_Disc__discnum__of_the_set_into_your_CD_drive;
                labPrompt.Text = labPrompt.Text.Replace("<discnum>", Global.CurrentDiscNum.ToString());                
                
                
                progBook.Maximum = Global.RippedCDFiles.TotalDiscs;
                //trigger off the whole process
                timerStart.Start();
            }

        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            timerStart.Stop();
            if (OpenITunes())
            {
                AskForNextDisc(Global.CurrentDiscNum);
            }

        }

        private void timerPoll_Tick(object sender, EventArgs e)
        {
            timerPoll.Stop();

            TryingAgain(Resources.Converting_this_track_seems_to_be_taking_a_very_long_time);
        }

        private void frmCDprocess_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (myMplayer != null)
            {
                myMplayer.Close();
                myMplayer = null;
            }

            if (Global.Options.MuteSoundWhileRipping)
            {
                SoundPlayer.RestoreVolume();
            }
        }

        private void chkAllowTrackSelection_CheckedChanged(object sender, EventArgs e)
        {
            Global.Options.AllowTrackSelection = chkAllowTrackSelection.Checked;
            Global.Options.Save();
        }



    }
}