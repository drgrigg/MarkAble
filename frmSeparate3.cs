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
    public partial class frmSeparate3 : Form
    {
        public frmSeparate3()
        {
            InitializeComponent();
        }


        private void frmSeparate3_Load(object sender, EventArgs e)
        {
            if (Global.UnRippedFiles.Count > 0)
            {
                var frec = Global.UnRippedFiles[0];

                string author = "";
                string title = "";
                string album = "";

                switch(frec.FileType)
                {
                    case Global.FileTypes.AAC:
                        break;

                    case Global.FileTypes.MP3:
                        var thisMP3 = new mp3file();
                        if (thisMP3.OpenForRead(frec.FilePath))
                        {
                            thisMP3.GetAudioData();

                            author = String.IsNullOrEmpty(thisMP3.Artist) ? "" : thisMP3.Artist;
                            title = String.IsNullOrEmpty(thisMP3.Title) ? "" : thisMP3.Title;
                            album = String.IsNullOrEmpty(thisMP3.Album) ? title : thisMP3.Album;

                            txtTitle.Text = album;
                            if (!String.IsNullOrEmpty(author))
                            {
                                txtAuthor.Text = author;
                            }
                            thisMP3.Close();
                        }
                        break;

                    case Global.FileTypes.WMA:
                        var wma = new wmaMetaData();
                        wma.GetFieldByName(frec.FilePath, "Title", out title);
                        wma.GetFieldByName(frec.FilePath, "Author", out author);
                        wma.GetFieldByName(frec.FilePath, "WM/AlbumTitle", out album);
                        break;

                    case Global.FileTypes.MP2:
                    case Global.FileTypes.WAV:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (String.IsNullOrEmpty(txtTitle.Text))
                {
                    txtTitle.Text = Global.GetParentFolderName(frec.FilePath);
                }
                if (String.IsNullOrEmpty(txtAuthor.Text))
                {
                    txtAuthor.Text = Global.GetFilename(Global.GetParentPath(frec.FilePath));
                }
                Global.UnRippedFiles.BookTitle = txtTitle.Text;
                Global.UnRippedFiles.Author = txtAuthor.Text;
            }
        }


        private void butNext_Click(object sender, EventArgs e)
        {

            if (txtTitle.Text.Trim() != "")
            {
                Global.UnRippedFiles.BookTitle = txtTitle.Text;
            }
            else
            {
                MessageBox.Show(Resources.Please_set_the_book_s_title_in_Step_3, Resources.Not_ready, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (txtAuthor.Text.Trim() != "")
            {
                Global.UnRippedFiles.Author = txtAuthor.Text;
            }
            else
            {
                this.Text = Resources.Please_set_the_creator_s_name_in_Step_4;

                MessageBox.Show(Resources.Please_set_the_creator_s_name_in_Step_4, Resources.Not_ready, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (radNo.Checked) //change chapter names to be filenames.
            {
                for (int i = 0; i < Global.UnRippedFiles.Count; i++)
                {
                    Global.UnRippedFiles[i].ChapterName = Global.GetFilename(Global.UnRippedFiles[i].FilePath, false);
                }
            }
            else
            {
                //this just fills in partially completed descriptions for non-MP3 files
                var desc = new Descriptor { Artist = Global.UnRippedFiles.Author, Album = Global.UnRippedFiles.BookTitle };

                for (int i = 0; i < Global.UnRippedFiles.Count; i++)
                {
                    Global.UnRippedFiles[i].ChapterName = desc.QualifyString(Global.UnRippedFiles[i].ChapterName);
                }

            }

            Global.DeleteUnknownList();
            Global.WriteSeparateList(SeparateFileList.ProcessStages.MetaDataAdded);

            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            Global.DeleteUnknownList();
            Global.WriteSeparateList(SeparateFileList.ProcessStages.Sorted); //if hit cancel, we're one step behind.

            //report stuff about resume
            WindowState = FormWindowState.Normal;
            MessageBox.Show(Resources.Cancelled + " " + Resources.However_you_can_resume_converting_this_book_at_a_later_time_if_you_wish,
                Resources.Cancelled);
            
            DialogResult = DialogResult.Cancel;
        }

        private void butSwap_Click(object sender, EventArgs e)
        {
            var temp = txtTitle.Text;
            txtTitle.Text = txtAuthor.Text;
            txtAuthor.Text = temp;
        }


    }
}
