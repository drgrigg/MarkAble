using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmOptions : Form
    {
        private string myTempImage = "";

        public frmOptions()
        {
            InitializeComponent();
        }

        private void butSave_Click(object sender, EventArgs e)
        {

            if (chkPlaySoundOnDiscComplete.Checked && (!File.Exists(Global.Options.DiscCompleteSound)))
            {
                GetDiscCompleteSoundFile();
            }

            if (cboDrives.Text != "")
            {
                Global.Options.CDDrive = cboDrives.Text;
            }

            if (TryCreateFolder(txtTempFolder.Text))
                Global.Options.RipFolder = txtTempFolder.Text;

            if (TryCreateFolder(txtMergeFolder.Text))
                Global.Options.MergeFolder = txtMergeFolder.Text;

            if (TryCreateFolder(txtAudiobookFolder.Text))
                Global.Options.AudiobooksFolder = txtAudiobookFolder.Text;    
            
            Global.Options.RegularPrefix = txtRegularPrefix.Text;
            Global.Options.DefaultInterval = nudRegMins.Value;
            Global.Options.DeleteRippedFiles = chkDeleteRipped.Checked;
            Global.Options.DeleteMergedFiles = chkDeleteMerged.Checked;
            Global.Options.DefaultImage = myTempImage;
            Global.Options.Encoder = (Global.EncodeToTypes)cboEncoder.SelectedItem;

            Global.Options.MaxSize = nudMaxMB.Value;
            Global.Options.MaxHours = nudMaxHours.Value;
            Global.Options.ChangePrefs = chkChangePrefs.Checked;
            Global.Options.DefaultGenre = txtGenre.Text;
            Global.Options.AudioCDTemplate = txtAudioCDTemplate.Text;
            Global.Options.SeparateTemplate = txtSeparateTemplate.Text;
            Global.Options.DebugMode = chkDebugMode.Checked;
            Global.Options.CheckForNewVersion = chkCheckVersion.Checked;
            Global.Options.EjectDiscs = chkEjectDisc.Checked;
            Global.Options.CreateFoldersForAuthorAndBook = chkCreateFolders.Checked;
            Global.Options.MuteSoundWhileRipping = chkMuteSoundWhileRipping.Checked;
            Global.Options.PlaySoundOnDiscComplete = chkPlaySoundOnDiscComplete.Checked;
            Global.Options.AllowTrackSelection = chkAllowTrackSelection.Checked;
            Global.Options.AddToITunesLibrary = chkInsertIntoITunes.Checked;
        	Global.Options.BackgroundColor = labBackColor.BackColor;
            Global.Options.MaxImageSize = Convert.ToInt32(nudMaxImageSize.Value);

            switch(cboBitRate.SelectedIndex)
            {
                case 1:
                    Global.Options.BitRate = 64;
                    break;
                case 2:
                    Global.Options.BitRate = 96;
                    break;
                case 3:
                    Global.Options.BitRate = 128;
                    break;
                case 4:
                    Global.Options.BitRate = 256;
                    break;
                default:
                    Global.Options.ChangePrefs = false;
                    break;

            }

            if (cboTrackInterval.SelectedItem != null)
            {
                var ti = (TrackOrFileInterval) cboTrackInterval.SelectedItem;
                Global.Options.TrackSpacing = ti.Interval;
            }

            Global.Options.Save();

            WindowState = FormWindowState.Normal;

            DialogResult = DialogResult.OK;
        }

        private static bool TryCreateFolder(string foldername)
        {
            if (Directory.Exists(foldername))
            {
                return true;
            }
            try
            {
                Directory.CreateDirectory(foldername);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            FillCDcombo();
            FillEncoderCombo();

        	SetFormColors(Global.Options.BackgroundColor);

            //FUDGE!
            this.Height = this.labAnchor.Location.Y + (this.labAnchor.Height) * 10;


            txtTempFolder.Text = Global.Options.RipFolder;
            txtMergeFolder.Text = Global.Options.MergeFolder;
            txtAudiobookFolder.Text = Global.Options.AudiobooksFolder;
            txtRegularPrefix.Text = Global.Options.RegularPrefix;
            nudRegMins.Value = Global.Options.DefaultInterval;
            chkDeleteRipped.Checked = Global.Options.DeleteRippedFiles;
            chkDeleteMerged.Checked = Global.Options.DeleteMergedFiles;
            chkDebugMode.Checked = Global.Options.DebugMode;
            chkCheckVersion.Checked = Global.Options.CheckForNewVersion;
            chkEjectDisc.Checked = Global.Options.EjectDiscs;
            chkCreateFolders.Checked = Global.Options.CreateFoldersForAuthorAndBook;
            chkMuteSoundWhileRipping.Checked = Global.Options.MuteSoundWhileRipping;
            chkAllowTrackSelection.Checked = Global.Options.AllowTrackSelection;
            chkInsertIntoITunes.Checked = Global.Options.AddToITunesLibrary;
            if (Global.Options.MaxImageSize <= 0 || Global.Options.MaxImageSize > 4096) { Global.Options.MaxImageSize = 4096; }

            try
            {
                nudMaxImageSize.Value = Convert.ToDecimal(Global.Options.MaxImageSize);
            }
            catch
            {
                nudMaxImageSize.Value = Convert.ToDecimal(4096);
            }

            chkPlaySoundOnDiscComplete.Checked = Global.Options.PlaySoundOnDiscComplete;

            nudMaxMB.Value = Global.Options.MaxSize;
            nudMaxHours.Value = Global.Options.MaxHours;
            chkChangePrefs.Checked = Global.Options.ChangePrefs;

            txtSeparateTemplate.Text = Global.Options.SeparateTemplate;
            txtAudioCDTemplate.Text = Global.Options.AudioCDTemplate;

            switch(Global.Options.BitRate)
            {
                case 64:
                    cboBitRate.SelectedIndex = 1;
                    break;
                case 96:
                    cboBitRate.SelectedIndex = 2;
                    break;
                case 128:
                    cboBitRate.SelectedIndex = 3;
                    break;
                case 256:
                    cboBitRate.SelectedIndex = 4;
                    break;
                default:
                    cboBitRate.SelectedIndex = 0;
                    break;
            }

            if (File.Exists(Global.Options.DefaultImage))
            {
                picBookImage.Load(Global.Options.DefaultImage);
            }
            else
            {
                picBookImage.Load(Application.StartupPath + @"\" + "defaultimage.jpg");
                Global.Options.DefaultImage = Application.StartupPath + @"\" + "defaultimage.jpg";
            }

            txtGenre.Text = Global.Options.DefaultGenre;
            var desc = new Descriptor();
            desc.UseAudioCDExampleData();
            labExample1.Text = Resources.Eg_colon + desc.QualifyString(txtAudioCDTemplate.Text);
            desc.UseSeparateExampleData();
            labExample2.Text = Resources.Eg_colon + desc.QualifyString(txtSeparateTemplate.Text);

            for (int n = 1; n <= 20; n++)
            {
                cboTrackInterval.Items.Add(new TrackOrFileInterval(n));
            }
            if (Global.Options.TrackSpacing >= 0 && Global.Options.TrackSpacing <= 10)
                cboTrackInterval.SelectedIndex = Global.Options.TrackSpacing-1;
        }

    	private void SetFormColors(Color colorToSet)
    	{
    		this.BackColor = colorToSet;
    		for (int i = 0; i < tabControl1.TabPages.Count; i++)
    		{
    			var tabpage = tabControl1.TabPages[i];
				tabpage.BackColor = Global.GetDarkerColor(colorToSet, 0.9);
			}
    		labBackColor.BackColor = colorToSet;
    	}

    	private void FillEncoderCombo()
        {
            cboEncoder.Items.Clear();
            cboEncoder.Items.Add(Global.EncodeToTypes.AAC);
            cboEncoder.Items.Add(Global.EncodeToTypes.MP3);

            for (int i = 0; i < cboEncoder.Items.Count; i++)
            {
                if (Global.Options.Encoder == (Global.EncodeToTypes)cboEncoder.Items[i])
                {
                    cboEncoder.SelectedIndex = i;
                }
            }
        }

        private void FillCDcombo()
        {
            try
            {
                string[] drivesList = Directory.GetLogicalDrives();
                foreach (string drive in drivesList)
                {
                    char drivechar = drive.ToUpper()[0];
                    if (drivechar > 'C')
                        cboDrives.Items.Add(drive);
                }
            }
            catch
            {
                //MessageBox.Show("Unable to load list of drives", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            if (cboDrives.Items.Count == 0)
            {
                //load dumb list
                cboDrives.Items.Add(@"C:\");
                cboDrives.Items.Add(@"D:\");
                cboDrives.Items.Add(@"E:\");
                cboDrives.Items.Add(@"F:\");
                cboDrives.Items.Add(@"G:\");
                cboDrives.Items.Add(@"H:\");
            }

            cboDrives.SelectedIndex = 0;
            string CDDrive = Global.Options.CDDrive.ToUpper();
            for (int i1 = 0; i1 < cboDrives.Items.Count; i1++)
            {
                if ((string)cboDrives.Items[i1] == CDDrive)
                    cboDrives.SelectedIndex = i1;
            }
        }

        private void butChoosePic_Click(object sender, EventArgs e)
        {
            if (dlgOpenImage.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Image imgbig = Image.FromFile(dlgOpenImage.FileName);

                    Image imgsmall = ImageTools.ConstrainProportions(imgbig, Global.Options.MaxImageSize, ImageTools.Dimensions.Height);
                    if (imgsmall == null) { throw new InvalidOperationException(); }
                    Global.SafeImageSave(imgsmall, Global.LocalAppData() + @"\MarkAble" + @"\" + "bookimage.png", ImageFormat.Png);
                    myTempImage = Global.LocalAppData() + @"\MarkAble" + @"\" + "bookimage.png";
                    picBookImage.Image = imgsmall;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, Resources.Unable_to_load_image_exclaim);
                }
            }
        }

        private void cboEncoder_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cboEncoder.SelectedItem != null)
            //{
            //    switch ((Global.FileTypes)cboEncoder.SelectedItem)
            //    {
            //        case Global.FileTypes.AAC:
            //            tabControl1.TabPages[3].Visible = true;
            //            break;

            //        case Global.FileTypes.MP3:
            //            tabControl1.TabPages[3].Enabled = false;
            //            break;

            //    }
            //}
        }

 
        private void butBrowseTemp_Click(object sender, EventArgs e)
        {
            dlgFolder.RootFolder = Environment.SpecialFolder.MyComputer;

            if (dlgFolder.ShowDialog() == DialogResult.OK)
            {
                txtTempFolder.Text = dlgFolder.SelectedPath;
            }
        }

        private void butBrowseMerged_Click(object sender, EventArgs e)
        {
            dlgFolder.RootFolder = Environment.SpecialFolder.MyComputer;

            if (dlgFolder.ShowDialog() == DialogResult.OK)
            {
                txtMergeFolder.Text = dlgFolder.SelectedPath;
            }

        }

        private void butBrowseAudiobooks_Click(object sender, EventArgs e)
        {
            dlgFolder.RootFolder = Environment.SpecialFolder.MyComputer;

            if (dlgFolder.ShowDialog() == DialogResult.OK)
            {
                txtAudiobookFolder.Text = dlgFolder.SelectedPath;
            }
        }

        private void txtAudioCDTemplate_TextChanged(object sender, EventArgs e)
        {
            var desc = new Descriptor();
            desc.UseAudioCDExampleData();
            labExample1.Text = Resources.Eg_colon + desc.QualifyString(txtAudioCDTemplate.Text);
        }

        private void txtSeparateTemplate_TextChanged(object sender, EventArgs e)
        {
            var desc = new Descriptor();
            desc.UseSeparateExampleData();
            labExample2.Text = Resources.Eg_colon + desc.QualifyString(txtSeparateTemplate.Text);
        }

        private void cboBitRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            int previousRate = Global.Options.BitRate;

            switch (cboBitRate.SelectedIndex)
            {
                case 1:
                    Global.Options.BitRate = 64;
                    chkChangePrefs.Checked = true;
                    break;
                case 2:
                    Global.Options.BitRate = 96;
                    chkChangePrefs.Checked = true;
                    break;
                case 3:
                    Global.Options.BitRate = 128;
                    chkChangePrefs.Checked = true;
                    break;
                case 4:
                    Global.Options.BitRate = 256;
                    chkChangePrefs.Checked = true;
                    break;
                default:
                    Global.Options.BitRate = 0;
                    Global.Options.ChangePrefs = false;
                    chkChangePrefs.Checked = false;
                    break;

            }

            //if (Global.Options.ChangePrefs && (previousRate != Global.Options.BitRate) && !Prefs.RateIsAlreadySet(Global.Options.BitRate))
            //{
            //    if (MessageBox.Show(
            //            Resources.Do_you_want_MarkAble_to_change_the_iTunes_encoder_rate_now_query
            //            + Resources.If_so__and_if_iTunes_is_running_quit_iTunes
            //            + Resources.Otherwise__click__No__now,
            //            Resources.Change_now_query, MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
            //        DialogResult.Yes)
            //    {
            //        Prefs.SetCustomRate();
            //    }
            //}

        }

        private void butLoadSound_Click(object sender, EventArgs e)
        {
            GetDiscCompleteSoundFile();
        }

        private void GetDiscCompleteSoundFile()
        {
            dlgOpenSound.Title = Resources.Choose_a_short_sound_file;
            if ((!String.IsNullOrEmpty(Global.Options.DiscCompleteSound)) && Directory.Exists(Global.GetPath(Global.Options.DiscCompleteSound)))
            {
                dlgOpenSound.FileName = Global.GetFilename(Global.Options.DiscCompleteSound);
                dlgOpenSound.InitialDirectory = Global.GetPath(Global.Options.DiscCompleteSound);
            }
            else
            {
                dlgOpenSound.InitialDirectory = Global.MyDocuments();
            }

            if (dlgOpenSound.ShowDialog() == DialogResult.OK)
            {
                Global.Options.DiscCompleteSound = dlgOpenSound.FileName;
                chkPlaySoundOnDiscComplete.Checked = true;
            }
            else
            {
                Global.Options.DiscCompleteSound = string.Empty;
                chkPlaySoundOnDiscComplete.Checked = false;
            }
        }

        private SoundPlayer mySoundPlayer;

        private void butPlaySound_Click(object sender, EventArgs e)
        {
            if (mySoundPlayer != null)
            {
                mySoundPlayer.Close();
                mySoundPlayer = null;
            }

            if (!String.IsNullOrEmpty(Global.Options.DiscCompleteSound))
            {
                mySoundPlayer = new SoundPlayer();
                mySoundPlayer.Open(Global.Options.DiscCompleteSound);
                mySoundPlayer.Play();
            }
        }

        private void frmOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mySoundPlayer != null)
            {
                mySoundPlayer.Close();
                mySoundPlayer = null;
            }
        }

        private void cboTrackInterval_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

		private void labBackColor_Click(object sender, EventArgs e)
		{
			var f = new ColorDialog {SolidColorOnly = true, FullOpen = true, Color = labBackColor.BackColor};
			f.ShowDialog();
			SetFormColors(f.Color);
		}
    }
}