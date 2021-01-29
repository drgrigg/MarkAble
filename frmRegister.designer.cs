namespace MarkAble2
{
    partial class frmRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRegister));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRegistrationName = new System.Windows.Forms.TextBox();
            this.txtRegistrationKey = new System.Windows.Forms.TextBox();
            this.butRegister = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.labHeader = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // txtRegistrationName
            // 
            resources.ApplyResources(this.txtRegistrationName, "txtRegistrationName");
            this.txtRegistrationName.Name = "txtRegistrationName";
            // 
            // txtRegistrationKey
            // 
            resources.ApplyResources(this.txtRegistrationKey, "txtRegistrationKey");
            this.txtRegistrationKey.Name = "txtRegistrationKey";
            // 
            // butRegister
            // 
            resources.ApplyResources(this.butRegister, "butRegister");
            this.butRegister.Name = "butRegister";
            this.butRegister.UseVisualStyleBackColor = true;
            this.butRegister.Click += new System.EventHandler(this.butRegister_Click);
            // 
            // butCancel
            // 
            this.butCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.butCancel, "butCancel");
            this.butCancel.Name = "butCancel";
            this.butCancel.UseVisualStyleBackColor = true;
            // 
            // labHeader
            // 
            this.labHeader.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.labHeader, "labHeader");
            this.labHeader.ForeColor = System.Drawing.Color.Black;
            this.labHeader.Name = "labHeader";
            // 
            // frmRegister
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Wheat;
            this.Controls.Add(this.labHeader);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butRegister);
            this.Controls.Add(this.txtRegistrationKey);
            this.Controls.Add(this.txtRegistrationName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmRegister";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRegistrationName;
        private System.Windows.Forms.TextBox txtRegistrationKey;
        private System.Windows.Forms.Button butRegister;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label labHeader;
    }
}