using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using iTunesLib;
using AAClib;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmMergeProcess : Form
    {
        private iTunesApp myITunes;

        public frmMergeProcess()
        {
            InitializeComponent();
        }

        private void frmMergeProcess_Load(object sender, EventArgs e)
        {
            //this just hides then changes the labels so they will be correct for MP3 conversion.
            if (Global.Options.Encoder == Global.EncodeToTypes.MP3)
            {
                labWriteChapterData.Visible = false;
                labChapters.Visible = false;
                progWriteChapters.Visible = false;
                labDone.Text =
                    Resources.All_MP3_files_have_now_been_converted_to_bookmarking_format;
                labDone.AutoSize = false;
                labDone.Width = 404;
                labDone.Height = 58;
            }
        }

 

        private bool MergeAACFiles(FileListBase fileList, int listcount, int maxcount)
        {      
            string firstfile = "";

            if (fileList == null)
                return false; //nothing to do

            fileList.MergedOK = false;

            if (fileList.Count == 0)
                return false; //nothing to do

            if (fileList.Count == 1)
            {
                return ProcessSingleAAC(fileList, listcount, maxcount);  //no need for any merging
            }

            AACfile aac1, aac2;

            aac1 = new AACfile();
            aac1.ReportError += AAC_ReportError;
            aac1.ProcessingProgress += AAC_ProcessingProgress;
            aac1.ReportMessage += AAC_ReportMessage;


            firstfile = fileList[0].FilePath;
            try
            {
                aac1.ReadFile(firstfile, false);

            }
            catch (Exception ex)
            {
                var prompt = new StringBuilder(Resources.Error_reading_file_filename);
                prompt.Replace("<filename>", firstfile);

                MessageBox.Show(prompt.ToString() + "\r\n\r\n" + ex.Message, Resources.Error_exclaim);
                return false;
            } 
            
            if (aac1.LoadedOK)
            {
                aac1.RemoveAllTracksExceptAudio(); //for simplicity when we write it out again.
                aac1.SearchAndDestroy("udta"); //get rid of iTunes or Nero data
                aac1.SearchAndDestroy("free"); //get rid of iTunes or Nero data
                aac1.ChangeMovieTimeScale(Global.StandardTimeScale);

                var totalDuration = TimeSpan.Zero;
                fileList[0].CumulativeDuration = totalDuration;
                
                progMergeHeaders.Maximum = fileList.Count;

                Cursor = Cursors.WaitCursor;

                var badlist = new BadFiles();


                //this bit should get skipped if there's only one file in the list.
                for (int i = 1; i < fileList.Count; i++)
                {
                    aac2 = new AACfile();
                    aac2.ReportError += AAC_ReportError;
                    aac2.ProcessingProgress += AAC_ProcessingProgress;
                    aac2.ReportMessage += AAC_ReportMessage;

                    try
                    {
                        aac2.ReadFile(fileList[i].FilePath, false);

                    }
                    catch (Exception ex)
                    {
                        var prompt = new StringBuilder(Resources.Error_reading_file_filename);
                        prompt.Replace("<filename>", fileList[i].FilePath);

                        MessageBox.Show(prompt.ToString() + "\r\n\r\n" + ex.Message, Resources.Error_exclaim);
                        return false;
                    }  
                    
                    if (aac2.LoadedOK)
                    {
                        //note that we add export line BEFORE we merge the file, to get duration at start.
                        totalDuration = aac1.GetMovieDurationAsTimeSpan();
                        fileList[i].CumulativeDuration = totalDuration;

                        //this just merges the headers in memory, doesn't write anything to disk.
                        var result = aac1.MergeWith(aac2);
                        switch (result)
                        {
                            case AACfile.MergeResults.Success:
                                //all is well, keep going
                                break;

                            //these are just warnings
                            case AACfile.MergeResults.EmptyFile:
                                //empty file encountered, but keep going
                                Global.LogIt("MergeAACfiles", fileList[i].FilePath + " had no audio!");
                                badlist.Add(new BadFile { FileName = fileList[i].FilePath, FileProblem = result });
                                break;
                            case AACfile.MergeResults.MismatchTimeScale:
                                Global.LogIt("MergeAACfiles", "Mismatch of timescales in " + fileList[i].FilePath);
                                badlist.Add(new BadFile { FileName = fileList[i].FilePath, FileProblem = result });
                                break;
                            case AACfile.MergeResults.MismatchPreferredRate:
                                Global.LogIt("MergeAACfiles", "Mismatch of preferred rate in " + fileList[i].FilePath);
                                badlist.Add(new BadFile { FileName = fileList[i].FilePath, FileProblem = result });
                                break;
                            case AACfile.MergeResults.MismatchSampleRate:
                                Global.LogIt("MergeAACfiles", "Mismatch of sample rate in " + fileList[i].FilePath);
                                badlist.Add(new BadFile { FileName = fileList[i].FilePath, FileProblem = result });
                                break;

                            //these are fatal and we have to stop.
                            case AACfile.MergeResults.NoFilePath:
                                MessageBox.Show("Fatal Error: No file path stored for " + fileList[i].FilePath, Resources.Error_exclaim);
                                return false;
                            case AACfile.MergeResults.BadStructure:
                                MessageBox.Show("Fatal error: Bad file structure in " + fileList[i].FilePath, Resources.Error_exclaim);
                                return false;
                        }

                    }
                    progMergeHeaders.Value = i;
                    aac2.CloseFileReader();
                }


                progMergeHeaders.Value = progMergeHeaders.Maximum;

                //this actually writes the headers and weaves in the data from all the source fileList.

                string mergedname = GetMergedName(listcount, maxcount, fileList.BookTitle, ".m4b");

                //set meta-data
                SetAACMetaData(fileList, listcount, aac1);

                aac1.WriteMergedFile(mergedname);
                fileList.MergedPath = mergedname;

                aac1.CloseFileReader();
                aac1.CloseFileWriter();
                aac1.Dispose();

                //show the warnings, if any.
                if (Global.ProcessMode != Global.ProcessModes.ProcessingBatch)
                {
                    if (badlist.Count > 0)
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine(Resources.Some_problems_were_encountered_during_merging);
                        sb.AppendLine();
                        foreach (var badfile in badlist)
                        {
                            sb.AppendLine(badfile.ToString());
                        }
                        sb.AppendLine();
                        sb.AppendLine(Resources.However_processing_continued_and_the_merged_file_may_be_OK);
                        MessageBox.Show(sb.ToString(), Resources.Problems);
                    }
                }

                fileList.MergedOK = true;
                Cursor = Cursors.Default;
                return true;
            }

            // else return error
            Cursor = Cursors.Default;
            fileList.MergedOK = false;

            MessageBox.Show(Resources.Unable_to_load_initial_file, Resources.Error_exclaim);
            return false;
        }

        private bool ProcessSingleAAC(FileListBase fileList, int listcount, int maxcount)
        {
            string firstfile;
            firstfile = fileList[0].FilePath;
            string mergedname = GetMergedName(listcount, maxcount, fileList.BookTitle, ".m4b");

            try
            {
                var aac = new AACfile();
                aac.ReportError += AAC_ReportError;
                aac.ProcessingProgress += AAC_ProcessingProgress;
                aac.ReportMessage += AAC_ReportMessage;

                aac.InterpretMetaData = true;
                aac.ReadFile(firstfile, true);
                if (aac.LoadedOK)
                {
                    SetAACMetaData(fileList, listcount, aac);
                    aac.SaveFile(mergedname);
                }
                else
                {
                    throw new SystemException();
                }
                fileList.MergedPath = mergedname;
                fileList.MergedOK = true;
                return true;
            }
            catch (Exception ex)
            {
                var prompt = new StringBuilder(Resources.Error_copying_file_filename);
                prompt.Replace("<filename>", firstfile);

                MessageBox.Show(prompt.ToString() + "\r\n\r\n" + ex.Message, Resources.Error_exclaim);
                fileList.MergedOK = false;
                return false;
            }
        }

        private void SetAACMetaData(FileListBase fileList, int listcount, AACfile aac1)
        {
            aac1.BookTitle = Global.SafeFile(fileList.BookTitle);
            aac1.Author = Global.SafeFile(fileList.Author);
            aac1.PartName = Global.ListsToMerge.Count > 1
                                ? Global.SafeFile(fileList.BookTitle) + " " + Global.PadZero(listcount, Global.ListsToMerge.Count)
                                : Global.SafeFile(fileList.BookTitle);
            aac1.Genre = Global.Options.DefaultGenre;
            aac1.Description = "Audiobook created by MarkAble";

            if (!String.IsNullOrEmpty(fileList.BookImage) && File.Exists(fileList.BookImage))
            {
                Image coverImage = Global.MakeSmallImage(fileList.BookImage);

                if (coverImage != null)
                {
                    aac1.CoverArtwork = coverImage;
                }
            }
        }

        private static string GetMergedName(int listcount, int maxcount, string title, string ext)
        {
            string mergedname;
            if (maxcount == 1)
            {
                mergedname = Global.Options.MergeFolder + @"\" + Global.SafeFile(title);
            }
            else if (maxcount >= 2 & maxcount <= 9)
            {
                mergedname = Global.Options.MergeFolder + @"\" + Global.SafeFile(title) + " " +
                             listcount.ToString("0");
            }
            else if (maxcount >= 10 && maxcount <= 99)
            {
                mergedname = Global.Options.MergeFolder + @"\" + Global.SafeFile(title) + " " +
                             listcount.ToString("00");
            }
            else
            {
                mergedname = Global.Options.MergeFolder + @"\" + Global.SafeFile(title) + " " +
                             listcount.ToString("000");
            }

            mergedname += ext;
            return mergedname;
        }

        private void AAC_ReportMessage(string activity, string message)
        {
            //MessageBox.Show(activity + ":" + message);
            Global.LogIt(activity + ":" + message);
        }

        private void AAC_ProcessingProgress(double percentage, AACfile.ProcessingPhases phase)
        {
            if ((percentage > 100) || (percentage < 0))
            {
                Application.DoEvents();
                return;
            }

            switch(phase)
            {
                case AACfile.ProcessingPhases.None:
                    break;
                case AACfile.ProcessingPhases.ReadingFile:
                    break;
                case AACfile.ProcessingPhases.ConstructingHeader:
                    progWriteChapters.Value = (int) percentage;
                    break;
                case AACfile.ProcessingPhases.WritingHeader:
                    progWriteChapters.Value = (int)percentage;
                    break;
                case AACfile.ProcessingPhases.WritingAudioData:
                    progWriteChapters.Value = (int)(percentage *0.95); //this leaves gap at end for iTunes import to happen in.
                    break;
                case AACfile.ProcessingPhases.WriteComplete:
                    progWriteChapters.Value = 95; //this leaves gap at end for iTunes import to happen in.
                    break;
                case AACfile.ProcessingPhases.ConstructingMergeHeader:
                    progWriteMerged.Value = (int)percentage;
                    break;
                case AACfile.ProcessingPhases.WritingMergeHeader:
                    progWriteMerged.Value = (int)percentage;
                    break;
                case AACfile.ProcessingPhases.WritingMergeAudioData:
                    progWriteMerged.Value = (int) percentage;
                    break;
                case AACfile.ProcessingPhases.WriteMergeComplete:
                    progWriteMerged.Value = 100;
                    break;
                case AACfile.ProcessingPhases.ReadComplete:
                    break;
            }

            Application.DoEvents();
        }

        private static void AAC_ReportError(string message)
        {
            Global.LogIt("AAC_ReportError",message);
            MessageBox.Show(message, "AAC Error");
        }

        private bool DoneIt;
        private void frmMergeProcess_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;
                timerProcess.Start();
            }
        }

        private void timerProcess_Tick(object sender, EventArgs e)
        {
            timerProcess.Stop();

            DoProcess();
        }

        private void DoProcess()
        {
            OpenITunes();

            int filenum = 1;
            foreach (var fileList in Global.ListsToMerge)
            {
                var OK = ProcessFileList(filenum, fileList);
                if (!OK)
                    return;
                filenum++;
            }

            if (Global.ProcessMode == Global.ProcessModes.CDs)
            {
                CleanUpAfterRip(Global.RippedCDFiles);
            }
            else
            {
                CleanUpAfterRip(Global.ConvertedSeparateFiles);
            }

            if (Global.ProcessMode == Global.ProcessModes.ProcessingBatch)
            {
                WindowState = FormWindowState.Normal;
                DialogResult = DialogResult.OK;
                return;
            }

            PlayCompleteSound();

            WindowState = FormWindowState.Normal;
            butDone.Visible = true;
            labDone.Visible = true;
            labDone.AutoSize = false;
            labDone.Width = 404;
            labDone.Height = 58;
            labCurrentState.Text = "";

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

                        //wait to allow audio file to play for a little while.
                        int sleeptime = (int)myMplayer.AudioLength / 1000;
                        if (sleeptime > 10)
                            sleeptime = 10; //ten secs maximum

                        Global.WaitSecs(sleeptime);
                    }
                    catch (Exception)
                    {
                        //do nowt
                    }
                }
            }
        }

        private static void CleanUpAfterRip(FileListBase fileList)
        {
            if (fileList == null)
                return;

            //clean up by deleting ripped files (if any)
            if ((Global.Options.DeleteRippedFiles))
            {
                for (int i = 0; i < fileList.Count; i++)
                {
                    try
                    {
                        //check first that the file is in our temp ripped folder, otherwise don't delete
                        if (Global.GetPath(fileList[i].FilePath).Equals(Global.Options.RipFolder,StringComparison.OrdinalIgnoreCase))
                            File.Delete(fileList[i].FilePath);
                    }
                    catch (Exception)
                    {
                        Global.LogIt("Unable to delete file: ", fileList[i].FilePath);
                    }
                }
            }

            //delete the ripped file list (this prevents resume at this point).
            if (Global.ProcessMode == Global.ProcessModes.CDs)
            {
                try
                {
                   File.Delete(Global.Options.RipFolder + @"\" + Global.SafeFile(fileList.BookTitle) + ".filelist");   
                }
                catch (Exception)
                {
                    Global.LogIt("CleanUpAfterRip", "Unable to delete file");
                }
            }

            if (File.Exists(Global.Options.RipFolder + @"\" + Global.SafeFile(fileList.BookTitle) + ".separatelist"))
            {
                try
                {
                    File.Delete(Global.Options.RipFolder + @"\" + Global.SafeFile(fileList.BookTitle) + ".separatelist");
                }
                catch (Exception)
                {
                    Global.LogIt("CleanUpAfterRip", "Unable to delete file");
                } 
            }
        }


        private bool ProcessFileList(int filenum, FileListBase fileList)
        {
            var prompt = new StringBuilder(Resources.Processing_booktitle_by_author);
            prompt.Replace("<booktitle>", fileList.BookTitle);
            prompt.Replace("<author>", fileList.Author);
            labBookTitle.Text = prompt.ToString();

            prompt = new StringBuilder(Resources.File_filenum_of_totfiles);
            prompt.Replace("<filenum>", filenum.ToString());
            prompt.Replace("<totfiles>", Global.ListsToMerge.Count.ToString());
            labFileNum.Text = prompt.ToString();

            labCurrentState.Text = string.Empty;

            Application.DoEvents();

            progMergeHeaders.Value = 0;
            progWriteChapters.Value = 0;
            progWriteMerged.Value = 0;
            Application.DoEvents();

            bool OK = false;
            switch (Global.Options.Encoder)
            {
                case Global.EncodeToTypes.AAC:
                    OK = MergeAACFiles(fileList, filenum, Global.ListsToMerge.Count);
                    break;
                case Global.EncodeToTypes.MP3:
                    OK = MergeMp3Files(fileList, filenum, Global.ListsToMerge.Count);
                    break;
            }
                
            if (!OK)
                return false;

            Cursor = Cursors.WaitCursor;


            if ((fileList.ChapterType == Global.ChapterTypes.None) || (Global.Options.Encoder == Global.EncodeToTypes.MP3))
            {
                var authorpath = Global.Options.AudiobooksFolder + @"\" + Global.SafeFile(fileList.Author) + @"\" +
                                 Global.SafeFile(fileList.BookTitle); 

                if (! Directory.Exists(authorpath))
                {
                    try
                    {
                        Directory.CreateDirectory(authorpath);   
                    }
                    catch(Exception ex)
                    {
                        prompt = new StringBuilder(Resources.Unable_to_create_folder_path);
                        prompt.Replace("<path>", authorpath);

                        MessageBox.Show(prompt.ToString() + "\r\n\r\n" + ex.Message, Resources.Error_exclaim, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }


                fileList.FinalPath = authorpath + @"\" +
                                     Global.GetFilename(fileList.MergedPath);
                try
                {
                    if (File.Exists(fileList.FinalPath))
                    {
                        File.Delete(fileList.FinalPath);
                    }
                    File.Move(fileList.MergedPath, fileList.FinalPath);
                }
                catch (Exception ex)
                {
                    prompt = new StringBuilder(Resources.Unable_to_move_converted_file_to_path);
                    prompt.Replace("<path>", fileList.FinalPath);

                    MessageBox.Show(prompt.ToString() + "\r\n\r\n" + ex.Message, Resources.Error_exclaim,MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                Chapterize(fileList);
            }

            progMergeHeaders.Value = progMergeHeaders.Maximum;
            progWriteChapters.Value = progWriteChapters.Maximum;
            progWriteMerged.Value = progWriteMerged.Maximum;

            labCurrentState.Text = Resources.Importing_audiobook_into_iTunes__please_wait;

            Cursor = Cursors.Default;

            if ((fileList.MergedOK) && (myITunes != null) && Global.Options.AddToITunesLibrary)
            {
                if (Global.Options.Encoder == Global.EncodeToTypes.AAC)
                {
                    AddMergedAACToLibrary(fileList, filenum);
                }
                if (Global.Options.Encoder == Global.EncodeToTypes.MP3)
                {
                    AddMergedMp3ToLibrary(fileList, filenum);
                }                
            }
            return true;
        }

        private const int TryLimit = 10;

        private void AddMergedAACToLibrary(FileListBase fileList, int filenum)
        {
            Cursor = Cursors.WaitCursor;

            var track = (IITFileOrCDTrack)myITunes.AddFileToLibrary(fileList.FinalPath, Global.iTunesWaitSecs);

            //wait for a bit...
            Global.WaitSecs(5);

            if (track == null)
            {
                Global.LogIt("DoProcess", "iTunes returned null track.");
            }

            Cursor = Cursors.Default;
        }

        private void AddMergedMp3ToLibrary(FileListBase fileList, int filenum)
        {
            Cursor = Cursors.WaitCursor;

            var track = (IITFileOrCDTrack)myITunes.AddFileToLibrary(fileList.FinalPath, Global.iTunesWaitSecs);

            //wait for a bit...
            Global.WaitSecs(5);

            if (track != null)
            {
                int tries = 0;
                bool doneit = false;

                while (!doneit && tries < TryLimit) //might have to make a few attempts at this
                {
                    try
                    {
                        if (Global.MakeSmallImage(fileList.BookImage) != null)
                        {
                            track.AddArtworkFromFile(Global.BookImageFile());
                        }

                        if (Global.Options.Encoder == Global.EncodeToTypes.MP3)
                        {
                            try
                            {
                                track.RememberBookmark = true;
                                track.ExcludeFromShuffle = true;
                            }
                            catch
                            {
                                Global.LogIt("DoProcess", "Unable to set bookmarking.");
                            }
                        }

                        doneit = true;
                        Global.LogIt("DoProcess", "Audiobook entered into library");
                    }
                    catch (Exception ex)
                    {
                        //probably going to be 'this file is not modifiable - may still be being moved, so wait 10 seconds before trying again
                        Global.WaitSecs(5);
                        Global.LogIt("DoProcess", ex.Message);
                        Global.WaitSecs(5);

                        tries++;
                        if (tries >= TryLimit)
                        {
                            MessageBox.Show(
                                Resources.Unable_to_set_cover_artwork_and_bookmarking_in_iTunes,
                                Resources.Sorry_exclaim, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Global.LogIt("DoProcess", "Unable to modify track!");
                        }

                    }
                }
            }
            else
            {
                Global.LogIt("DoProcess", "iTunes returned null track.");
            }

            Cursor = Cursors.Default;
        }

        private void Chapterize(FileListBase fileList)
        {
            if (fileList.ChapterType == Global.ChapterTypes.None)
                return; //nothing to do.

            if (File.Exists(fileList.MergedPath))
            {
                var aac = new AACfile {InterpretMetaData = true};
                aac.ReportError += AAC_ReportError;
                aac.ProcessingProgress += AAC_ProcessingProgress;
                aac.ReportMessage += AAC_ReportMessage;

                string copyFile = Global.GetPath(fileList.MergedPath) + "\\~" + Global.GetFilename(fileList.MergedPath);
                bool OK = DeleteCopyFile(copyFile);
                if (!OK)
                    return;

                try
                {
                    if (Global.Options.DeleteMergedFiles)
                    {
                        File.Move(fileList.MergedPath, copyFile);
                    }
                    else
                    {
                        File.Copy(fileList.MergedPath, copyFile, true);
                    }
                }
                catch (Exception ex)
                {
                    var prompt = new StringBuilder(Resources.Unable_to_move_converted_file_to_path);
                    prompt.Replace("<path>", copyFile);
                    MessageBox.Show(prompt.ToString() + "\r\n\r\n" + ex.Message, Resources.Error_exclaim,
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                aac.ReadFile(copyFile);

                if (aac.LoadedOK)
                {

                    switch (fileList.ChapterType)
                    {
                        case Global.ChapterTypes.BySourceFile:
                            for (int i = 0; i < fileList.Count; i++)
                            {
                                int remainder = (i+1)%fileList.TrackSpacing;
                                if (remainder == 0)
                                {
                                    var frec = fileList[i];
                                    aac.AddChapterStop(frec.CumulativeDuration, frec.ChapterName);
                                }
                            }
                            break;

                        case Global.ChapterTypes.ByDiscAndTrack:
                            for (int i = 0; i < fileList.Count; i++)
                            {
                                var frec = (CDTrackRecord)fileList[i];
                                int remainder = (frec.TrackNumber - 1) % fileList.TrackSpacing;
                                if ((frec.TrackNumber == 1) || (remainder == 0))
                                {
                                    var desc = new Descriptor
                                                   {
                                                       Artist = fileList.Author,
                                                       Album = fileList.BookTitle,
                                                       SongTitle = frec.ChapterName,
                                                       DiscNumber = frec.DiscNumber,
                                                       TrackNumber = frec.TrackNumber
                                                   };

                                    aac.AddChapterStop(frec.CumulativeDuration, desc.QualifyString(Global.Options.AudioCDTemplate));
                                }
                            }
                            break;

                        case Global.ChapterTypes.ByTime:
                            AddRegularChaps(aac, fileList.RegularMins);
                            break;

                        case Global.ChapterTypes.ByTimeWithDisc:
                            AddRegularChaps(aac, fileList.RegularMins);
                            for (int i = 0; i < fileList.Count; i++)
                            {
                                var frec = (CDTrackRecord)fileList[i];
                                if (frec.TrackNumber == 1)
                                {
                                    var desc = new Descriptor
                                    {
                                        Artist = fileList.Author,
                                        Album = fileList.BookTitle,
                                        SongTitle = frec.ChapterName,
                                        DiscNumber = frec.DiscNumber,
                                        TrackNumber = frec.TrackNumber
                                    };

                                    aac.AddChapterStop(frec.CumulativeDuration, desc.QualifyString(Global.Options.AudioCDTemplate));
                                }
                            }
                            break;

                        default:
                            break;
                    }
//                    aac.AddImage(0, Global.BookImage);

                    string authorpath = "";

                    //following is normally TRUE (some weird users don't want it!)
                    if (Global.Options.CreateFoldersForAuthorAndBook)
                    {
                        authorpath = Global.Options.AudiobooksFolder + @"\" + Global.SafeFile(fileList.Author) +
                                         @"\" + Global.SafeFile(fileList.BookTitle);
                    }
                    else
                    {
                        authorpath = Global.Options.AudiobooksFolder;
                    }

                    if (!Directory.Exists(authorpath))
                    {
                        try
                        {
                            Directory.CreateDirectory(authorpath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(Resources.Unable_to_create_folder_path.Replace("<path>",authorpath) + "\r\n\r\n" + ex.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    fileList.FinalPath = authorpath + @"\" +
                                         Global.GetFilename(fileList.MergedPath);
                    aac.SaveFile(fileList.FinalPath);
                    aac.CloseFileReader();
                    aac.CloseFileWriter();
                    aac.Dispose();
                }

                OK = DeleteCopyFile(copyFile);
            }
        }

        private static bool DeleteCopyFile(string copyFile)
        {
            if (File.Exists(copyFile))
            {
                try
                {
                    File.Delete(copyFile);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.Unable_to_delete_prior_copy_path.Replace("<path>", copyFile) + ", " + ex.Message, Resources.Error_exclaim);
                    return false;
                }
            }
            return true; //it's ok if file isn't already existing.
        }

        private static bool AddRegularChaps(AACfile mp4, double regularMins)
        {
            if (mp4 == null)
                return false;

            if (!mp4.LoadedOK)
                return false;

            long intlong;

            try
            {
                intlong = Convert.ToInt64(TimeSpan.TicksPerMinute * regularMins);
                if (intlong <= 0)
                    return true; //nothing to do, but not a failure.
            }
            catch
            {
                return false;
            }


            TimeSpan incrementSpan = new TimeSpan(intlong);

            TimeSpan EndTime = mp4.GetMovieDurationAsTimeSpan();
            TimeSpan CurrentTime = TimeSpan.Zero;

            int chapnum = 1;
            while (CurrentTime < EndTime)
            {
                mp4.AddChapterStop(CurrentTime, Global.Options.RegularPrefix + " " + chapnum.ToString());
                CurrentTime = CurrentTime + incrementSpan;
                chapnum++;
            }

            return true;
        }


        private bool OpenITunes()
        {
           labFileNum.Text = Resources.Please_wait__opening_iTunes;

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

                myITunes.AACConvertedAll += myITunes_AACConvertedAll;
                myITunes.MP3ConvertedAll += myITunes_MP3ConvertedAll;
                myITunes.ReportProgress += myITunes_ReportProgress;
                myITunes.Waiting += myITunes_Waiting;
                myITunes.iTune.OnCOMCallsDisabledEvent += myITunes_OnCOMCallsDisabledEvent;
                myITunes.iTune.OnCOMCallsEnabledEvent += myITunes_OnCOMCallsEnabledEvent;
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

        void myITunes_OnCOMCallsEnabledEvent()
        {
            //
        }

        void myITunes_OnCOMCallsDisabledEvent(ITCOMDisabledReason reason)
        {
            //
        }

        void myITunes_ReportProgress(string name, int progress, int maxprogress)
        {
            //
        }

        void myITunes_MP3ConvertedAll(iTunesConvertOperationStatus status)
        {
            //
        }

        void myITunes_AACConvertedAll(iTunesConvertOperationStatus status)
        {
            //
        }

        private void butDone_Click(object sender, EventArgs e)
        {

        }

        private bool MergeMp3Files(FileListBase fileList, int listcount, int maxcount)
        {
            string firstfile = "";

            if (fileList == null)
                return false;

            if (fileList.Count == 0)
                return false;

            if (fileList.Count == 1)
            {
                firstfile = fileList[0].FilePath;
                string mergedname = GetMergedName(listcount, maxcount, fileList.BookTitle, ".mp3");
                try
                {
                    File.Copy(firstfile, mergedname, true);
                    fileList.MergedPath = mergedname;
                    fileList.MergedOK = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.Unable_to_move_converted_file_to_path.Replace("<path>", firstfile) + ": " + ex.Message, Resources.Error_exclaim);
                    fileList.MergedOK = false;
                    return false;
                }
                return true;
            }

            progMergeHeaders.Maximum = fileList.Count;

            string DestName = GetMergedName(listcount, maxcount, fileList.BookTitle, ".mp3");

            fileList.MergedPath = DestName;

            if (File.Exists(DestName))
            {
                bool doneIt = false;

                while (!doneIt)
                {
                    try
                    {
                        File.Delete(DestName);
                        doneIt = true;
                    }
                    catch (Exception)
                    {
                        if (MessageBox.Show(
                            Resources.Unable_to_delete_existing_file_path.Replace("<path>", DestName) + "\r\n\r\n" +
                            Resources.Try_again_query, Resources.Error_exclaim, MessageBoxButtons.YesNo,
                            MessageBoxIcon.Error) == DialogResult.No)
                        {
                            return false;
                        }
                    }
                }
            }

            long TotalFrames = 0;

            progMergeHeaders.Maximum = fileList.Count;

            for (int i = 0; i < fileList.Count; i++)
            {
                var frec = fileList[i];
                if (frec.FileType != Global.FileTypes.MP3)
                {
                    MessageBox.Show(Resources.Bad_file_type_in_list.Replace("<filetype>", frec.FileType.ToString()), Resources.Error_exclaim);
                    return false;
                }
                var thisMP3 = new mp3file();
                if (thisMP3.OpenForRead(frec.FilePath))
                {
                    TotalFrames += thisMP3.NumFrames;
                    thisMP3.Close();
                }
                progMergeHeaders.Value++;
            }

            var mergeFile = new mp3file();

            progWriteMerged.Maximum = fileList.Count;

            if (mergeFile.OpenForWrite(DestName))
            {
                SetMp3MetaData(fileList, listcount, mergeFile);

                long FramesToCopy = TotalFrames;

                var firstItem = new AudioFileItem();
                GetMp3Info(fileList[0].FilePath,ref firstItem);

                if (firstItem.HasXing)
                {
                    mergeFile.WriteXing(firstItem.Offset, firstItem.byte1, firstItem.byte2, firstItem.byte3,
                                        firstItem.byte4, firstItem.FirstFrameLength, FramesToCopy);
                }

                int n = 0;
                long TotalFramesCopied = 0;

                while (n < fileList.Count)
                {
                    FileRecord frec = fileList[n];

                    long FramesCopied = 0;

                    mergeFile.NewAppendFile(frec.FilePath, out FramesCopied);
                    TotalFramesCopied += FramesCopied;
                    n++;
                    progWriteMerged.Value = n;
                    Application.DoEvents();
                }
                if ((firstItem.HasXing) && (TotalFramesCopied != FramesToCopy))
                {
                    mergeFile.PrependID2(); //do this really just to get position right
                    mergeFile.WriteXing(firstItem.Offset, firstItem.byte1, firstItem.byte2, firstItem.byte3, firstItem.byte4, firstItem.FirstFrameLength, TotalFramesCopied);
                }

                mergeFile.AppendID1();
                mergeFile.Close();

                progWriteMerged.Value = progWriteMerged.Maximum;

            }
            else
            {
                MessageBox.Show(Resources.Unable_to_open_path_for_writing.Replace("<path>", DestName), Resources.Error_exclaim, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                fileList.MergedOK = false;
                return false;
            }
            fileList.MergedOK = true;
            return true;
        }

        private void SetMp3MetaData(FileListBase fileList, int listcount, mp3file mergeFile)
        {
            mergeFile.Album = Global.SafeFile(fileList.BookTitle);
            mergeFile.Title = Global.ListsToMerge.Count > 1
                                  ? Global.SafeFile(fileList.BookTitle) + " " + Global.PadZero(listcount, Global.ListsToMerge.Count)
                                  : Global.SafeFile(fileList.BookTitle);
            mergeFile.Artist = Global.SafeFile(fileList.Author);
            mergeFile.Genre = Global.Options.DefaultGenre;
            mergeFile.Comment = "Audiobook created by MarkAble";
        }


        private static void GetMp3Info(string aFile, ref AudioFileItem thisItem)
        {
            var thisMp3 = new mp3file();
            thisItem.AudioType = AudioFileItem.AudioTypes.MP3;
            if (thisMp3.OpenForRead(aFile))
            {
                thisItem.Length = thisMp3.Length;
                thisMp3.GetAudioData();

                thisItem.Artist = thisMp3.Artist;
                thisItem.Album = thisMp3.Album;
                thisItem.Title = thisMp3.Title;
                thisItem.Genre = thisMp3.Genre;
                thisItem.Year = thisMp3.Year;
                thisItem.Comment = thisMp3.Comment;
                thisItem.Encoder = thisMp3.Encoder;
                thisItem.Podcast = thisMp3.Podcast;
                thisItem.DiscNum = thisMp3.DiscNum;
                thisItem.TrackNum = thisMp3.Track;

                thisItem.Channels = thisMp3.CurrentFrame.ChannelMode == mp3file.ChannelTypes.SingleChannel ? 1 : 2;
                thisItem.Duration = thisMp3.DurationInSecs;
                thisItem.NumFrames = thisMp3.NumFrames;
                thisItem.byte1 = thisMp3.CurrentFrame.byte1;
                thisItem.byte2 = thisMp3.CurrentFrame.byte2;
                thisItem.byte3 = thisMp3.CurrentFrame.byte3;
                thisItem.byte4 = thisMp3.CurrentFrame.byte4;
                thisItem.FirstFrameLength = thisMp3.CurrentFrame.Length;
                thisItem.Offset = thisMp3.CurrentFrame.Offset;
                thisItem.HasXing = thisMp3.HasXing;

                thisMp3.Close();
            }
        }

        private void frmMergeProcess_FormClosing(object sender, FormClosingEventArgs e)
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

    }
}
