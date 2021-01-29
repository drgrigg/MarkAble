using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MarkAble2
{
    public partial class frmAsk : Form
    {
        public frmAsk()
        {
            InitializeComponent();
        }

        private string myQuestion = "Question";
        private bool myStopAsking = false;

        public frmAsk(string question)
        {
            InitializeComponent();
            myQuestion = question;
        }

        public bool StopAsking()
        {
            return myStopAsking;
        }

        private void butYes_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        private void butNo_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private bool DoneIt;
        private void frmAsk_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;
                labPrompt.Text = myQuestion;
            }
        }

        private void chkDontAsk_CheckedChanged(object sender, EventArgs e)
        {
            myStopAsking = chkDontAsk.Checked;
        }
    }
}
