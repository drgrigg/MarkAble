namespace MarkAble2
{
    partial class frmSeparate3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSeparate3));
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.butNext = new System.Windows.Forms.Button();
            this.dlgSelectFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgSelectFiles = new System.Windows.Forms.OpenFileDialog();
            this.butCancel = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radYes = new System.Windows.Forms.RadioButton();
            this.radNo = new System.Windows.Forms.RadioButton();
            this.butSwap = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txtAuthor
            // 
            this.txtAuthor.AcceptsReturn = true;
            resources.ApplyResources(this.txtAuthor, "txtAuthor");
            this.txtAuthor.Name = "txtAuthor";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtTitle
            // 
            this.txtTitle.AcceptsReturn = true;
            resources.ApplyResources(this.txtTitle, "txtTitle");
            this.txtTitle.Name = "txtTitle";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // radYes
            // 
            resources.ApplyResources(this.radYes, "radYes");
            this.radYes.Checked = true;
            this.radYes.Name = "radYes";
            this.radYes.TabStop = true;
            this.radYes.UseVisualStyleBackColor = true;
            // 
            // radNo
            // 
            resources.ApplyResources(this.radNo, "radNo");
            this.radNo.Name = "radNo";
            this.radNo.UseVisualStyleBackColor = true;
            // 
            // butSwap
            // 
            this.butSwap.BackColor = System.Drawing.SystemColors.ButtonFace;
            resources.ApplyResources(this.butSwap, "butSwap");
            this.butSwap.Name = "butSwap";
            this.butSwap.UseVisualStyleBackColor = true;
            this.butSwap.Click += new System.EventHandler(this.butSwap_Click);
            // 
            // frmSeparate3
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.butSwap);
            this.Controls.Add(this.radNo);
            this.Controls.Add(this.radYes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtAuthor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSeparate3";
            this.Load += new System.EventHandler(this.frmSeparate3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butNext;
        private System.Windows.Forms.FolderBrowserDialog dlgSelectFolder;
        private System.Windows.Forms.OpenFileDialog dlgSelectFiles;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radYes;
        private System.Windows.Forms.RadioButton radNo;
        private System.Windows.Forms.Button butSwap;
    }
}