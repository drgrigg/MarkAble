namespace MarkAble2
{
    partial class frmIntro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIntro));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.butQuit = new System.Windows.Forms.Button();
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.butOther = new System.Windows.Forms.Button();
            this.butAudioCD = new System.Windows.Forms.Button();
            this.butPrefs = new System.Windows.Forms.Button();
            this.butHelp = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.timerResume = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // butQuit
            // 
            resources.ApplyResources(this.butQuit, "butQuit");
            this.butQuit.Name = "butQuit";
            this.butQuit.UseVisualStyleBackColor = true;
            this.butQuit.Click += new System.EventHandler(this.butQuit_Click);
            // 
            // timerClose
            // 
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // butOther
            // 
            resources.ApplyResources(this.butOther, "butOther");
            this.butOther.Image = global::MarkAble2.Properties.Resources.folders_small;
            this.butOther.Name = "butOther";
            this.butOther.UseVisualStyleBackColor = true;
            this.butOther.Click += new System.EventHandler(this.butOther_Click);
            // 
            // butAudioCD
            // 
            resources.ApplyResources(this.butAudioCD, "butAudioCD");
            this.butAudioCD.Image = global::MarkAble2.Properties.Resources.CDs_smaller;
            this.butAudioCD.Name = "butAudioCD";
            this.butAudioCD.UseVisualStyleBackColor = true;
            this.butAudioCD.Click += new System.EventHandler(this.butAudioCD_Click);
            // 
            // butPrefs
            // 
            resources.ApplyResources(this.butPrefs, "butPrefs");
            this.butPrefs.Name = "butPrefs";
            this.butPrefs.UseVisualStyleBackColor = true;
            this.butPrefs.Click += new System.EventHandler(this.butPrefs_Click);
            // 
            // butHelp
            // 
            resources.ApplyResources(this.butHelp, "butHelp");
            this.butHelp.Name = "butHelp";
            this.butHelp.UseVisualStyleBackColor = true;
            this.butHelp.Click += new System.EventHandler(this.butHelp_Click);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // timerResume
            // 
            this.timerResume.Interval = 1000;
            this.timerResume.Tick += new System.EventHandler(this.timerResume_Tick);
            // 
            // frmIntro
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.label12);
            this.Controls.Add(this.butHelp);
            this.Controls.Add(this.butPrefs);
            this.Controls.Add(this.butQuit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.butOther);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butAudioCD);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmIntro";
            this.Activated += new System.EventHandler(this.frmIntro_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmIntro_FormClosing);
            this.Load += new System.EventHandler(this.frmIntro_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butAudioCD;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button butOther;
        private System.Windows.Forms.Button butQuit;
        private System.Windows.Forms.Timer timerClose;
        private System.Windows.Forms.Button butPrefs;
        private System.Windows.Forms.Button butHelp;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Timer timerResume;
    }
}

