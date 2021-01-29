using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmSeparate2 : Form
    {
        public frmSeparate2()
        {
            InitializeComponent();
        }


        private void FillList()
        {
            lstFiles.Items.Clear();

            FileRecord.StringTypes stringType = FileRecord.StringTypes.FileName;

            //sorting:
            if (radByFileName.Checked)
            {
                Global.UnRippedFiles.SortByFileName();
                stringType = FileRecord.StringTypes.FileName;
            }
            if (radByPath.Checked)
            {
                Global.UnRippedFiles.SortByPath();
                stringType = FileRecord.StringTypes.Trimmed;
            }
            if (radManual.Checked)
            {
                stringType = FileRecord.StringTypes.Trimmed;
            }

            for (int i = 0; i < Global.UnRippedFiles.Count; i++)
            {
                var frec = Global.UnRippedFiles[i];
                frec.StringType = stringType;
                lstFiles.Items.Add(frec);
            }
        }


        private void frmSeparate2_Load(object sender, EventArgs e)
        {
            FillList();
        }

        private void radManual_CheckedChanged(object sender, EventArgs e)
        {
            butUp.Visible = radManual.Checked;
            butDown.Visible = radManual.Checked;
            FillList();

        }

        private void radAlphaNumeric_CheckedChanged(object sender, EventArgs e)
        {
            FillList();
        }

        private void radAlpha_CheckedChanged(object sender, EventArgs e)
        {
            FillList();
        }

        private void butNext_Click(object sender, EventArgs e)
        {
            Global.WriteSeparateList(SeparateFileList.ProcessStages.Sorted);
            DialogResult = DialogResult.OK;
        }


        private void butUp_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex < 1) return;

            int currentSelection = lstFiles.SelectedIndex;

            FileRecord frecToMove = (FileRecord)lstFiles.SelectedItem;

            Global.UnRippedFiles.MoveUp(frecToMove);

            FillList();

            lstFiles.SelectedIndex = currentSelection-1;
            

        }

        private void butDown_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedIndex > (lstFiles.Items.Count -2)) return;

            int currentSelection = lstFiles.SelectedIndex;

            FileRecord frecToMove = (FileRecord)lstFiles.SelectedItem;

            Global.UnRippedFiles.MoveDown(frecToMove);

            FillList();

            lstFiles.SelectedIndex = currentSelection + 1;
            

        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            Global.WriteSeparateList(SeparateFileList.ProcessStages.Selected); //if hit cancel, we're one step behind.

            //report stuff about resume
            WindowState = FormWindowState.Normal;
            MessageBox.Show(Resources.Cancelled + " " + Resources.However_you_can_resume_converting_this_book_at_a_later_time_if_you_wish,
                Resources.Cancelled);
            
            DialogResult = DialogResult.Cancel;
        }
    }
}
