using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmBatch : Form
    {
        public frmBatch()
        {
            InitializeComponent();
        }

        private void frmBatch_Load(object sender, EventArgs e)
        {
            lstBooks.CheckOnClick = false;
            chkSkipByDefault.Text = chkSkipByDefault.Text.Replace("{type}", Global.Options.Encoder.ToString());

            FillList();

        }

        private void FillList()
        {
            if ((Global.CurrentBatch != null) && (Global.CurrentBatch.Lists.Count > 0))
            {
                labFileName.Text = Global.CurrentBatch.SourceFile;

                chkSkipByDefault.Checked = Global.CurrentBatch.SkipConversion;

                lstBooks.Items.Clear();
                foreach(SeparateFileList fl in Global.CurrentBatch.Lists)
                {
                   lstBooks.Items.Add(fl);
                }
            }
            CheckAll(true);
        }

        private void CheckAll(bool truefalse)
        {
            for (int i = 0; i < lstBooks.Items.Count; i++)
            {
                lstBooks.SetItemChecked(i, truefalse);
            }
        }

        private void butCheckAll_Click(object sender, EventArgs e)
        {
            CheckAll(true);
        }

        private void butCheckNone_Click(object sender, EventArgs e)
        {
            CheckAll(false);
        }

        private void butDelete_Click(object sender, EventArgs e)
        {
            if (lstBooks.SelectedItem != null)
            {
                var fl = (SeparateFileList) lstBooks.SelectedItem;

                if (MessageBox.Show(Resources.Are_you_sure_you_want_to_remove_this_project_permanently_from_the_batch_file, Resources.Sure_query, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Global.CurrentBatch.Lists.Remove(fl);

                    try
                    {
                        //save changed file
                        Global.ToXmlFile(Global.CurrentBatch.SourceFile, Global.CurrentBatch, false);

                        if (Global.CurrentBatch.Lists.Count == 0)
                        {
                            if (
                                MessageBox.Show(Resources.The_batch_file_is_now_empty_Delete_it_query, Resources.Empty_exclaim,
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            {
                                try
                                {
                                    File.Delete(Global.CurrentBatch.SourceFile);
                                    DialogResult = DialogResult.Cancel;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(Resources.Unable_to_delete_batch_file + "\r\n\r\n" + ex.Message, Resources.Error_exclaim);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(Resources.Unable_to_alter_batch_file + "\r\n\r\n" + ex.Message, Resources.Error_exclaim);
                    }
                    FillList();
                }
            }
        }

        private void butNext_Click(object sender, EventArgs e)
        {
            for (int i = lstBooks.Items.Count - 1; i >= 0; i--)
            {
                if (lstBooks.GetItemChecked(i))
                {
                    var fl = (SeparateFileList) lstBooks.Items[i];
                    if (!fl.AllFilesExist())
                    {
                        if (
                            MessageBox.Show(
                                               Resources.Could_not_find_some_of_the_files_in_project_booktitle.Replace("<booktitle>",fl.BookTitle) +
                                               "\r\r\r\n" +
                                               Resources.Remove_the_project_from_the_batch_query,
                                               Resources.Missing_files, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                               == DialogResult.Yes)
                        {
                            lstBooks.Items.Remove(lstBooks.Items[i]);
                            Global.CurrentBatch.Lists.Remove(fl);
                        }
                        else
                        {
                            lstBooks.SetItemChecked(i,false);
                        }
                    }
                }
            }


            //For simplicity, we temporarily remove (ie, don't save to file) the unwanted projects
            //from the current batch before going on.  If no projects left, we cancel.

            for (int i = 0; i < lstBooks.Items.Count; i++)
            {
                if (lstBooks.GetItemChecked(i) == false)
                {
                    var project = (SeparateFileList) lstBooks.Items[i];
                    Global.CurrentBatch.Lists.Remove(project);
                }
            }

            if (Global.CurrentBatch.Lists.Count == 0)
            {
                MessageBox.Show(Resources.Nothing_to_do_exclaim, Resources.Cancelled);
            }

            DialogResult = Global.CurrentBatch.Lists.Count > 0 ? DialogResult.OK : DialogResult.Cancel;
        }

        private void chkSkipByDefault_CheckedChanged(object sender, EventArgs e)
        {
            Global.CurrentBatch.SkipConversion = chkSkipByDefault.Checked;
            try
            {
                Global.ToXmlFile(Global.CurrentBatch.SourceFile, Global.CurrentBatch, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Unable_to_alter_batch_file + "\r\n\r\n" + ex.Message, Resources.Error_exclaim);
            }

        }

        private bool DoneIt;

        private void frmBatch_Activated(object sender, EventArgs e)
        {
            if(!DoneIt)
            {
                DoneIt = true;
            }
        }
    }
}
