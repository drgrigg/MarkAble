using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MarkAble2.Properties;
using System.Threading;

namespace MarkAble2
{
	public partial class frmIntro : Form
	{
		public frmIntro()
		{
			InitializeComponent();
		}

		private void butQuit_Click(object sender, EventArgs e)
		{

//            if (Prefs.HaveChanged())
//            {
//                DialogResult result = MessageBox.Show(
//Resources.Your_iTunes_encoder_preferences_have_been_changed_by_MarkAble + "\r\n\r\n" +
//Resources.If_you_would_like_us_to_restore_your_previous_settings__please_close_iTunes + "\r\n" +
//Resources.When_it_has_completely_shut_down__click__Yes + "\r\n\r\n" +
//Resources.Otherwise__click__No__now + " " + Resources.You_won_t_be_asked_again,
//                    Resources.Restore_Preferences, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//                if (result == DialogResult.Yes)
//                {
//                    Prefs.RestorePrefs();
//                }
//                else
//                {
//                    Prefs.DeleteBackup();
//                }
//            }

			timerClose.Enabled = true;
		}

		private void timerClose_Tick(object sender, EventArgs e)
		{
			timerClose.Stop();
			Close();
		}

		private void butAudioCD_Click(object sender, EventArgs e)
		{
			Global.ProcessMode = Global.ProcessModes.CDs;

			if (AnyIncompleteCDprojects())
				return; //if true, we processed one or more incomplete projects

			int xpos = Left;
			int ypos = Top;

			try
			{
				HideIntro();

				//get required audio CD info
				if (GetCDInfo(ref xpos, ref ypos) == DialogResult.Cancel)
				{
					RestoreIntro(xpos, ypos);
					return;
				}

				//now show processing form.
				if (ProcessCD(ref xpos, ref ypos) == DialogResult.Cancel)
				{
					RestoreIntro(xpos, ypos);

					return;
				}

				//now show merge form
				if (GetMergeAndChapterInfo(ref xpos, ref ypos) == DialogResult.Cancel)
				{
					RestoreIntro(xpos, ypos);
					return;
				}

				Global.BuildListsToMerge(Global.RippedCDFiles);

				if (MergeFiles(ref xpos, ref ypos) == DialogResult.Cancel)
				{
					RestoreIntro(xpos, ypos);

					return;
				}
			}
			finally
			{
				RestoreIntro(xpos, ypos);
			}
		}

		private void HideIntro()
		{
			//just push it out of sight
			Rectangle rect = Screen.GetBounds(this);
			Left = rect.Width + 2;
			Top = rect.Height + 2;
		}

		private void RestoreIntro(int xpos, int ypos)
		{
			WindowState = FormWindowState.Normal;
			Left = xpos;
			Top = ypos;
			Show();
			Activate();
			Global.SaveLog();
		}

		private DialogResult MergeFiles(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmMergeProcess());
		}

		private DialogResult GetMergeAndChapterInfo(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmMergeAndChapter());

		}

		private static DialogResult GetCDInfo(ref int xpos, ref int ypos)
		{
			Global.RippedCDFiles = new CDFileList();

			var cdForm = new frmCDinfo {Left = xpos, Top = ypos, BackColor = Global.Options.BackgroundColor};

			DialogResult result = cdForm.ShowDialog();
			xpos = cdForm.Left;
			ypos = cdForm.Top;
			cdForm.Close();
		   
			return result;
		}

		private static DialogResult ProcessCD(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmCDprocess());
		}

		private void frmIntro_Load(object sender, EventArgs e)
		{
			PositionTheForm();
		}

		private static void LoadOptions()
		{
			if (!Directory.Exists(Global.LocalAppData() + @"\MarkAble"))
			{
				try
				{
					Directory.CreateDirectory(Global.LocalAppData() + @"\MarkAble");
				}
				catch (Exception ex)
				{
					MessageBox.Show(Resources.Unable_to_create_local_app_data_folder + ": " + ex.Message, Resources.Error_exclaim);
					return;
				}
			}

			Global.Options = OptionClass.Load(Global.LocalAppData() + @"\MarkAble\Options.xml");

			SetMyDocsSubFolder(ref Global.Options.RipFolder, "Temp");
			SetMyDocsSubFolder(ref Global.Options.MergeFolder, "Merged");
			SetMyMusicSubFolder(ref Global.Options.AudiobooksFolder, "MarkAble Audiobooks");

			if (String.IsNullOrEmpty(Global.Options.DefaultImage))
			{
				Global.Options.DefaultImage = Application.StartupPath + @"\" + "defaultimage.jpg";
			}

			if (Global.Options.TrackSpacing == 0)
				Global.Options.TrackSpacing = 1;

			if (Global.Options.DebugMode)
			{
				Global.LogLine();
				Global.LogIt("OPTIONS:", Global.Options.ToString());
				Global.LogLine();
				Global.SaveLog();
			}

			Global.Options.Save();
		}

		private static void SetMyDocsSubFolder(ref string foldername, string defaultname)
		{
			if (String.IsNullOrEmpty(foldername))
			{
				foldername = Global.MyDocuments() + @"\MarkAble\" + defaultname;
			}
			if (!Directory.Exists(foldername))
			{
				try
				{
					Directory.CreateDirectory(foldername);
				}
				catch (Exception)
				{
					MessageBox.Show(Resources.Unable_to_create_folder + ": " + foldername);
				}
			}
		}

		private static void SetMyMusicSubFolder(ref string foldername, string defaultname)
		{
			if (String.IsNullOrEmpty(foldername))
			{
				foldername = Global.MyMusic() + @"\" + defaultname;
			}
			if (!Directory.Exists(foldername))
			{
				try
				{
					Directory.CreateDirectory(foldername);
				}
				catch (Exception)
				{
					MessageBox.Show(Resources.Unable_to_create_folder + ": " + foldername);
				}
			}
		}

		private void frmIntro_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.LastPosition = Location;
			Settings.Default.Save();
			Global.SaveLog();
		}

		private VersionCheck currentVersion, myVersion;


		private bool DoneIt;
		private void frmIntro_Activated(object sender, EventArgs e)
		{
			if (!DoneIt)
			{
				DoneIt = true;

				Text = "MarkAble " + Global.Version;

				LoadOptions();
				
				this.BackColor = Global.Options.BackgroundColor;

				//if (Global.Options.ChangePrefs && (!Prefs.CustomRateIsAlreadySet()))
				//{
				//    DialogResult result = MessageBox.Show(
				//                                             Resources.
				//                                                 Your_preferred_encoding_rate_has_not_been_set_in_iTunes +
				//                                             "\r\n\r\n" +
				//                                             Resources.
				//                                                 If_you_wish_MarkAble_to_set_this_rate__please_close_down_iTunes +
				//                                             "\r\n" +
				//                                             Resources.
				//                                                 When_it_has_fully_closed_down__then_click__Yes__below +
				//                                             "\r\n\r\n" +
				//                                             Resources.Otherwise__click__No__now,
				//                                             "Set iTunes Prefs?", MessageBoxButtons.YesNo,
				//                                             MessageBoxIcon.Information);

				//    if (result == DialogResult.Yes)
				//    {
				//        Prefs.SetCustomRate();
				//    }
				//}

				if (Global.Options.CheckForNewVersion)
				{
					CheckNewVersion();
				}
			}
		}

		private void PositionTheForm()
		{
			var lastPosition = Settings.Default.LastPosition;

			if (IsOnScreen(lastPosition))
			{
				Location = lastPosition;
			}
			else
			{
				Settings.Default.LastPosition = Location;
				Settings.Default.Save();
			}
		}

		private bool IsOnScreen(Point position)
		{
			Rectangle screen = Screen.PrimaryScreen.Bounds;
			return (position.X >= screen.Left && position.Y >= screen.Top && position.X <= (screen.Right - Width) &&
					position.Y <= (screen.Bottom - Height));
		}

		private void CheckNewVersion()
		{
			currentVersion = new VersionCheck();
			currentVersion.GotVersion += GotCurrentVersion;
			currentVersion.RequestCurrentVersion("http://rightword.com.au/products/markable/getversion.php");
		}

		void GotCurrentVersion()
		{
			var compareVersion = new CompareCurrentVersionDelegate(CompareCurrentVersion);
			this.BeginInvoke(compareVersion);
		}

		private delegate void CompareCurrentVersionDelegate();
		private void CompareCurrentVersion()
		{
			myVersion = new VersionCheck(Global.Version);
			if (myVersion.CompareTo(currentVersion) < 0)
			{
				var prompt = Resources.A_new_version_of_MarkAble_is_available;
				prompt = prompt.Replace("<versionnum>", currentVersion.ToString());

				var F = new frmAsk(prompt);
				F.BackColor = Global.Options.BackgroundColor;
				DialogResult result = F.ShowDialog();
				if (F.StopAsking())
				{
					Global.Options.CheckForNewVersion = false;
					Global.Options.Save();
				}

				if (result == DialogResult.Yes)
				{
					System.Diagnostics.Process.Start("http://rightword.com.au/products/markable/download.php");
					//this.Close();
				}
			}
		}

		private bool AnyIncompleteCDprojects()
		{
			//look for incomplete rips
			Global.LogImmediate("ProcessIncomplete", "Looking for *.filelist in " + Global.Options.RipFolder);
			string[] incompletes = Directory.GetFiles(Global.Options.RipFolder, "*.filelist");

			if (incompletes.Length == 0)
			{
				return false; //go on with normal processing.
			}
			
			//this handles multiple incomplete projects
			foreach(var incomplete in incompletes)
			{
				if (ReadIncompleteIntoRippedCDFiles(incomplete))
				{
					var processResult = ProcessIncompleteCDProject(incomplete);
					switch(processResult)
					{
						case DialogResult.OK: //processed one incomplete project, get out of loop
						case DialogResult.Cancel: //cancelled out of processing this project, don't go on to next
							return true;

						case DialogResult.Abort: //killed this project, try the next one or do normal
						case DialogResult.Ignore: //skipping this project, try the next one or do normal
							break;

						default: //any odd dialog result
							return false;
					}
				}
			}
			return false; //go on with normal processing.
		}

		private DialogResult ProcessIncompleteCDProject(string incompleteProjectFile)
		{
			var prompt = new StringBuilder();
			prompt.Append(Resources.It_looks_as_if_you_didn_t_finish_processing_CDs_for_booktitle);
			prompt.Replace("<booktitle>", Global.RippedCDFiles.BookTitle);
			prompt.Replace("<author>", Global.RippedCDFiles.Author);
			prompt.AppendLine();
			prompt.AppendLine();
			prompt.Append(Resources.Do_you_want_to_resume_query);

			DialogResult result =
				MessageBox.Show(prompt.ToString(), Resources.Resume_query,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				//check to see that we're encoding remaining files same way as earlier ones!!
				if (Global.RippedCDFiles.Count > 0 &&
					Global.RippedCDFiles[0].FileType != Global.ConvertEncoderToFileType(Global.Options.Encoder))
				{
					prompt = new StringBuilder(Resources.Files_so_far_ripped_were_encoded_as_filetype);
					prompt.Replace("<filetype>", Global.RippedCDFiles[0].FileType.ToString());
					prompt.AppendLine();
					prompt.AppendLine();
					prompt.Append(Resources.Your_current_preferences_are_for_encodertype_encoding);
					prompt.Replace("<encodertype>", Global.Options.Encoder.ToString());
					prompt.AppendLine();
					prompt.AppendLine();
					prompt.Append(Resources.Change_your_options_to_filetype);
					prompt.Replace("<filetype>", Global.RippedCDFiles[0].FileType.ToString());
					prompt.Append(" (");
					prompt.Append(Resources.Saying_No_will_abandon_the_resume);
					prompt.Append(")");

					result =
						MessageBox.Show(prompt.ToString(),
										Resources.Different_Encoder, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

					if (result == DialogResult.No)
						return DialogResult.Ignore;

					Global.Options.Encoder = Global.ConvertFileTypeToEncoder(Global.RippedCDFiles[0].FileType);
				}

				int xpos = Left;
				int ypos = Top;

				try
				{
					HideIntro();

					Global.CurrentDiscNum = Global.RippedCDFiles.CurrentDisc + 1; //we want to process the NEXT disc!

					if (Global.RippedCDFiles.CurrentDisc < Global.RippedCDFiles.TotalDiscs) //still work to do
					{
						Global.ProcessingFirstCd = true;

						//now show processing form.
						if (ProcessCD(ref xpos, ref ypos) == DialogResult.Cancel)
						{
							RestoreIntro(xpos, ypos);
							return DialogResult.Cancel;
						}
					}

					//now show merge form
					if (GetMergeAndChapterInfo(ref xpos, ref ypos) == DialogResult.Cancel)
					{
						RestoreIntro(xpos, ypos);
						return DialogResult.Cancel;
					}

					Global.BuildListsToMerge(Global.RippedCDFiles);

					//actually do the merge
					if (MergeFiles(ref xpos, ref ypos) == DialogResult.Cancel)
					{
						RestoreIntro(xpos, ypos);
						return DialogResult.Cancel;
					}

					RestoreIntro(xpos, ypos);

					Global.RippedCDFiles = null;
					return DialogResult.OK; //processed this incomplete project, get out of loop.
				}
				finally
				{
					RestoreIntro(xpos, ypos);
				}
			}


			//only get here if we said 'No' to processing this one.
			prompt = new StringBuilder(Resources.Delete_the_booktitle_project);
			prompt.Replace("<booktitle>", Global.RippedCDFiles.BookTitle);
			prompt.AppendLine();
			prompt.AppendLine();
			prompt.Append(Resources.If_you_say_Yes_you_ll_never_be_prompted_to_resume_this_project_again);

			if (MessageBox.Show(prompt.ToString(), Resources.Discard_project_query, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				//delete the already ripped files
				for (int i = 0; i < Global.RippedCDFiles.Count; i++)
				{
					try
					{
						File.Delete(Global.RippedCDFiles[i].FilePath);
					}
					catch (Exception)
					{
						//do nowt
					}
				}
				Global.RippedCDFiles = null;

				//delete the project file.
				try
				{
					File.Delete(incompleteProjectFile);
				}
				catch (Exception)
				{
					//do nowt
				} 
				return DialogResult.Abort; //killed this incomplete project
			}
			return DialogResult.Ignore; //not processing this project just yet, but not removing it
		}

		private bool ReadIncompleteIntoRippedCDFiles(string incomplete)
		{
			try
			{
				Global.RippedCDFiles = (CDFileList) Global.FromXmlFile(incomplete, typeof (CDFileList));
			}
			catch (Exception ex)
			{
				Global.LogImmediate("Unable to parse .filelist " + incomplete, ex.Message);

				try
				{
					File.Delete(incomplete);
				}
				catch (Exception ex2)
				{
					Global.LogImmediate("Unable to delete badly parsed filelist: " + incomplete, ex2.Message);
				} 
				return false;
			}

			if (Global.RippedCDFiles == null)
			{
				Global.LogImmediate("Unable to parse .filelist " + incomplete, "Returned null RippedCDFiles");
				return false;
			}
			return true;
		}

		private bool AnyIncompleteSeparateProjects()
		{
			//look for incomplete rips
			Global.LogImmediate("ProcessIncomplete", "Looking for *.separatelist in " + Global.Options.RipFolder);
			string[] incompletes = Directory.GetFiles(Global.Options.RipFolder, "*.separatelist");
			if (incompletes.Length == 0)
			{
				return false; //processing normally must continue
			}

			foreach(var incomplete in incompletes)
			{
				if(ReadIncompleteIntoUnRippedFiles(incomplete))
				{
					var processResult = ProcessIncompleteSeparateProject(incomplete);
					switch (processResult)
					{
						case DialogResult.OK: //processed one incomplete project, get out of loop
						case DialogResult.Cancel: //cancelled out of processing this project, don't go on to next
							return true;

						case DialogResult.Abort: //killed this project, try the next one
						case DialogResult.Ignore: //skipping this project, try the next one.
							break;

						default: //any unexpected result
							return false; //go on with normal processing.
					}
				}
			}

			return false; //go on with normal processing.
		}

		private DialogResult ProcessIncompleteSeparateProject(string incompleteProjectFile)
		{
			var prompt = new StringBuilder();
			prompt.Append(Resources.It_looks_as_if_you_didn_t_finish_processing_files_for_booktitle);
			prompt.Replace("<booktitle>", Global.UnRippedFiles.BookTitle);
			prompt.Replace("<author>", Global.UnRippedFiles.Author);
			prompt.AppendLine();
			prompt.AppendLine();
			prompt.Append(Resources.Do_you_want_to_resume_query);

			DialogResult result =
				MessageBox.Show(prompt.ToString(), Resources.Resume_query,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				////check to see that we're encoding remaining files same way as earlier ones!!

				//if (Global.UnRippedFiles.Files[0].FileType != Global.ConvertEncoderToFileType(Global.Options.Encoder))
				//{
				//    prompt = new StringBuilder(Resources.Files_so_far_ripped_were_encoded_as_filetype);
				//    prompt.Replace("<filetype>", Global.UnRippedFiles[0].FileType.ToString());
				//    prompt.AppendLine();
				//    prompt.AppendLine();
				//    prompt.Append(Resources.Your_current_preferences_are_for_encodertype_encoding);
				//    prompt.Replace("<encodertype>", Global.Options.Encoder.ToString());
				//    prompt.AppendLine();
				//    prompt.AppendLine();
				//    prompt.Append(Resources.Change_your_options_to_filetype);
				//    prompt.Replace("<filetype>", Global.UnRippedFiles[0].FileType.ToString());
				//    prompt.Append(" (");
				//    prompt.Append(Resources.Saying_No_will_abandon_the_resume);
				//    prompt.Append(")");

				//    result =
				//        MessageBox.Show(prompt.ToString(),
				//            Resources.Different_Encoder, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				//    if (result == DialogResult.No)
				//        return DialogResult.Ignore;

				//    Global.Options.Encoder = Global.ConvertFileTypeToEncoder(Global.UnRippedFiles.Files[0].FileType);
				//}


				if (Global.UnRippedFiles.ProcessStage < SeparateFileList.ProcessStages.UnMerged)
				{
					Global.ConvertedSeparateFiles = new SeparateFileList();
					Global.ConvertedSeparateFiles.Clone(Global.UnRippedFiles);

					foreach(SeparateRecord frec in Global.UnRippedFiles.Files)
					{
						if (frec.Converted)
						{
							Global.ConvertedSeparateFiles.Add(frec);
						}
					}
				}
				else
				{
					Global.ConvertedSeparateFiles = Global.UnRippedFiles;
				}

				//delete the current incomplete project file to avoid retriggering if we hit another error.
				try
				{
					File.Delete(incompleteProjectFile);
				}
				catch (Exception)
				{
					//do nowt
				}

				int xpos = Left;
				int ypos = Top;

				try
				{
					HideIntro();

					if (Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.None)
					{
						if (ShowSeparate1(ref xpos, ref ypos) == DialogResult.Cancel)
						{
							RestoreIntro(xpos, ypos);
							return DialogResult.Cancel;
						}
					}

					if (Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.Selected)
					{
						if (ShowSeparate2(ref xpos, ref ypos) == DialogResult.Cancel)
						{
							RestoreIntro(xpos, ypos);
							return DialogResult.Cancel;
						}
					}

					if (Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.Sorted)
					{
						if (ShowSeparate3(ref xpos, ref ypos) == DialogResult.Cancel)
						{
							RestoreIntro(xpos, ypos);
							return DialogResult.Cancel;
						}
					}

					Global.DeleteUnknownList();

					if (Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.MetaDataAdded)
					{
						if (ShowSeparate4(ref xpos, ref ypos) == DialogResult.Cancel)
						{
							RestoreIntro(xpos, ypos);
							return DialogResult.Cancel;
						}
					}

					if (Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.ParametersSet || Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.Converting)
					{
						//convert the files to AAC or MP3
						if (ShowProcessSeparate(ref xpos, ref ypos) == DialogResult.Cancel)
						{
							RestoreIntro(xpos, ypos);
							return DialogResult.Cancel;
						}
					}

					if (Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.UnMerged)
					{
						//now set us up to merge
						Global.BuildListsToMerge(Global.ConvertedSeparateFiles);

						//actually do the merge
						if (MergeFiles(ref xpos, ref ypos) == DialogResult.Cancel)
						{
							RestoreIntro(xpos, ypos);
							return DialogResult.Cancel;
						}
					}
				}
				finally
				{
					RestoreIntro(xpos, ypos);
				}        

				return DialogResult.OK;
			}

			//only get here if we said 'No' to processing this one.
			prompt = new StringBuilder(Resources.Delete_the_booktitle_project);
			prompt.Replace("<booktitle>", Global.UnRippedFiles.BookTitle);
			prompt.AppendLine();
			prompt.AppendLine();
			prompt.Append(Resources.If_you_say_Yes_you_ll_never_be_prompted_to_resume_this_project_again);

			if (MessageBox.Show(prompt.ToString(), Resources.Discard_project_query, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				Global.DeleteUnknownList();

				if (Global.UnRippedFiles.ProcessStage == SeparateFileList.ProcessStages.UnMerged)
				{
					for (int i = 0; i < Global.UnRippedFiles.Count; i++)
					{
						if (Global.GetPath(Global.UnRippedFiles[i].FilePath) == Global.Options.RipFolder) //only delete them if they are in our temp folder.
						{
							try
							{
								File.Delete(Global.UnRippedFiles[i].FilePath);
							}
							catch (Exception)
							{
								//do nowt
							}
						}
					} 
				}

				try
				{
					File.Delete(incompleteProjectFile);
				}
				catch (Exception)
				{
					//do nowt
				}
				return DialogResult.Abort;
			}
			return DialogResult.Ignore;
		}

		private static bool ReadIncompleteIntoUnRippedFiles(string incomplete)
		{
			try
			{
				Global.UnRippedFiles = (SeparateFileList)Global.FromXmlFile(incomplete, typeof(SeparateFileList));
			}
			catch (Exception ex)
			{
				Global.LogImmediate("Unable to parse .separatelist " + incomplete, ex.Message);

				try
				{
					File.Delete(incomplete);
				}
				catch (Exception ex2)
				{
					Global.LogImmediate("Unable to delete badly parsed .separatelist: " + incomplete, ex2.Message);
				}
				return false;
			}

			if (Global.UnRippedFiles == null)
			{
				Global.LogImmediate("Unable to parse .separatelist " + incomplete, "Returned null Global.UnRippedFiles");
				return false;
			}
			return true;
		}


		private void timerResume_Tick(object sender, EventArgs e)
		{
			timerResume.Stop();
		}

		private void butOther_Click(object sender, EventArgs e)
		{
			Global.ProcessMode = Global.ProcessModes.Other;
			Global.ConvertedSeparateFiles = new SeparateFileList();

			if (AnyIncompleteSeparateProjects()) //returns false if we did not finish processing.
				return;

			int xpos = Left;
			int ypos = Top;

			try
			{
				HideIntro();

				Global.UnRippedFiles = new SeparateFileList();

				if (ShowSeparate1(ref xpos, ref ypos) == DialogResult.Cancel)
				{
					RestoreIntro(xpos, ypos);
					return;
				}

				if (Global.ProcessMode != Global.ProcessModes.ProcessingBatch)
				{
					ProcessSeparateNonBatch(xpos, ypos);
				}
				else //we're running a batch
				{
					ProcessBatch(xpos, ypos);
				}
			}
			finally
			{
				RestoreIntro(xpos, ypos);
			}
		}

		private void ProcessSeparateNonBatch(int xpos, int ypos)
		{
			if (ShowSeparate2(ref xpos, ref ypos) == DialogResult.Cancel)
			{
				RestoreIntro(xpos, ypos);
				return;
			}
			//else do step 3 of separate files.

			if (ShowSeparate3(ref xpos, ref ypos) == DialogResult.Cancel)
			{
				RestoreIntro(xpos, ypos);
				return;
			}

			Global.UnRippedFiles.NumParts = 1;

			if (ShowSeparate4(ref xpos, ref ypos) == DialogResult.Cancel)
			{
				RestoreIntro(xpos, ypos);
				return;
			}

			//check to see if we added the current files to a batch, rather than process immediately
			if (Global.ProcessMode == Global.ProcessModes.AddedToBatch)
			{
				RestoreIntro(xpos, ypos);
				return;
			}

			//convert the files to AAC
			if (ShowProcessSeparate(ref xpos, ref ypos) == DialogResult.Cancel)
			{
				RestoreIntro(xpos, ypos);
				return;
			}

			//now set us up to merge
			Global.BuildListsToMerge(Global.ConvertedSeparateFiles);

			//actually do the merge
			if (MergeFiles(ref xpos, ref ypos) == DialogResult.Cancel)
			{
				RestoreIntro(xpos, ypos);
				return;
			}
			RestoreIntro(xpos, ypos);
		}

		private void ProcessBatch(int xpos, int ypos)
		{
			if (Global.CurrentBatch != null)
			{
				//show batch list contents
				if (ShowBatch(ref xpos, ref ypos) == DialogResult.Cancel)
				{
					RestoreIntro(xpos, ypos);
					return;
				}

				foreach (SeparateFileList unrippedList in Global.CurrentBatch.Lists)
				{
					Global.UnRippedFiles = unrippedList;

					//convert the files to AAC
					if (ShowProcessSeparate(ref xpos, ref ypos) == DialogResult.Cancel)
					{
						RestoreIntro(xpos, ypos);
						return;
					}

					//now set us to merge
					Global.BuildListsToMerge(Global.ConvertedSeparateFiles);

					//actually do the merge
					if (MergeFiles(ref xpos, ref ypos) == DialogResult.Cancel)
					{
						RestoreIntro(xpos, ypos);
						return;
					}
				}
				RestoreIntro(xpos,ypos);
				PlayCompleteSound();

				MessageBox.Show(Resources.MarkAble_batch_processing_complete, Resources.Done_exclaim);
			}
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
					}
					catch (Exception)
					{
						//do nowt
					}
				}
			}
		}


		private static DialogResult ShowSeparate1(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmSeparate1());
		}

		private static DialogResult ShowSeparate2(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmSeparate2());
		}

		private static DialogResult ShowSeparate3(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmSeparate3());
		}

		private static DialogResult ShowSeparate4(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmSeparateMergeChap());
		}

		private static DialogResult ShowProcessSeparate(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmSeparateProcess());
		}

		private static DialogResult ShowBatch(ref int xpos, ref int ypos)
		{
			return ShowForm(ref xpos, ref ypos, new frmBatch());
		}



		private static DialogResult ShowForm(ref int xpos, ref int ypos, Form aForm)
		{
			aForm.Left = xpos;
			aForm.Top = ypos;
			aForm.BackColor = Global.Options.BackgroundColor;

			DialogResult result = aForm.ShowDialog();

			xpos = aForm.Left;
			ypos = aForm.Top;
			aForm.Close();

			return result;
		}

		private void butPrefs_Click(object sender, EventArgs e)
		{
			var F = new frmOptions();

			F.BackColor = Global.Options.BackgroundColor;
			F.ShowDialog();
			this.BackColor = Global.Options.BackgroundColor;
		}

		private void butHelp_Click(object sender, EventArgs e)
		{
			var culture = Thread.CurrentThread.CurrentUICulture;
			System.Diagnostics.Process.Start("http://rightword.com.au/products/markable/help2/default.php?language=" + culture.TwoLetterISOLanguageName);
		}

	}
}