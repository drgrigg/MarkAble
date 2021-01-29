namespace MarkAble2
{
    partial class frmSeparate2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeparate2));
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.butNext = new System.Windows.Forms.Button();
            this.dlgSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgSelectFiles = new System.Windows.Forms.OpenFileDialog();
            this.radByFileName = new System.Windows.Forms.RadioButton();
            this.radByPath = new System.Windows.Forms.RadioButton();
            this.radManual = new System.Windows.Forms.RadioButton();
            this.butCancel = new System.Windows.Forms.Button();
            this.butUp = new System.Windows.Forms.Button();
            this.butDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstFiles
            // 
            this.lstFiles.AllowDrop = true;
            this.lstFiles.FormattingEnabled = true;
            resources.ApplyResources(this.lstFiles, "lstFiles");
            this.lstFiles.Name = "lstFiles";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // butNext
            // 
            resources.ApplyResources(this.butNext, "butNext");
            this.butNext.Name = "butNext";
            this.butNext.UseVisualStyleBackColor = true;
            this.butNext.Click += new System.EventHandler(this.butNext_Click);
            // 
            // dlgSelectFolder
            // 
            resources.ApplyResources(this.dlgSelectFolder, "dlgSelectFolder");
            this.dlgSelectFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.dlgSelectFolder.ShowNewFolderButton = false;
            // 
            // dlgSelectFiles
            // 
            this.dlgSelectFiles.AddExtension = false;
            resources.ApplyResources(this.dlgSelectFiles, "dlgSelectFiles");
            this.dlgSelectFiles.Multiselect = true;
            // 
            // radByFileName
            // 
            resources.ApplyResources(this.radByFileName, "radByFileName");
            this.radByFileName.Checked = true;
            this.radByFileName.Name = "radByFileName";
            this.radByFileName.TabStop = true;
            this.radByFileName.UseVisualStyleBackColor = true;
            this.radByFileName.CheckedChanged += new System.EventHandler(this.radAlphaNumeric_CheckedChanged);
            // 
            // radByPath
            // 
            resources.ApplyResources(this.radByPath, "radByPath");
            this.radByPath.Name = "radByPath";
            this.radByPath.UseVisualStyleBackColor = true;
            this.radByPath.CheckedChanged += new System.EventHandler(this.radAlpha_CheckedChanged);
            // 
            // radManual
            // 
            resources.ApplyResources(this.radManual, "radManual");
            this.radManual.Name = "radManual";
            this.radManual.UseVisualStyleBackColor = true;
            this.radManual.CheckedChanged += new System.EventHandler(this.radManual_CheckedChanged);
            // 
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butUp
            // 
            resources.ApplyResources(this.butUp, "butUp");
            this.butUp.Name = "butUp";
            this.butUp.UseVisualStyleBackColor = true;
            this.butUp.Click += new System.EventHandler(this.butUp_Click);
            // 
            // butDown
            // 
            resources.ApplyResources(this.butDown, "butDown");
            this.butDown.Name = "butDown";
            this.butDown.UseVisualStyleBackColor = true;
            this.butDown.Click += new System.EventHandler(this.butDown_Click);
            // 
            // frmSeparate2
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.butDown);
            this.Controls.Add(this.butUp);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.radManual);
            this.Controls.Add(this.radByPath);
            this.Controls.Add(this.radByFileName);
            this.Controls.Add(this.butNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSeparate2";
            this.Load += new System.EventHandler(this.frmSeparate2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butNext;
        private System.Windows.Forms.FolderBrowserDialog dlgSelectFolder;
        private System.Windows.Forms.OpenFileDialog dlgSelectFiles;
        private System.Windows.Forms.RadioButton radByFileName;
        private System.Windows.Forms.RadioButton radByPath;
        private System.Windows.Forms.RadioButton radManual;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butUp;
        private System.Windows.Forms.Button butDown;
    }
}