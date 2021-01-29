using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmSeparateMergeChap : Form
    {

        public frmSeparateMergeChap()
        {
            InitializeComponent();
        }

        //this is set to a ridiculous length unless adjusted
        private TimeSpan MaxDurationForSampleRate = new TimeSpan(50, 0, 0, 0); //50 days

        private bool DoneIt;
        private void frmSeparateMergeChap_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;

                if (Global.Options.Encoder == Global.EncodeToTypes.AAC)
                {
                    double maxsecs = (double)Int32.MaxValue / (double)Global.SampleRate;
                    long maxticks = Convert.ToInt64(TimeSpan.TicksPerSecond * maxsecs);
                    MaxDurationForSampleRate = new TimeSpan(maxticks);
                }

                if (Global.UnRippedFiles.Count > 0)
                {
                    nudFiles.Maximum = Global.UnRippedFiles.Count;
                    int numFiles = 1;
                    while ((CheckFileSize(numFiles) != FileSizeChecks.OK) && (numFiles < Global.UnRippedFiles.Count))
                    {
                        Application.DoEvents();
                        numFiles++;
                    }
                    nudFiles.Value = numFiles;
                    ShowFileSize();
      
                }
            }
        }

        private void SetBookImage()
        {
            string anImage = Global.UnRippedFiles.BookImage;
            if (!String.IsNullOrEmpty(anImage) && File.Exists(anImage))
            {
                try
                {
                    Image imgsmall = Global.MakeSmallImage(anImage);
                    picBookImage.Image = imgsmall;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Resources.Unable_to_load_image_exclaim);
                }
            }
        }

        private void nudFiles_ValueChanged(object sender, EventArgs e)
        {
            ShowFileSize();
        }

        private void ShowFileSize()
        {
            if ((Global.UnRippedFiles != null) && (Global.UnRippedFiles.Count > 0))
            {
                double totsize = Global.UnRippedFiles.TotalSize() / 1E6; //this should give MB
                TimeSpan totduration = Global.UnRippedFiles.TotalDuration();

                if (totsize > 0)
                {
                    if (nudFiles.Value == 1)
                    {
                        var filebreakdown = new StringBuilder(Resources.File_will_be_totsize_MB_in_size_duration_hours);
                        filebreakdown.Replace("<totsize>", totsize.ToString("#,##0.0"));
                        filebreakdown.Replace("<duration>", totduration.TotalHours.ToString("#,##0.0"));

                        labFileBreakdown.Text = filebreakdown.ToString();
                    }
                    else
                    {
                        totsize = totsize/(double) nudFiles.Value;
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
                else
                {
                    if (nudFiles.Value == 1)
                    {
                        var filebreakdown = new StringBuilder(Resources.File_will_have_duration_duration_hours_);
                        filebreakdown.Replace("<duration>", totduration.TotalHours.ToString("#,##0.0"));

                        labFileBreakdown.Text = filebreakdown.ToString();
                    }
                    else
                    {
                        double newticks = (double) totduration.Ticks/(double) nudFiles.Value;
                        long longticks = Convert.ToInt64(newticks);
                        totduration = new TimeSpan(longticks);

                        var filebreakdown = new StringBuilder(Resources.Each_file_will_have_duration_duration_hours);
                        filebreakdown.Replace("<duration>", totduration.TotalHours.ToString("#,##0.0"));

                        labFileBreakdown.Text = totduration > TimeSpan.Zero
                                                    ? filebreakdown.ToString()
                                                    : Resources.Unable_to_determine_file_size_and_duration;
                    }

                }
            }
        }

        private void butStart_Click(object sender, EventArgs e)
        {
            SetListParameters();
            Global.WriteSeparateList(SeparateFileList.ProcessStages.ParametersSet);

            WindowState = FormWindowState.Normal;
            DialogResult = DialogResult.OK;
        }

        private void SetListParameters()
        {
            //this version just sets all the global values to apply after conversion is
            //complete.

            Global.UnRippedFiles.NumParts = (int) nudFiles.Value;
            Global.UnRippedFiles.RegularMins = (double)nudRegMins.Value;

            if (cboFileInterval.SelectedItem != null)
            {
                var fi = (TrackOrFileInterval)cboFileInterval.SelectedItem;
                Global.Options.TrackSpacing = fi.Interval;
                Global.UnRippedFiles.TrackSpacing = fi.Interval;
            }

            if (String.IsNullOrEmpty(Global.UnRippedFiles.BookImage))
            {
                Global.UnRippedFiles.BookImage = Application.StartupPath + @"\" + "defaultimage.jpg";
            }

            if (radChapsByTime.Checked)
            {
                Global.UnRippedFiles.ChapterType = Global.ChapterTypes.ByTime;
            }

            if (radChapsByFile.Checked)
            {
                Global.UnRippedFiles.ChapterType = Global.ChapterTypes.BySourceFile;   
            }

            if (radChapsNone.Checked)
            {
                Global.UnRippedFiles.ChapterType = Global.ChapterTypes.None;
                Global.UnRippedFiles.BookImage = "";
            }

            Global.Options.ChapterType = Global.UnRippedFiles.ChapterType;
            Global.Options.Save();
        }


        private void frmSeparateMergeChap_Load(object sender, EventArgs e)
        {
            InitImage();

            InitIntervals();

            InitEncoder();

            InitChapterTypes();
        }


        private void InitChapterTypes()
        {
            switch(Global.Options.ChapterType)
            {
                case Global.ChapterTypes.None:
                    radChapsNone.Checked = true;
                    break;
                case Global.ChapterTypes.ByTime:
                    radChapsByTime.Checked = true;
                    break;
                case Global.ChapterTypes.BySourceFile:
                    radChapsByFile.Checked = true;
                    break;
                case Global.ChapterTypes.ByTimeWithDisc: //invalid choice for separate files   
                    radChapsByFile.Checked = true;
                    break;
            }
        }

        private void InitEncoder()
        {
            if (Global.Options.Encoder == Global.EncodeToTypes.MP3)
            {
                labchap7prompt.Text = Resources.MP3_files_can_t_have_chapter_stops;
                panel1.Hide();
            }
            else
            {
                labchap7prompt.Text = Resources.Audiobooks_on_the_iPod_can_have_chapter_stops;
                panel1.Show();
            }
        }

        private void InitIntervals()
        {
            for (int n = 1; n <= 30; n++)
            {
                cboFileInterval.Items.Add(new TrackOrFileInterval(n));
            }
            if (Global.Options.TrackSpacing >= 0 && Global.Options.TrackSpacing <= 30)
                cboFileInterval.SelectedIndex = Global.Options.TrackSpacing - 1;


            if (Global.Options.DefaultInterval > 0)
                nudRegMins.Value = Global.Options.DefaultInterval;
        }

        private void InitImage()
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

            if (String.IsNullOrEmpty(Global.UnRippedFiles.BookImage))
            {
                Global.UnRippedFiles.BookImage = Global.Options.DefaultImage;
            }

            SetBookImage();
        }

        private void butChoosePic_Click(object sender, EventArgs e)
        {
            dlgOpenImage.FileName = "";

            if (dlgOpenImage.ShowDialog() == DialogResult.OK)
            {
                Global.UnRippedFiles.BookImage = dlgOpenImage.FileName;
                SetBookImage();
            }
        }

        private void radChapsByFile_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmSeparateMergeChap_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void frmSeparateMergeChap_DragDrop(object sender, DragEventArgs e)
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
                Global.UnRippedFiles.BookImage = files[0];
                picBookImage.Image = imgsmall;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Resources.Unable_to_load_image_exclaim);
            }
        }

        private void nudFiles_Validating(object sender, CancelEventArgs e)
        {
            FileSizeChecks check = CheckFileSize((int)nudFiles.Value);

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
            if ((Global.UnRippedFiles != null) && (Global.UnRippedFiles.Count > 0))
            {
                double totsize = Global.UnRippedFiles.TotalSize() / 1E6; //this should give MB
                TimeSpan totduration = Global.UnRippedFiles.TotalDuration();

                totsize = totsize / (double)NumFiles;
                double newticks = (double)totduration.Ticks / (double)NumFiles;
                long longticks = Convert.ToInt64(newticks);
                totduration = new TimeSpan(longticks);

                if (totduration > MaxDurationForSampleRate)
                {
                    return FileSizeChecks.Critical;
                }

                if (totduration.TotalHours > (double)Global.Options.MaxHours)
                {
                    return FileSizeChecks.BiggerThanOptionDuration;
                }

                if
                    (totsize > (double)Global.Options.MaxSize) 
                {
                    return FileSizeChecks.BiggerThanOptionSize;
                }

                return FileSizeChecks.OK;
            }
            return FileSizeChecks.OK;
        }

        private void butBatch_Click(object sender, EventArgs e)
        {
            string BatchFolder = Global.GetBatchFolder();

            dlgBatchFile.InitialDirectory = BatchFolder;

            bool Done = false;

            while (!Done)
            {
                if (dlgBatchFile.ShowDialog() == DialogResult.OK)
                {
                    var fname = dlgBatchFile.FileName;

                    Done = SaveBatch(fname);

                    if (Done)
                    {
                        Global.ProcessMode = Global.ProcessModes.AddedToBatch;
                        WindowState = FormWindowState.Normal;
                        DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    Done = true;
                }
            }
        }

        private bool SaveBatch(string fname)
        {
            if (File.Exists(fname))
            {
                try
                {
                    Global.CurrentBatch = (BatchList)Global.FromXmlFile(fname, typeof(BatchList));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.Unable_to_open_batch_file + "\r\n" + ex.Message, Resources.Error_exclaim);
                    Global.CurrentBatch = null;
                }

                if (Global.CurrentBatch != null)
                {
                    var sb = new StringBuilder();
                    foreach (SeparateFileList fl in Global.CurrentBatch.Lists)
                    {
                        sb.AppendLine(fl.ToString());
                    }

                    if (MessageBox.Show(
                        Resources.This_batchfile_already_contains_the_following_projects + ":" + "\r\n\r\n" +
                        sb.ToString()
                        + "\r\n"
                        + Resources.Do_you_want_to_continue_query + "\r\n\r\n" +
                        Resources.If_you_say_YES__this_project_will_be_added_to_the_existing_batchfile,
                        Resources.Add_to_existing_batch_query, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                        DialogResult.No)
                    {
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(Resources.Unable_to_open_batch_file, Resources.Error_exclaim);
                    return false;
                }
            }
            else
            {
                Global.CurrentBatch = new BatchList();
            }
                
            SetListParameters();

            Global.CurrentBatch.Lists.Add(Global.UnRippedFiles);
            try
            {
                Global.ToXmlFile(fname, Global.CurrentBatch, false);
                Global.DeleteSeparateList(); //need to get rid of temporary file if it's going into a batch.

                MessageBox.Show(Resources.Project_added_to_batch, Resources.Success_exclaim);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Unable_to_save_batch + ":" + ex.Message, Resources.Error_exclaim);
                return false;
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            Global.WriteSeparateList(SeparateFileList.ProcessStages.MetaDataAdded); //if hit cancel, we're one step behind.
            
            //report stuff about resume
            WindowState = FormWindowState.Normal;
            MessageBox.Show(Resources.Cancelled + " " + Resources.However_you_can_resume_converting_this_book_at_a_later_time_if_you_wish,
                Resources.Cancelled);

            DialogResult = DialogResult.Cancel;
        }
    }

}

