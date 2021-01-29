namespace MarkAble2
{
    partial class frmSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplash));
            this.labVersion = new System.Windows.Forms.Label();
            this.labRegistered = new System.Windows.Forms.Label();
            this.butRegisterNow = new System.Windows.Forms.Button();
            this.butContinue = new System.Windows.Forms.Button();
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.linkIpodSoft = new System.Windows.Forms.LinkLabel();
            this.linkRightword = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.labTranslator = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labVersion
            // 
            resources.ApplyResources(this.labVersion, "labVersion");
            this.labVersion.BackColor = System.Drawing.Color.White;
            this.labVersion.Name = "labVersion";
            // 
            // labRegistered
            // 
            resources.ApplyResources(this.labRegistered, "labRegistered");
            this.labRegistered.BackColor = System.Drawing.Color.White;
            this.labRegistered.Name = "labRegistered";
            // 
            // butRegisterNow
            // 
            resources.ApplyResources(this.butRegisterNow, "butRegisterNow");
            this.butRegisterNow.BackColor = System.Drawing.Color.White;
            this.butRegisterNow.Name = "butRegisterNow";
            this.butRegisterNow.UseVisualStyleBackColor = false;
            this.butRegisterNow.Click += new System.EventHandler(this.butRegisterNow_Click);
            // 
            // butContinue
            // 
            resources.ApplyResources(this.butContinue, "butContinue");
            this.butContinue.BackColor = System.Drawing.Color.White;
            this.butContinue.Name = "butContinue";
            this.butContinue.UseVisualStyleBackColor = false;
            this.butContinue.Click += new System.EventHandler(this.butContinue_Click);
            // 
            // timerClose
            // 
            this.timerClose.Interval = 1000;
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // linkIpodSoft
            // 
            this.linkIpodSoft.ActiveLinkColor = System.Drawing.Color.SaddleBrown;
            resources.ApplyResources(this.linkIpodSoft, "linkIpodSoft");
            this.linkIpodSoft.BackColor = System.Drawing.Color.White;
            this.linkIpodSoft.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkIpodSoft.LinkColor = System.Drawing.Color.Black;
            this.linkIpodSoft.Name = "linkIpodSoft";
            this.linkIpodSoft.TabStop = true;
            this.linkIpodSoft.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkIPodSoft_LinkClicked);
            // 
            // linkRightword
            // 
            this.linkRightword.ActiveLinkColor = System.Drawing.Color.SaddleBrown;
            resources.ApplyResources(this.linkRightword, "linkRightword");
            this.linkRightword.BackColor = System.Drawing.Color.White;
            this.linkRightword.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkRightword.LinkColor = System.Drawing.Color.Black;
            this.linkRightword.Name = "linkRightword";
            this.linkRightword.TabStop = true;
            this.linkRightword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRightword_LinkClicked);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // labTranslator
            // 
            resources.ApplyResources(this.labTranslator, "labTranslator");
            this.labTranslator.BackColor = System.Drawing.Color.White;
            this.labTranslator.Name = "labTranslator";
            // 
            // frmSplash
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImage = global::MarkAble2.Properties.Resources.markable_spash;
            this.Controls.Add(this.labTranslator);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkRightword);
            this.Controls.Add(this.linkIpodSoft);
            this.Controls.Add(this.butContinue);
            this.Controls.Add(this.butRegisterNow);
            this.Controls.Add(this.labRegistered);
            this.Controls.Add(this.labVersion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSplash";
            this.Activated += new System.EventHandler(this.frmSplash_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labVersion;
        private System.Windows.Forms.Label labRegistered;
        private System.Windows.Forms.Button butRegisterNow;
        private System.Windows.Forms.Button butContinue;
        private System.Windows.Forms.Timer timerClose;
        private System.Windows.Forms.LinkLabel linkIpodSoft;
        private System.Windows.Forms.LinkLabel linkRightword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labTranslator;
    }
}