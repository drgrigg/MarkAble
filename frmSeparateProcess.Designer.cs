namespace MarkAble2
{
    partial class frmSeparateProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeparateProcess));
            this.label1 = new System.Windows.Forms.Label();
            this.labBookTitle = new System.Windows.Forms.Label();
            this.timerStart = new System.Windows.Forms.Timer(this.components);
            this.progTrack = new System.Windows.Forms.ProgressBar();
            this.labTrack = new System.Windows.Forms.Label();
            this.labBook = new System.Windows.Forms.Label();
            this.progBook = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.labPrologue = new System.Windows.Forms.Label();
            this.labPrompt = new System.Windows.Forms.Label();
            this.butCancel = new System.Windows.Forms.Button();
            this.timerPoll = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labBookTitle
            // 
            resources.ApplyResources(this.labBookTitle, "labBookTitle");
            this.labBookTitle.Name = "labBookTitle";
            // 
            // timerStart
            // 
            this.timerStart.Tick += new System.EventHandler(this.timerStart_Tick);
            // 
            // progTrack
            // 
            this.progTrack.ForeColor = System.Drawing.Color.SaddleBrown;
            resources.ApplyResources(this.progTrack, "progTrack");
            this.progTrack.Name = "progTrack";
            // 
            // labTrack
            // 
            resources.ApplyResources(this.labTrack, "labTrack");
            this.labTrack.Name = "labTrack";
            // 
            // labBook
            // 
            resources.ApplyResources(this.labBook, "labBook");
            this.labBook.Name = "labBook";
            // 
            // progBook
            // 
            this.progBook.ForeColor = System.Drawing.Color.SaddleBrown;
            resources.ApplyResources(this.progBook, "progBook");
            this.progBook.Name = "progBook";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // labPrologue
            // 
            resources.ApplyResources(this.labPrologue, "labPrologue");
            this.labPrologue.Name = "labPrologue";
            // 
            // labPrompt
            // 
            resources.ApplyResources(this.labPrompt, "labPrompt");
            this.labPrompt.Name = "labPrompt";
            // 
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click_1);
            // 
            // timerPoll
            // 
            this.timerPoll.Interval = 120000;
            // 
            // frmSeparateProcess
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.labPrompt);
            this.Controls.Add(this.labPrologue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labBook);
            this.Controls.Add(this.progBook);
            this.Controls.Add(this.labTrack);
            this.Controls.Add(this.progTrack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labBookTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSeparateProcess";
            this.Activated += new System.EventHandler(this.frmSeparateProcess_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSeparateProcess_FormClosing);
            this.Load += new System.EventHandler(this.frmSeparateProcess_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labBookTitle;
        private System.Windows.Forms.Timer timerStart;
        private System.Windows.Forms.ProgressBar progTrack;
        private System.Windows.Forms.Label labTrack;
        private System.Windows.Forms.Label labBook;
        private System.Windows.Forms.ProgressBar progBook;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labPrologue;
        private System.Windows.Forms.Label labPrompt;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Timer timerPoll;
    }
}