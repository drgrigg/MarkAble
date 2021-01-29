using System;
using System.IO;
using System.Windows.Forms;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmCDinfo : Form
    {
        public frmCDinfo()
        {
            InitializeComponent();
        }

        private void butStart_Click(object sender, EventArgs e)
        {           
            if (!String.IsNullOrEmpty(cboDrives.Text))
            {
                Global.Options.CDDrive = cboDrives.Text;
            }
            else
            {
                MessageBox.Show(Resources.Please_set_the_CD_drive_in_Step_1, Resources.Not_ready, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Global.RippedCDFiles.TotalDiscs = (int)nudNumCDs.Value;

            if (txtTitle.Text.Trim() != "")
            {
                Global.RippedCDFiles.BookTitle = txtTitle.Text;
            }
            else
            {
                MessageBox.Show(Resources.Please_set_the_book_s_title_in_Step_3, Resources.Not_ready, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtAuthor.Text.Trim() != "")
            {
                Global.RippedCDFiles.Author = txtAuthor.Text;
            }
            else
            {
                MessageBox.Show(Resources.Please_set_the_author_s_name_in_Step_4, Resources.Not_ready, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            } 
            
            Global.CurrentDiscNum = 1;
            Global.ProcessingFirstCd = true;

            Global.WriteRippedFilelist();

            WindowState = FormWindowState.Normal;

            this.DialogResult = DialogResult.OK;
        }

        private void frmCDinfo_Load(object sender, EventArgs e)
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

            labReady.Text =
                Resources.We_ll_now_start_to_convert_the_tracks_from_each_disc_to_an__encodertype__format;

            labReady.Text = labReady.Text.Replace("<encodertype>", Global.Options.Encoder.ToString());
 
        }

        private bool DoneIt;
        private void frmCDinfo_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;
                //Global.ResizeForDPI(this);
            }
        }


    }
}