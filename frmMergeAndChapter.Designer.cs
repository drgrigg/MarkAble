namespace MarkAble2
{
    partial class frmMergeAndChapter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMergeAndChapter));
            this.labBookTitle = new System.Windows.Forms.Label();
            this.labPrompt = new System.Windows.Forms.Label();
            this.nudFiles = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.labFileBreakdown = new System.Windows.Forms.Label();
            this.labStep7 = new System.Windows.Forms.Label();
            this.butChoosePic = new System.Windows.Forms.Button();
            this.butStart = new System.Windows.Forms.Button();
            this.labchap7prompt = new System.Windows.Forms.Label();
            this.radChapsByTrack = new System.Windows.Forms.RadioButton();
            this.radChapsByTime = new System.Windows.Forms.RadioButton();
            this.nudRegMins = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.radChapsNone = new System.Windows.Forms.RadioButton();
            this.dlgOpenImage = new System.Windows.Forms.OpenFileDialog();
            this.butCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.picBookImage = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboTrackInterval = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.radDiscOnly = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegMins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBookImage)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labBookTitle
            // 
            resources.ApplyResources(this.labBookTitle, "labBookTitle");
            this.labBookTitle.Name = "labBookTitle";
            // 
            // labPrompt
            // 
            resources.ApplyResources(this.labPrompt, "labPrompt");
            this.labPrompt.Name = "labPrompt";
            // 
            // nudFiles
            // 
            resources.ApplyResources(this.nudFiles, "nudFiles");
            this.nudFiles.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFiles.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFiles.Name = "nudFiles";
            this.nudFiles.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFiles.ValueChanged += new System.EventHandler(this.nudFiles_ValueChanged);
            this.nudFiles.Validating += new System.ComponentModel.CancelEventHandler(this.nudFiles_Validating);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labFileBreakdown
            // 
            resources.ApplyResources(this.labFileBreakdown, "labFileBreakdown");
            this.labFileBreakdown.Name = "labFileBreakdown";
            // 
            // labStep7
            // 
            resources.ApplyResources(this.labStep7, "labStep7");
            this.labStep7.Name = "labStep7";
            // 
            // butChoosePic
            // 
            resources.ApplyResources(this.butChoosePic, "butChoosePic");
            this.butChoosePic.Name = "butChoosePic";
            this.butChoosePic.UseVisualStyleBackColor = true;
            this.butChoosePic.Click += new System.EventHandler(this.butChoosePic_Click);
            // 
            // butStart
            // 
            resources.ApplyResources(this.butStart, "butStart");
            this.butStart.Name = "butStart";
            this.butStart.UseVisualStyleBackColor = true;
            this.butStart.Click += new System.EventHandler(this.butStart_Click);
            // 
            // labchap7prompt
            // 
            resources.ApplyResources(this.labchap7prompt, "labchap7prompt");
            this.labchap7prompt.Name = "labchap7prompt";
            // 
            // radChapsByTrack
            // 
            resources.ApplyResources(this.radChapsByTrack, "radChapsByTrack");
            this.radChapsByTrack.Name = "radChapsByTrack";
            this.radChapsByTrack.UseVisualStyleBackColor = true;
            // 
            // radChapsByTime
            // 
            resources.ApplyResources(this.radChapsByTime, "radChapsByTime");
            this.radChapsByTime.Checked = true;
            this.radChapsByTime.Name = "radChapsByTime";
            this.radChapsByTime.TabStop = true;
            this.radChapsByTime.UseVisualStyleBackColor = true;
            // 
            // nudRegMins
            // 
            resources.ApplyResources(this.nudRegMins, "nudRegMins");
            this.nudRegMins.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudRegMins.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRegMins.Name = "nudRegMins";
            this.nudRegMins.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // radChapsNone
            // 
            resources.ApplyResources(this.radChapsNone, "radChapsNone");
            this.radChapsNone.Name = "radChapsNone";
            this.radChapsNone.UseVisualStyleBackColor = true;
            // 
            // dlgOpenImage
            // 
            resources.ApplyResources(this.dlgOpenImage, "dlgOpenImage");
            // 
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // picBookImage
            // 
            this.picBookImage.BackColor = System.Drawing.Color.Black;
            this.picBookImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.picBookImage, "picBookImage");
            this.picBookImage.Name = "picBookImage";
            this.picBookImage.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboTrackInterval);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.radDiscOnly);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.radChapsByTrack);
            this.panel1.Controls.Add(this.radChapsByTime);
            this.panel1.Controls.Add(this.radChapsNone);
            this.panel1.Controls.Add(this.nudRegMins);
            this.panel1.Controls.Add(this.label4);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // cboTrackInterval
            // 
            this.cboTrackInterval.FormattingEnabled = true;
            resources.ApplyResources(this.cboTrackInterval, "cboTrackInterval");
            this.cboTrackInterval.Name = "cboTrackInterval";
            this.cboTrackInterval.SelectedIndexChanged += new System.EventHandler(this.cboTrackInterval_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // radDiscOnly
            // 
            resources.ApplyResources(this.radDiscOnly, "radDiscOnly");
            this.radDiscOnly.Name = "radDiscOnly";
            this.radDiscOnly.UseVisualStyleBackColor = true;
            // 
            // frmMergeAndChapter
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.labchap7prompt);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butStart);
            this.Controls.Add(this.butChoosePic);
            this.Controls.Add(this.picBookImage);
            this.Controls.Add(this.labStep7);
            this.Controls.Add(this.labFileBreakdown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudFiles);
            this.Controls.Add(this.labPrompt);
            this.Controls.Add(this.labBookTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMergeAndChapter";
            this.Activated += new System.EventHandler(this.frmMergeAndChapter_Activated);
            this.Load += new System.EventHandler(this.frmMergeAndChapter_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMergeAndChapter_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMergeAndChapter_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.nudFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRegMins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBookImage)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labBookTitle;
        private System.Windows.Forms.Label labPrompt;
        private System.Windows.Forms.NumericUpDown nudFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labFileBreakdown;
        private System.Windows.Forms.Label labStep7;
        private System.Windows.Forms.PictureBox picBookImage;
        private System.Windows.Forms.Button butChoosePic;
        private System.Windows.Forms.Button butStart;
        private System.Windows.Forms.Label labchap7prompt;
        private System.Windows.Forms.RadioButton radChapsByTrack;
        private System.Windows.Forms.RadioButton radChapsByTime;
        private System.Windows.Forms.NumericUpDown nudRegMins;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radChapsNone;
        private System.Windows.Forms.OpenFileDialog dlgOpenImage;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radDiscOnly;
        private System.Windows.Forms.ComboBox cboTrackInterval;
        private System.Windows.Forms.Label label2;
    }
}