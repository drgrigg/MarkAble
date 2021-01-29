using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AAClib;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmSeparate1 : Form
    {
        public frmSeparate1()
        {
            InitializeComponent();
        }

        private void butSelectFiles_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Global.Options.LastSourceFolder))
                dlgSelectFiles.InitialDirectory = Global.Options.LastSourceFolder;

            try
            {
                if ((dlgSelectFiles.ShowDialog() == DialogResult.OK) && (dlgSelectFiles.FileNames.Length > 0))
                {
                    string chosenfolder = Global.GetPath(dlgSelectFiles.FileNames[0]);
                    if (chosenfolder != Global.Options.LastSourceFolder)
                    {
                        Global.Options.LastSourceFolder = chosenfolder;
                        Global.Options.Save();
                    }

                    this.Cursor = Cursors.WaitCursor;
                    foreach (string filename in dlgSelectFiles.FileNames)
                    {
                        AddFileToList(filename);
                    }
                    FillList();

                    if (String.IsNullOrEmpty(Global.UnRippedFiles.BookImage))
                    {
                        var imagePath = chosenfolder;
                        Global.UnRippedFiles.BookImage = Global.GetFirstImage(imagePath);
                    }

                    this.Cursor = Cursors.Default;
                }
            }
            catch (InvalidOperationException ex)
            {
                Global.LogIt("AddFileTolist", ex.Message);

                MessageBox.Show(
                    Resources.You_may_have_selected_too_many_files_at_once + "\r\n\r\n" + Resources.Try_adding_files_in_smaller_groups_or_use_Select_Folder,
                    Resources.Error_exclaim);
            }
            catch (Exception ex)
            {
                Global.LogIt("AddFileTolist", ex.Message);
                MessageBox.Show(Resources.Error_on_opening_files + ":" + "\r\n\r\n" + ex.Message, Resources.Error_exclaim);
            }
        }


        private void AddFileToList(string filename)
        {
            var inf = new FileInfo(filename);

            var frec = new SeparateRecord {FilePath = filename, Size = inf.Length, Converted = false};

            string ext = Global.GetExtension(filename);

            string author;
            string album;
            string title;
            int discnum = 0;
            int tracknum = 0;
            string shortfile = Global.GetFilename(filename, false);
            string parent = Global.GetParentFolderName(filename);
            Descriptor desc;

            try
            {
                switch (ext)
                {
                    case "mp3":
                        frec.FileType = Global.FileTypes.MP3;
                        var thisMp3 = new mp3file();
                        if (thisMp3.OpenForRead(filename))
                        {
                            thisMp3.GetAudioData();

                            author = String.IsNullOrEmpty(thisMp3.Artist) ? Global.UnRippedFiles.Author : thisMp3.Artist;
                            album = String.IsNullOrEmpty(thisMp3.Album) ? Global.UnRippedFiles.BookTitle : thisMp3.Album;
                            title = String.IsNullOrEmpty(thisMp3.Title) ? album : thisMp3.Title;

                            if (!String.IsNullOrEmpty(thisMp3.DiscNum))
                            {
                                try
                                {
                                    //could be disc "1" or "1 of 2" or "1/2" etc
                                    string[] temp = thisMp3.DiscNum.Split(new char[] {' ','/','.',',',':','-'} );
                                    if (temp.Length > 0)
                                        discnum = Convert.ToInt32(temp[0]);
                                }
                                catch (Exception)
                                {
                                    //do nowt
                                }
                            }

                            if (!String.IsNullOrEmpty(thisMp3.Track))
                            {
                                try
                                {
                                    //could be track "1 of 22"
                                    string[] temp = thisMp3.Track.Split(' ');
                                    if (temp.Length > 0)
                                        tracknum = Convert.ToInt32(temp[0]);
                                }
                                catch (Exception)
                                {
                                    //do nowt
                                }
                            }


                            SetDescriptors(frec, album, title, author, shortfile, parent, discnum, tracknum);

                            frec.Size = thisMp3.Length;
                            frec.DurationTicks = Convert.ToInt64(thisMp3.DurationInSecs*TimeSpan.TicksPerSecond);

                            if (String.IsNullOrEmpty(Global.UnRippedFiles.BookImage))
                            {
                                try
                                {
                                    if (thisMp3.Pictures.Count > 0)
                                    {
                                        var coverImage =
                                            thisMp3.Pictures.GetImageWithType(PictureRec.PictureTypes.FrontCover) ??
                                            thisMp3.Pictures[0].Picture;

                                        var shrunkimage = ImageTools.ConstrainProportions(coverImage, Global.Options.MaxImageSize,
                                                                                          ImageTools.Dimensions.Width);
                                        if (shrunkimage == null) { throw new InvalidOperationException(); }

                                        Global.SafeImageSave(shrunkimage, Global.Options.RipFolder + @"\" + Global.SafeFile(title) + @".png", ImageFormat.Png);
										Global.UnRippedFiles.BookImage = Global.Options.RipFolder + @"\" + Global.SafeFile(title) + @".png";
                                    }
                                }
                                catch (Exception ex)
                                {    
                                    //do nowt
                                    Global.LogIt("Saving MP3 image",ex.Message);
                                    Global.UnRippedFiles.BookImage = "";
                                }
                            }

                            thisMp3.Close();
                        }
                        break;

                    case "mp2":
                        DeferDescription(shortfile, parent, frec);
                        frec.FileType = Global.FileTypes.MP2;
                        break;

                    case "wav":
                        DeferDescription(shortfile, parent, frec);
                        frec.FileType = Global.FileTypes.WAV;
                        var wav = new WAVfile(filename);
                        frec.DurationTicks = Convert.ToInt64(wav.DurationInSecs() * TimeSpan.TicksPerSecond);
                        frec.Size = wav.Length;
                        break;

                    case "m4a":
                    case "m4b":
                    case "mp4":
                        frec.FileType = Global.FileTypes.AAC;
                        //open file to get duration.
                        var aac = new AACfile();
                        aac.ReportError += aac_ReportError;
                        aac.ProcessingProgress += aac_ProcessingProgress;
                        aac.InterpretMetaData = true;
                        //aac.ReadFile(filename,false);
                        aac.ReadMovieHeader(filename);
                        if (aac.LoadedOK)
                        {
                            title = aac.PartName;
                            author = aac.Author;
                            album = aac.BookTitle;

                            SetDescriptors(frec, album, title, author, shortfile, parent, discnum, tracknum);

                            frec.Duration = aac.GetMovieDurationAsTimeSpan();

                            if (String.IsNullOrEmpty(Global.UnRippedFiles.BookImage))
                            {
                                try
                                {
                                    var coverImage = aac.GetArtwork();

                                    if (coverImage != null)
                                    {
                                        var shrunkimage = ImageTools.ConstrainProportions(coverImage, Global.Options.MaxImageSize,
                                                                                          ImageTools.Dimensions.Width);
                                        if (shrunkimage == null) { throw new InvalidOperationException(); }

                                        Global.SafeImageSave(shrunkimage, Global.Options.RipFolder + @"\bookimage.png", ImageFormat.Png);
                                        Global.UnRippedFiles.BookImage = Global.Options.RipFolder + @"\bookimage.png";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //do nowt
                                    Global.LogIt("Saving AAC image", ex.Message);
                                    Global.UnRippedFiles.BookImage = "";
                                }
                            }
                        }
                        aac.CloseFileReader();
                        aac.Dispose();
                        break;

                    case "wma":
                        frec.FileType = Global.FileTypes.WMA;
                        var wma = new wmaMetaData();
                        wma.GetFieldByName(filename, "Title", out title);
                        wma.GetFieldByName(filename, "Author", out author);
                        
                        string protect = "false";
                        wma.GetFieldByName(filename, "Is_Protected", out protect);
                        if (protect == "true")
                        {
                            frec.Protected = true;
                        }
                        desc = new Descriptor
                                   {
                                       SongTitle = String.IsNullOrEmpty(title) ? shortfile : title,
                                       Artist = String.IsNullOrEmpty(author) ? "" : author,
                                       FileName = shortfile
                        };

                        //this allows setting up a template for chapter descriptions
                        frec.ChapterName = desc.QualifyString(Global.Options.SeparateTemplate);

                        break;
                }

                Global.UnRippedFiles.Add(frec);
            }
            catch (Exception ex)
            { 
                Global.LogIt("AddFileTolist",ex.Message);
                MessageBox.Show(Resources.Error_adding_file_filename.Replace("<filename>", filename) + " :" + ex.Message, Resources.Error_exclaim);
            }
        }

        private void SetDescriptors(FileRecord frec, string album, string title, string author, string shortfile, string parent, int discnum, int tracknum)
        {
            Descriptor desc;
            desc = new Descriptor
                       {
                           SongTitle = title,
                           Artist = author,
                           Album = album,
                           FileName = shortfile,
                           ParentFolder = parent,
                           DiscNumber = discnum,
                           TrackNumber = tracknum
                       };

            if (desc.DiscNumber == 0) desc.DiscNumber = 1;
            if (desc.TrackNumber == 0) desc.TrackNumber = 1;

            //this allows setting up a template for chapter descriptions
            frec.ChapterName = desc.QualifyString(Global.Options.SeparateTemplate);

            if (String.IsNullOrEmpty(frec.ChapterName) || frec.ChapterName.Trim() == "")
                //no info found in ID3 tags, so use simple file name
            {
                frec.ChapterName = desc.QualifyString("%F");
            }
        }

        private void DeferDescription(string shortfile, string parent, FileRecord frec)
        {
            string title = shortfile;
            var desc = new Descriptor
                                  {
                                      SongTitle = title,
                                      FileName = shortfile,
                                      ParentFolder = parent
                                  };


            //this allows setting up a template for chapter descriptions
            frec.ChapterName = desc.QualifyString(Global.Options.SeparateTemplate);

            if (String.IsNullOrEmpty(frec.ChapterName) || frec.ChapterName.Trim() == "")
                //no info found in ID3 tags, so use simple file name
            {
                frec.ChapterName = desc.QualifyString("%F");
            }
        }

        void aac_ProcessingProgress(double percent, AACfile.ProcessingPhases phase)
        {
            
        }

        void aac_ReportError(string message)
        {
            Global.LogImmediate(message);
        }

        private void FillList()
        {
            lstFiles.Items.Clear();

            if (Global.UnRippedFiles.AnyFileIsProtected())
            {
                if (!ContinueToProcessProtected())
                    return;
            }

            for (int i = 0; i < Global.UnRippedFiles.Count; i++)
            {
                var frec = Global.UnRippedFiles[i];
                frec.StringType = chkFullPath.Checked ? FileRecord.StringTypes.Trimmed : FileRecord.StringTypes.FileName;
                lstFiles.Items.Add(frec);
            }
        }

        private bool ContinueToProcessProtected()
        {
            if (Global.UnRippedFiles.AllFilesAreProtected())
            {
                MessageBox.Show(Resources.All_of_the_files_you_have_selected_are_protected_by_Digital_Rights_Management + "\r\n\r\n" +
Resources.Unfortunately_ITunes_cannot_process_DRM_protected_files, Resources.Protected_files_encountered, MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                Global.UnRippedFiles.Files.Clear();
                return false;
            }

            var result =
                MessageBox.Show(
                    Resources.Some_of_the_files_you_have_selected_are_protected_by_Digital_Rights_Management 
                    + "\r\n\r\n" + Resources.Unfortunately_ITunes_cannot_process_DRM_protected_files
                    + "\r\n\r\n" + Resources.Process_just_the_unprotected_files_query,
                    Resources.Protected_files_encountered, MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                for (int i = Global.UnRippedFiles.Count -1; i >= 0; i--)
                {
                    var thisfile = (SeparateRecord) Global.UnRippedFiles[i];
                    if (thisfile.Protected)
                    {
                        Global.UnRippedFiles.RemoveAt(i);
                    }
                }
                return true;
            }
            return false;
        }

        private void chkFullPath_CheckedChanged(object sender, EventArgs e)
        {
            FillList();
        }

        private void butSelectFolder_Click(object sender, EventArgs e)
        {
            dlgSelectFolder.RootFolder = Environment.SpecialFolder.Desktop;
            if (Directory.Exists(Global.Options.LastSourceFolder))
            {
                dlgSelectFolder.SelectedPath = Global.Options.LastSourceFolder;
            }

            if (dlgSelectFolder.ShowDialog() == DialogResult.OK)
            {
                string chosenfolder = dlgSelectFolder.SelectedPath;
                if (chosenfolder != Global.Options.LastSourceFolder)
                {
                    Global.Options.LastSourceFolder = dlgSelectFolder.SelectedPath;
                    Global.Options.Save();
                }

                AddFolder(dlgSelectFolder.SelectedPath);
                FillList();
            }
        }

        //recursive routine
        private void AddFolder(string foldername)
        {

            var allfiles = Global.GetFilesMatchingPattern(foldername,
                                                          new string[]
                                                              {
                                                                  "*.mp3",
                                                                  "*.wav",
                                                                  "*m4?",
                                                                  "*.wma"
                                                              });

            foreach (var file in allfiles)
            {
                AddFileToList(file);
            }

            if (String.IsNullOrEmpty(Global.UnRippedFiles.BookImage))
            {
                var imagePath = foldername;
                Global.UnRippedFiles.BookImage = Global.GetFirstImage(imagePath);
            }
        }

        private void butNext_Click(object sender, EventArgs e)
        {
            Global.WriteSeparateList(SeparateFileList.ProcessStages.Selected);

            DialogResult = DialogResult.OK;
        }

        private void frmSeparate1_DragDrop(object sender, DragEventArgs e)
        {
            DropStuff(e);
        }

        private void frmSeparate1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void lstFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void lstFiles_DragDrop(object sender, DragEventArgs e)
        {
            DropStuff(e);
        }

        private void DropStuff(DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (files.Length == 0)
                return;

            const string audioexts = ".wav.mp3.m4a.m4b.wma.mp4";
            const string imageexts = ".jpg.png.gif.bmp";

            foreach (string filename in files)
            {
                if (File.Exists(filename))
                {
                    string ext = Global.GetExtension(filename);
                    if (audioexts.IndexOf(ext) >= 0) //it's an audio file
                    {
                        AddFileToList(filename);
                        Global.Options.LastSourceFolder = Global.GetPath(filename);
                    }
                    else
                    {
                        if (imageexts.IndexOf(ext) >= 0) //it's an image file
                        {
                            if (String.IsNullOrEmpty(Global.UnRippedFiles.BookImage))
                            {
                                Global.UnRippedFiles.BookImage = filename;
                            }
                        }
                    }
                }
                else
                {
                    //see if it's a directory name
                    if (Directory.Exists(filename))
                    {
                        AddFolder(filename);
                    }


                }
            }
            Global.Options.Save();
            FillList();
        }

        private void butRemoveSelected_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItems.Count == 0)
                return;

            for (int i = 0; i < lstFiles.SelectedItems.Count; i++)
            {
                try
                {
                    var frec = (FileRecord)lstFiles.SelectedItems[i];
                    Global.UnRippedFiles.Remove(frec);
                }
                catch
                {
                    //do nowt
                }
            }
            FillList();

        }

        private void butBatch_Click(object sender, EventArgs e)
        {
            dlgBatchFile.InitialDirectory = Global.GetBatchFolder();

            if (dlgBatchFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //FixOldBatchList(dlgBatchFile.FileName);

                    Global.CurrentBatch = (BatchList) Global.FromXmlFile(dlgBatchFile.FileName, typeof (BatchList));
                    if (Global.CurrentBatch != null && Global.CurrentBatch.Lists.Count > 0)
                    {
                        Global.ProcessMode = Global.ProcessModes.ProcessingBatch;
                        Global.CurrentBatch.SourceFile = dlgBatchFile.FileName;
                        //close this form and go into batch mode.
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show(Resources.Nothing_to_do_exclaim, Resources.No_lists_found);
                        Global.CurrentBatch = null;
                        //do nowt
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.Unable_to_open_batch_file + "\r\n" + ex.Message, Resources.Error_exclaim);
                    Global.CurrentBatch = null;
                    //do nowt
                }
            }
        }

        //this fixes batch files created before version 2.2.5 of MarkAble.  Should eventually remove this.
        private void FixOldBatchList(string fileName)
        {
            var OK = false;
            var sb = new StringBuilder();

            if (File.Exists(fileName))
            {
                using(TextReader tr = new StreamReader(fileName,Encoding.UTF8))
                {
                    var aline = tr.ReadLine();
                    while(aline != null)
                    {
                        if (aline.IndexOf("<File ") >= 0)
                        {
                            if (aline.IndexOf("SeparateRecord") >= 0)
                            {
                                OK = true;
                                break;
                            }
                            aline = aline.Replace("<File ", "<File d5p1:type=\"SeparateRecord\" xmlns:d5p1=\"http://www.w3.org/2001/XMLSchema-instance\" ");
                        }
                        sb.AppendLine(aline);
                        aline = tr.ReadLine();
                    }
                   
                    tr.Close();
                }
            }
            if (OK)
                return;

            using(TextWriter tw = new StreamWriter(fileName,false,Encoding.UTF8))
            {
                tw.Write(sb.ToString());
                tw.Flush();
                tw.Close();
            }
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            if (Global.UnRippedFiles.Count > 0)
            {
                Global.WriteSeparateList(SeparateFileList.ProcessStages.None); //cancel makes us one step behind.

                //report stuff about resume
                WindowState = FormWindowState.Normal;
                MessageBox.Show(Resources.Cancelled + " " + Resources.However_you_can_resume_converting_this_book_at_a_later_time_if_you_wish,
                    Resources.Cancelled);
            }
            DialogResult = DialogResult.Cancel;
        }

    }
}
