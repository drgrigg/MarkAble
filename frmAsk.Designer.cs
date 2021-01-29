namespace MarkAble2
{
    partial class frmAsk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAsk));
            this.labPrompt = new System.Windows.Forms.Label();
            this.butYes = new System.Windows.Forms.Button();
            this.butNo = new System.Windows.Forms.Button();
            this.chkDontAsk = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labPrompt
            // 
            resources.ApplyResources(this.labPrompt, "labPrompt");
            this.labPrompt.Name = "labPrompt";
            // 
            // butYes
            // 
            resources.ApplyResources(this.butYes, "butYes");
            this.butYes.Name = "butYes";
            this.butYes.UseVisualStyleBackColor = true;
            this.butYes.Click += new System.EventHandler(this.butYes_Click);
            // 
            // butNo
            // 
            resources.ApplyResources(this.butNo, "butNo");
            this.butNo.Name = "butNo";
            this.butNo.UseVisualStyleBackColor = true;
            this.butNo.Click += new System.EventHandler(this.butNo_Click);
            // 
            // chkDontAsk
            // 
            resources.ApplyResources(this.chkDontAsk, "chkDontAsk");
            this.chkDontAsk.Name = "chkDontAsk";
            this.chkDontAsk.UseVisualStyleBackColor = true;
            this.chkDontAsk.CheckedChanged += new System.EventHandler(this.chkDontAsk_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MarkAble2.Properties.Resources.question_icon;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // frmAsk
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.chkDontAsk);
            this.Controls.Add(this.butNo);
            this.Controls.Add(this.butYes);
            this.Controls.Add(this.labPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAsk";
            this.Activated += new System.EventHandler(this.frmAsk_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labPrompt;
        private System.Windows.Forms.Button butYes;
        private System.Windows.Forms.Button butNo;
        private System.Windows.Forms.CheckBox chkDontAsk;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}