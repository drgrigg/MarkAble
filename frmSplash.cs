using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmSplash : Form
    {
        public frmSplash()
        {
            InitializeComponent();
        }

        private bool DoneIt = false;

        private void frmSplash_Activated(object sender, EventArgs e)
        {
            if (!DoneIt)
            {
                DoneIt = true;

                labVersion.Text = "Version " + Global.Version;
                labRegistered.Text = "";

                Registration.EvaluationType = Registration.EvaluationTypes.EvaluationPeriod;

                if (Registration.IsRegistered())
                {
                    butContinue.Visible = false;
                    butRegisterNow.Visible = false;

                    labRegistered.Text = Resources.Registered_to + " " + Registration.RegisteredTo();
                    timerClose.Interval = 2000;
                    timerClose.Start();
                }
                else
                {
                    butRegisterNow.Visible = true;
                    butContinue.Visible = true;

                    if (Registration.IsExhausted())
                    {
                        labRegistered.Text = Resources.Evaluation_expired;
                    }
                    else //not exhausted, but going to....
                    {
                        var remaining = Resources.Evaluation_remaining.Replace("<days>",
                                                                               Registration.DaysRemaining().ToString());
                        labRegistered.Text = Resources.Unregistered + "\r\n\r\n" + remaining;
                    }
                }
            }
        }

        private bool CorrectlyRegistered()
        {
            frmRegister F = new frmRegister();
            if (F.ShowDialog() == DialogResult.OK)
            {
                return Registration.IsRegistered();
            }
            else
            {
                return false;
            }
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            timerClose.Stop();
            this.Close();
        }

        private void butRegisterNow_Click(object sender, EventArgs e)
        {
            if (CorrectlyRegistered())
            {
                labRegistered.Text = "Registered to: " + Registration.RegisteredTo();
                Application.DoEvents();
                timerClose.Enabled = true;
            }
        }

        private void butContinue_Click(object sender, EventArgs e)
        {
            timerClose.Enabled = true;
        }

        private void linkIPodSoft_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // launch default browser
                System.Diagnostics.Process.Start("http://www.ipodsoft.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void linkRightword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // launch default browser
                System.Diagnostics.Process.Start("http://rightword.com.au/products/markable");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}