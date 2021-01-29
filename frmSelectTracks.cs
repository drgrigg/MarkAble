using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MarkAble2
{
    public partial class frmSelectTracks : Form
    {
        public frmSelectTracks()
        {
            InitializeComponent();
        }

        private TrackList myTracks;

        public frmSelectTracks(TrackList tracks)
        {
            InitializeComponent();
            myTracks = tracks;
        }

        private void frmSelectTracks_Load(object sender, EventArgs e)
        {
            lstTracks.Items.Clear();

            for (int i = 0; i < myTracks.Count; i++)
            {
                var track = myTracks[i];
                lstTracks.Items.Add(i.ToString("00") + " " + track.Name);
            }

            CheckAll();
        }

        private void CheckAll()
        {
            for (int i = 0; i < lstTracks.Items.Count; i++)
            {
                lstTracks.SetItemChecked(i,true);
            }
        }

        private void UnCheckAll()
        {
            for (int i = 0; i < lstTracks.Items.Count; i++)
            {
                lstTracks.SetItemChecked(i, false);
            }
        }

        private void butCheckAll_Click(object sender, EventArgs e)
        {
            CheckAll();
        }

        private void butCheckNone_Click(object sender, EventArgs e)
        {
            UnCheckAll();
        }

        private void butNext_Click(object sender, EventArgs e)
        {
            for (int i = lstTracks.Items.Count -1; i >= 0; i--)
            {
                if (!lstTracks.GetItemChecked(i))
                {
                    myTracks.RemoveAt(i);
                }
            }
            DialogResult = DialogResult.OK;
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void chkStopShowing_CheckedChanged(object sender, EventArgs e)
        {
            Global.Options.AllowTrackSelection = !chkStopShowing.Checked; //note the negative here.
            Global.Options.Save();
        }
    }
}
