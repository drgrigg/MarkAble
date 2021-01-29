using System;
using System.Windows.Forms;
using MarkAble2.Properties;

namespace MarkAble2
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void butRegister_Click(object sender, EventArgs e)
        {
            if (Registration.ConfirmReg(txtRegistrationName.Text, txtRegistrationKey.Text))
            {
                Registration.Register(txtRegistrationName.Text);

                MessageBox.Show(Resources.Thanks_for_registering, Resources.Success_exclaim, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                string[] blocks = txtRegistrationKey.Text.Split('-');
                if (blocks.Length == 4) 
                {
                    try
                    {
                        int blocknum = Convert.ToInt32(blocks[0]);
                        //if we get here, first block was a number, so an earlier registration number

                        var gotopage = Resources.Please_visit_webpage_for_more_information.Replace("<webpage>", "http://rightword.com.au/products/markable/newversion.asp");

                        MessageBox.Show(
                            Resources.You_appear_to_have_entered_a_registration_key_for_an_earlier_version_of_MarkAble + "\r\n\r\n" + Resources.MarkAble_has_been_updated_and_you_will_require_a_new_registration_key + "\r\n\r\n." + gotopage, Resources.Sorry_exclaim, MessageBoxButtons.OK,MessageBoxIcon.Information);
                        return;
                    }
                    catch
                    {
                        //do nowt
                    }
                }

                MessageBox.Show(Resources.Invalid_registration_key_exclaim, Resources.Sorry_exclaim, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}