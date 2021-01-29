namespace MarkAble2
{
    partial class frmSeparate1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeparate1));
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.butNext = new System.Windows.Forms.Button();
            this.butSelectFiles = new System.Windows.Forms.Button();
            this.butSelectFolder = new System.Windows.Forms.Button();
            this.dlgSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgSelectFiles = new System.Windows.Forms.OpenFileDialog();
            this.butRemoveSelected = new System.Windows.Forms.Button();
            this.chkFullPath = new System.Windows.Forms.CheckBox();
            this.butCancel = new System.Windows.Forms.Button();
            this.butBatch = new System.Windows.Forms.Button();
            this.dlgBatchFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // lstFiles
            // 
            this.lstFiles.AllowDrop = true;
            this.lstFiles.FormattingEnabled = true;
            resources.ApplyResources(this.lstFiles, "lstFiles");
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstFiles.Sorted = true;
            this.lstFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lstFiles_DragDrop);
            this.lstFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lstFiles_DragEnter);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
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
            // butSelectFiles
            // 
            this.butSelectFiles.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butSelectFiles, "butSelectFiles");
            this.butSelectFiles.Name = "butSelectFiles";
            this.butSelectFiles.UseVisualStyleBackColor = true;
            this.butSelectFiles.Click += new System.EventHandler(this.butSelectFiles_Click);
            // 
            // butSelectFolder
            // 
            this.butSelectFolder.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butSelectFolder, "butSelectFolder");
            this.butSelectFolder.Name = "butSelectFolder";
            this.butSelectFolder.UseVisualStyleBackColor = true;
            this.butSelectFolder.Click += new System.EventHandler(this.butSelectFolder_Click);
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
            // butRemoveSelected
            // 
            this.butRemoveSelected.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butRemoveSelected, "butRemoveSelected");
            this.butRemoveSelected.Name = "butRemoveSelected";
            this.butRemoveSelected.UseVisualStyleBackColor = true;
            this.butRemoveSelected.Click += new System.EventHandler(this.butRemoveSelected_Click);
            // 
            // chkFullPath
            // 
            resources.ApplyResources(this.chkFullPath, "chkFullPath");
            this.chkFullPath.Name = "chkFullPath";
            this.chkFullPath.UseVisualStyleBackColor = true;
            this.chkFullPath.CheckedChanged += new System.EventHandler(this.chkFullPath_CheckedChanged);
            // 
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butBatch
            // 
            this.butBatch.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butBatch, "butBatch");
            this.butBatch.Name = "butBatch";
            this.butBatch.UseVisualStyleBackColor = true;
            this.butBatch.Click += new System.EventHandler(this.butBatch_Click);
            // 
            // dlgBatchFile
            // 
            this.dlgBatchFile.DefaultExt = "markbatch";
            this.dlgBatchFile.FileName = "*.markbatch";
            resources.ApplyResources(this.dlgBatchFile, "dlgBatchFile");
            // 
            // frmSeparate1
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.butBatch);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.chkFullPath);
            this.Controls.Add(this.butRemoveSelected);
            this.Controls.Add(this.butSelectFolder);
            this.Controls.Add(this.butSelectFiles);
            this.Controls.Add(this.butNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lstFiles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSeparate1";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmSeparate1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmSeparate1_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butNext;
        private System.Windows.Forms.Button butSelectFiles;
        private System.Windows.Forms.Button butSelectFolder;
        private System.Windows.Forms.FolderBrowserDialog dlgSelectFolder;
        private System.Windows.Forms.OpenFileDialog dlgSelectFiles;
        private System.Windows.Forms.Button butRemoveSelected;
        private System.Windows.Forms.CheckBox chkFullPath;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butBatch;
        private System.Windows.Forms.OpenFileDialog dlgBatchFile;
    }
}