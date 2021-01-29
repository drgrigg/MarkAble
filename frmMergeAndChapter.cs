using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AAClib;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmMergeAndChapter : Form
    {

        public frmMergeAndChapter()
        {
            InitializeComponent();
        }
       
        //this is set to a ridiculous length unless adjusted
        private TimeSpan MaxDurationForSampleRate = new TimeSpan(50,0,0,0); //50 days

        private bool DoneIt;
        private void frmMergeAndChapter_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;
                //Global.ResizeForDPI(this);
                if (Global.Options.Encoder == Global.EncodeToTypes.AAC && Global.RippedCDFiles.Count > 0)
                {
                    //read first AAC file to check sample rate
                    var frec = Global.RippedCDFiles[0];

                    if (frec.FileType == Global.FileTypes.AAC)
                    {
                        var aacname = frec.FilePath;
                        AACfile aac = new AACfile();
                        aac.ReportError += AAC_ReportError;
                        aac.ProcessingProgress += AAC_ProcessingProgress;
                        aac.ReportMessage += AAC_ReportMessage;

                        aac.ReadFile(aacname, false);
                        if (aac.LoadedOK)
                        {
                            Global.SampleRate = aac.GetAudioTrackTimeUnitsPerSecond();
                            double maxsecs = (double)Int32.MaxValue / (double)Global.SampleRate;
                            long maxticks = Convert.ToInt64(TimeSpan.TicksPerSecond*maxsecs);
                            MaxDurationForSampleRate = new TimeSpan(maxticks);
                        }
                        aac.Dispose();
                    }
                }
                nudFiles.Maximum = Global.RippedCDFiles.Count;
                int numFiles = 1;
                while((CheckFileSize(numFiles) != FileSizeChecks.OK) && (numFiles < Global.RippedCDFiles.Count))
                {
                    Application.DoEvents();
                    numFiles++;
                }
                nudFiles.Value = numFiles;
                ShowFileSize();
            }
        }

        private void AAC_ReportMessage(string A_0, string A_1)
        {
            Application.DoEvents();
        }

        private void AAC_ProcessingProgress(double A_0, AACfile.ProcessingPhases A_1)
        {
            Application.DoEvents();
        }

        private void AAC_ReportError(string A_0)
        {
            Application.DoEvents();
        }

        private void nudFiles_ValueChanged(object sender, EventArgs e)
        {
            ShowFileSize();
        }

        private void ShowFileSize()
        {
            if ((Global.RippedCDFiles != null) && (Global.RippedCDFiles.Count > 0))
            {
                double totsize = Global.RippedCDFiles.TotalSize() / 1E6; //this should give MB
                TimeSpan totduration = Global.RippedCDFiles.TotalDuration();

                if (nudFiles.Value == 1)
                {
                    var filebreakdown = new StringBuilder(Resources.File_will_be_totsize_MB_in_size_duration_hours);
                    filebreakdown.Replace("<totsize>", totsize.ToString("#,##0.0"));
                    filebreakdown.Replace("<duration>", totduration.TotalHours.ToString("#,##0.0"));

                    labFileBreakdown.Text = filebreakdown.ToString();
                }
                else
                {
                    totsize = totsize/(double)nudFiles.Value;
                    double newticks = (double) totduration.Ticks/(double) nudFiles.Value;
                    long longticks = Convert.ToInt64(newticks);
                    totduration = new TimeSpan(longticks);

                    if (totduration > TimeSpan.Zero)
                    {
                        var filebreakdown =
                            new StringBuilder(Resources.Each_file_will_be_approx_totsize_MB_in_size_duration_duration);
                        filebreakdown.Replace("<totsize>", totsize.ToString("#,##0"));
                        filebreakdown.Replace("<duration>", totduration.TotalHours.ToString("#,##0.0"));
                        labFileBreakdown.Text = filebreakdown.ToString();
                    }
                    else
                    {
                        var filebreakdown = new StringBuilder(Resources.Each_file_will_be_approx_totsize_MB_in_size);
                        filebreakdown.Replace("<totsize>", totsize.ToString("#,##0"));
                        labFileBreakdown.Text = filebreakdown.ToString();
                    }
                }

            }
        }

        private void butStart_Click(object sender, EventArgs e)
        {
            Global.ProcessMode = Global.ProcessModes.CDs;
            Global.RippedCDFiles.NumParts = (int) nudFiles.Value;
            
            if (cboTrackInterval.SelectedItem != null)
            {
                var ti = (TrackOrFileInterval)cboTrackInterval.SelectedItem;
                Global.Options.TrackSpacing = ti.Interval;
                Global.RippedCDFiles.TrackSpacing = ti.Interval;
            }

            Global.RippedCDFiles.RegularMins = (double)nudRegMins.Value;

            if (String.IsNullOrEmpty(Global.RippedCDFiles.BookImage))
            {
                Global.RippedCDFiles.BookImage = Application.StartupPath + @"\" + "defaultimage.jpg";
            }

            if (radChapsByTime.Checked)
            {
                Global.RippedCDFiles.ChapterType = Global.ChapterTypes.ByTime;
            }

            if (radDiscOnly.Checked)
            {
                Global.RippedCDFiles.ChapterType = Global.ChapterTypes.ByTimeWithDisc;
            }

            if (radChapsByTrack.Checked)
            {
                Global.RippedCDFiles.ChapterType = Global.ChapterTypes.ByDiscAndTrack;   
            }

            if (radChapsNone.Checked)
            {
                Global.RippedCDFiles.ChapterType = Global.ChapterTypes.None;
            }

            Global.Options.ChapterType = Global.RippedCDFiles.ChapterType;
            Global.Options.Save();

            Global.WriteRippedFilelist();

            WindowState = FormWindowState.Normal;

            DialogResult = DialogResult.OK;
        }



        private void frmMergeAndChapter_Load(object sender, EventArgs e)
        {
            if (File.Exists(Global.Options.DefaultImage))
            {
                picBookImage.Load(Global.Options.DefaultImage);
            }
            else
            {
                picBookImage.Load(Application.StartupPath + @"\" + "defaultimage.jpg");
                Global.Options.DefaultImage = Application.StartupPath + @"\" + "defaultimage.jpg";
            }
            Global.RippedCDFiles.BookImage = Global.Options.DefaultImage;

            if (Global.Options.DefaultInterval > 0)
                nudRegMins.Value = Global.Options.DefaultInterval;

            if (Global.Options.Encoder == Global.EncodeToTypes.MP3)
            {
                labchap7prompt.Text =
                    Resources.MP3_files_can_t_have_chapter_stops;
                panel1.Hide();
            }
            else
            {
                labchap7prompt.Text =
                    Resources.Audiobooks_on_the_iPod_can_have_chapter_stops;
                panel1.Show();

                for (int n = 1; n <= 30; n++)
                {
                    cboTrackInterval.Items.Add(new TrackOrFileInterval(n));
                }

                if (Global.Options.TrackSpacing >= 0 && Global.Options.TrackSpacing <= 30)
                    cboTrackInterval.SelectedIndex = Global.Options.TrackSpacing - 1;

                switch (Global.Options.ChapterType)
                {
                    case Global.ChapterTypes.None:
                        radChapsNone.Checked = true;
                        break;

                    case Global.ChapterTypes.ByTime:
                        radChapsByTime.Checked = true;
                        break;

                    case Global.ChapterTypes.ByDiscAndTrack:
                    case Global.ChapterTypes.BySourceFile:
                        radChapsByTrack.Checked = true;
                        break;
                    case Global.ChapterTypes.ByTimeWithDisc:
                        radDiscOnly.Checked = true;
                        break;
                }
            }

        }

        private void butChoosePic_Click(object sender, EventArgs e)
        {
            dlgOpenImage.FileName = "";

            if (dlgOpenImage.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image imgsmall = Global.MakeSmallImage(dlgOpenImage.FileName);
                    Global.RippedCDFiles.BookImage = dlgOpenImage.FileName;
                    picBookImage.Image = imgsmall;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Resources.Unable_to_load_image_exclaim);
                }
            }
        }



        private void frmMergeAndChapter_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void frmMergeAndChapter_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[]) e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 0)
                return;

            string graphicexts = ".gif.png.jpg.bmp";

            try
            {
                string ext = Global.GetExtension(files[0]);
                if (graphicexts.IndexOf(ext) == -1)
                    return; //not a graphic file

                Image imgsmall = Global.MakeSmallImage(files[0]);
                Global.RippedCDFiles.BookImage = files[0];
                picBookImage.Image = imgsmall;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Unable_to_load_image_exclaim);
            }
        }

        private void nudFiles_Validating(object sender, CancelEventArgs e)
        {
            FileSizeChecks check = CheckFileSize((int) nudFiles.Value);

            if (check == FileSizeChecks.Critical)
            {
                DialogResult result = MessageBox.Show(Resources.Files_will_be_too_big_to_play_at_the_current_sampling_rate + "\r\n\r\n" + Resources.Ignore_this_query,
                    Resources.Critical_exclaim,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    nudFiles.Focus();
                }
                return;
            }

            if (check != FileSizeChecks.OK)
            {
                DialogResult result = MessageBox.Show(
                    Resources.Files_will_be_larger_than_recommended_size_and_or_duration + "\r\n\r\n" + Resources.Ignore_this_query,
                    Resources.Warning_exclaim,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    nudFiles.Focus();
                }
            }
        }

        private enum FileSizeChecks
        {
            OK,
            BiggerThanOptionDuration,
            BiggerThanOptionSize,
            Critical
        }

        private FileSizeChecks CheckFileSize(int NumFiles)
        {
            if ((Global.RippedCDFiles != null) && (Global.RippedCDFiles.Count > 0))
            {
                double totsize = Global.RippedCDFiles.TotalSize()/1E6; //this should give MB
                TimeSpan totduration = Global.RippedCDFiles.TotalDuration();

                totsize = totsize/(double) NumFiles;
                double newticks = (double) totduration.Ticks/(double) NumFiles;
                long longticks = Convert.ToInt64(newticks);
                totduration = new TimeSpan(longticks);

                if (totduration > MaxDurationForSampleRate)
                {
                    return FileSizeChecks.Critical;
                }

                if (totduration.TotalHours > (double) Global.Options.MaxHours)
                {
                    return FileSizeChecks.BiggerThanOptionDuration;
                }

                if
                    (totsize > (double) Global.Options.MaxSize)
                {
                    return FileSizeChecks.BiggerThanOptionSize;
                }

                return FileSizeChecks.OK;
            }
            return FileSizeChecks.OK;
        }

        private void cboTrackInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            radChapsByTrack.Checked = true;
        }

    }

}

