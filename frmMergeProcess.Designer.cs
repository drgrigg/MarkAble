namespace MarkAble2
{
    partial class frmMergeProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMergeProcess));
            this.label2 = new System.Windows.Forms.Label();
            this.labBook = new System.Windows.Forms.Label();
            this.progMergeHeaders = new System.Windows.Forms.ProgressBar();
            this.labDisc = new System.Windows.Forms.Label();
            this.progWriteMerged = new System.Windows.Forms.ProgressBar();
            this.labChapters = new System.Windows.Forms.Label();
            this.labWriteChapterData = new System.Windows.Forms.Label();
            this.progWriteChapters = new System.Windows.Forms.ProgressBar();
            this.timerProcess = new System.Windows.Forms.Timer(this.components);
            this.labBookTitle = new System.Windows.Forms.Label();
            this.labFileNum = new System.Windows.Forms.Label();
            this.butDone = new System.Windows.Forms.Button();
            this.labCurrentState = new System.Windows.Forms.Label();
            this.labDone = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labBook
            // 
            resources.ApplyResources(this.labBook, "labBook");
            this.labBook.Name = "labBook";
            // 
            // progMergeHeaders
            // 
            this.progMergeHeaders.ForeColor = System.Drawing.Color.SaddleBrown;
            resources.ApplyResources(this.progMergeHeaders, "progMergeHeaders");
            this.progMergeHeaders.Name = "progMergeHeaders";
            // 
            // labDisc
            // 
            resources.ApplyResources(this.labDisc, "labDisc");
            this.labDisc.Name = "labDisc";
            // 
            // progWriteMerged
            // 
            this.progWriteMerged.ForeColor = System.Drawing.Color.SaddleBrown;
            resources.ApplyResources(this.progWriteMerged, "progWriteMerged");
            this.progWriteMerged.Name = "progWriteMerged";
            // 
            // labChapters
            // 
            resources.ApplyResources(this.labChapters, "labChapters");
            this.labChapters.Name = "labChapters";
            // 
            // labWriteChapterData
            // 
            resources.ApplyResources(this.labWriteChapterData, "labWriteChapterData");
            this.labWriteChapterData.Name = "labWriteChapterData";
            // 
            // progWriteChapters
            // 
            this.progWriteChapters.ForeColor = System.Drawing.Color.SaddleBrown;
            resources.ApplyResources(this.progWriteChapters, "progWriteChapters");
            this.progWriteChapters.Name = "progWriteChapters";
            // 
            // timerProcess
            // 
            this.timerProcess.Interval = 1000;
            this.timerProcess.Tick += new System.EventHandler(this.timerProcess_Tick);
            // 
            // labBookTitle
            // 
            resources.ApplyResources(this.labBookTitle, "labBookTitle");
            this.labBookTitle.Name = "labBookTitle";
            // 
            // labFileNum
            // 
            resources.ApplyResources(this.labFileNum, "labFileNum");
            this.labFileNum.Name = "labFileNum";
            // 
            // butDone
            // 
            this.butDone.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.butDone, "butDone");
            this.butDone.Name = "butDone";
            this.butDone.UseVisualStyleBackColor = true;
            this.butDone.Click += new System.EventHandler(this.butDone_Click);
            // 
            // labCurrentState
            // 
            resources.ApplyResources(this.labCurrentState, "labCurrentState");
            this.labCurrentState.Name = "labCurrentState";
            // 
            // labDone
            // 
            resources.ApplyResources(this.labDone, "labDone");
            this.labDone.Name = "labDone";
            // 
            // frmMergeProcess
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.labDone);
            this.Controls.Add(this.labCurrentState);
            this.Controls.Add(this.butDone);
            this.Controls.Add(this.labBookTitle);
            this.Controls.Add(this.labFileNum);
            this.Controls.Add(this.labWriteChapterData);
            this.Controls.Add(this.progWriteChapters);
            this.Controls.Add(this.labChapters);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labBook);
            this.Controls.Add(this.progMergeHeaders);
            this.Controls.Add(this.labDisc);
            this.Controls.Add(this.progWriteMerged);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMergeProcess";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.frmMergeProcess_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMergeProcess_FormClosing);
            this.Load += new System.EventHandler(this.frmMergeProcess_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labBook;
        private System.Windows.Forms.ProgressBar progMergeHeaders;
        private System.Windows.Forms.Label labDisc;
        private System.Windows.Forms.ProgressBar progWriteMerged;
        private System.Windows.Forms.Label labChapters;
        private System.Windows.Forms.Label labWriteChapterData;
        private System.Windows.Forms.ProgressBar progWriteChapters;
        private System.Windows.Forms.Timer timerProcess;
        private System.Windows.Forms.Label labBookTitle;
        private System.Windows.Forms.Label labFileNum;
        private System.Windows.Forms.Button butDone;
        private System.Windows.Forms.Label labCurrentState;
        private System.Windows.Forms.Label labDone;
    }
}