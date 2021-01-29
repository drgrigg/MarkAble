using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MarkAble2
{
	/// <summary>
	/// Summary description for frmSplash.
	/// </summary>
	public class frmXSplash : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox picMarkAble;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label labVersion;
		private System.Windows.Forms.Label labReg;
		private System.ComponentModel.IContainer components;

		public frmXSplash()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmXSplash));
            this.picMarkAble = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblVersion = new System.Windows.Forms.Label();
            this.labVersion = new System.Windows.Forms.Label();
            this.labReg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picMarkAble)).BeginInit();
            this.SuspendLayout();
            // 
            // picMarkAble
            // 
            this.picMarkAble.Image = ((System.Drawing.Image)(resources.GetObject("picMarkAble.Image")));
            this.picMarkAble.Location = new System.Drawing.Point(16, 16);
            this.picMarkAble.Name = "picMarkAble";
            this.picMarkAble.Size = new System.Drawing.Size(160, 160);
            this.picMarkAble.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMarkAble.TabIndex = 0;
            this.picMarkAble.TabStop = false;
            this.picMarkAble.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(175, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(267, 34);
            this.label1.TabIndex = 1;
            this.label1.Text = "MarkAble";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(183, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(259, 24);
            this.label2.TabIndex = 2;
            this.label2.Text = "A product of Rightword Enterprises and iPodSoft";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(88, 72);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(208, 16);
            this.lblVersion.TabIndex = 3;
            // 
            // labVersion
            // 
            this.labVersion.Location = new System.Drawing.Point(183, 64);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(259, 24);
            this.labVersion.TabIndex = 4;
            this.labVersion.Text = "Version: ";
            this.labVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labVersion.Click += new System.EventHandler(this.labVersion_Click);
            // 
            // labReg
            // 
            this.labReg.Location = new System.Drawing.Point(183, 112);
            this.labReg.Name = "labReg";
            this.labReg.Size = new System.Drawing.Size(259, 81);
            this.labReg.TabIndex = 5;
            this.labReg.Text = "Registered to:";
            this.labReg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labReg.Click += new System.EventHandler(this.labReg_Click);
            // 
            // frmSplash
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.LemonChiffon;
            this.ClientSize = new System.Drawing.Size(446, 202);
            this.Controls.Add(this.picMarkAble);
            this.Controls.Add(this.labReg);
            this.Controls.Add(this.labVersion);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSplash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About MarkAble";
            this.Load += new System.EventHandler(this.frmSplash_Load);
            this.Click += new System.EventHandler(this.frmSplash_Click);
            ((System.ComponentModel.ISupportInitialize)(this.picMarkAble)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmSplash_Load(object sender, System.EventArgs e)
		{
			labVersion.Text = "Version: " + Global.myVersion;
			if (Shareware.IsRegistered()) 
			{ 
				labReg.Text = "Registered to: " + Shareware.RegisteredTo();
				timer1.Enabled = true;
				return;
			}
			else
			{
				DateTime regDate;
				regDate = Shareware.GetStartDate();
				Shareware.IsExhausted = false;
				if (regDate.Year == 1900) // no date yet set
				{
					Global.ErrorLog.Remove(0,Global.ErrorLog.Length);
					Global.anyErrors = false;
					Shareware.SetStartDate(DateTime.Today);
					regDate = DateTime.Today;
					if (Global.anyErrors)
						MessageBox.Show(Global.ErrorLog.ToString(),"Error",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Exclamation);
					Global.anyErrors = false;
				}
				if ((DateTime.Today - regDate) > TimeSpan.FromDays(Global.EvaluationDays))
				{
					labReg.Text = "Unregistered and past evaluation date!";
					Shareware.IsExhausted = true;
				}
				else
				{
					TimeSpan spent;
					spent = (DateTime.Today - regDate);
					labReg.Text = "Only " + (Global.EvaluationDays - spent.Days).ToString() + " days remaining to evaluate.  Please register!";
				}
				timer1.Enabled = true;
			}
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmSplash_Click(object sender, System.EventArgs e)
		{
			if (Shareware.IsRegistered())
				this.Close();
		}

		private void label1_Click(object sender, System.EventArgs e)
		{
			if (Shareware.IsRegistered())
				this.Close();

		}

		private void labVersion_Click(object sender, System.EventArgs e)
		{
			if (Shareware.IsRegistered())
				this.Close();

		}

		private void label2_Click(object sender, System.EventArgs e)
		{
			if (Shareware.IsRegistered())
				this.Close();

		}

		private void labReg_Click(object sender, System.EventArgs e)
		{
			if (Shareware.IsRegistered())
				this.Close();

		}

		private void pictureBox1_Click(object sender, System.EventArgs e)
		{
			if (Shareware.IsRegistered())
				this.Close();
		}
	}
}
