namespace MarkAble2
{
    partial class frmSelectTracks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectTracks));
            this.butCancel = new System.Windows.Forms.Button();
            this.butNext = new System.Windows.Forms.Button();
            this.butCheckNone = new System.Windows.Forms.Button();
            this.butCheckAll = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lstTracks = new System.Windows.Forms.CheckedListBox();
            this.chkStopShowing = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // butNext
            // 
            resources.ApplyResources(this.butNext, "butNext");
            this.butNext.Name = "butNext";
            this.butNext.UseVisualStyleBackColor = true;
            this.butNext.Click += new System.EventHandler(this.butNext_Click);
            // 
            // butCheckNone
            // 
            this.butCheckNone.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butCheckNone, "butCheckNone");
            this.butCheckNone.Name = "butCheckNone";
            this.butCheckNone.UseVisualStyleBackColor = true;
            this.butCheckNone.Click += new System.EventHandler(this.butCheckNone_Click);
            // 
            // butCheckAll
            // 
            this.butCheckAll.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butCheckAll, "butCheckAll");
            this.butCheckAll.Name = "butCheckAll";
            this.butCheckAll.UseVisualStyleBackColor = true;
            this.butCheckAll.Click += new System.EventHandler(this.butCheckAll_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lstTracks
            // 
            this.lstTracks.FormattingEnabled = true;
            resources.ApplyResources(this.lstTracks, "lstTracks");
            this.lstTracks.Name = "lstTracks";
            // 
            // chkStopShowing
            // 
            resources.ApplyResources(this.chkStopShowing, "chkStopShowing");
            this.chkStopShowing.Name = "chkStopShowing";
            this.chkStopShowing.UseVisualStyleBackColor = true;
            this.chkStopShowing.CheckedChanged += new System.EventHandler(this.chkStopShowing_CheckedChanged);
            // 
            // frmSelectTracks
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.chkStopShowing);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butNext);
            this.Controls.Add(this.butCheckNone);
            this.Controls.Add(this.butCheckAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstTracks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmSelectTracks";
            this.Load += new System.EventHandler(this.frmSelectTracks_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Button butNext;
        private System.Windows.Forms.Button butCheckNone;
        private System.Windows.Forms.Button butCheckAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox lstTracks;
        private System.Windows.Forms.CheckBox chkStopShowing;
    }
}