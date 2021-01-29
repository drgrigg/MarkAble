namespace MarkAble2
{
    partial class frmBatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatch));
            this.lstBooks = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labFileName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.butDelete = new System.Windows.Forms.Button();
            this.butCheckAll = new System.Windows.Forms.Button();
            this.butCheckNone = new System.Windows.Forms.Button();
            this.butNext = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.chkSkipByDefault = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lstBooks
            // 
            resources.ApplyResources(this.lstBooks, "lstBooks");
            this.lstBooks.FormattingEnabled = true;
            this.lstBooks.Name = "lstBooks";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // labFileName
            // 
            resources.ApplyResources(this.labFileName, "labFileName");
            this.labFileName.Name = "labFileName";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // butDelete
            // 
            resources.ApplyResources(this.butDelete, "butDelete");
            this.butDelete.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.butDelete.Name = "butDelete";
            this.butDelete.UseVisualStyleBackColor = true;
            this.butDelete.Click += new System.EventHandler(this.butDelete_Click);
            // 
            // butCheckAll
            // 
            resources.ApplyResources(this.butCheckAll, "butCheckAll");
            this.butCheckAll.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.butCheckAll.Name = "butCheckAll";
            this.butCheckAll.UseVisualStyleBackColor = true;
            this.butCheckAll.Click += new System.EventHandler(this.butCheckAll_Click);
            // 
            // butCheckNone
            // 
            resources.ApplyResources(this.butCheckNone, "butCheckNone");
            this.butCheckNone.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.butCheckNone.Name = "butCheckNone";
            this.butCheckNone.UseVisualStyleBackColor = true;
            this.butCheckNone.Click += new System.EventHandler(this.butCheckNone_Click);
            // 
            // butNext
            // 
            resources.ApplyResources(this.butNext, "butNext");
            this.butNext.Name = "butNext";
            this.butNext.UseVisualStyleBackColor = true;
            this.butNext.Click += new System.EventHandler(this.butNext_Click);
            // 
            // butCancel
            // 
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // chkSkipByDefault
            // 
            resources.ApplyResources(this.chkSkipByDefault, "chkSkipByDefault");
            this.chkSkipByDefault.Checked = true;
            this.chkSkipByDefault.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSkipByDefault.Name = "chkSkipByDefault";
            this.chkSkipByDefault.UseVisualStyleBackColor = true;
            this.chkSkipByDefault.CheckedChanged += new System.EventHandler(this.chkSkipByDefault_CheckedChanged);
            // 
            // frmBatch
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.chkSkipByDefault);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butNext);
            this.Controls.Add(this.butCheckNone);
            this.Controls.Add(this.butDelete);
            this.Controls.Add(this.butCheckAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstBooks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmBatch";
            this.Activated += new System.EventHandler(this.frmBatch_Activated);
            this.Load += new System.EventHandler(this.frmBatch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lstBooks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button butDelete;
        private System.Windows.Forms.Button butCheckAll;
        private System.Windows.Forms.Button butCheckNone;
        private System.Windows.Forms.Button butNext;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.CheckBox chkSkipByDefault;
    }
}