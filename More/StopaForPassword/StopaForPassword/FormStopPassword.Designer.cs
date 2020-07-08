namespace StopaForPassword
{
    partial class FormStopPassword
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.BoxPassword = new System.Windows.Forms.TextBox();
            this.LabelPassword = new System.Windows.Forms.Label();
            this.panelPassword = new System.Windows.Forms.Panel();
            this.timerSetTab = new System.Windows.Forms.Timer(this.components);
            this.panelPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonOK.Location = new System.Drawing.Point(85, 122);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(109, 63);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "GO";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // BoxPassword
            // 
            this.BoxPassword.Location = new System.Drawing.Point(85, 43);
            this.BoxPassword.Name = "BoxPassword";
            this.BoxPassword.Size = new System.Drawing.Size(168, 20);
            this.BoxPassword.TabIndex = 1;
            // 
            // LabelPassword
            // 
            this.LabelPassword.AutoSize = true;
            this.LabelPassword.Location = new System.Drawing.Point(23, 46);
            this.LabelPassword.Name = "LabelPassword";
            this.LabelPassword.Size = new System.Drawing.Size(56, 13);
            this.LabelPassword.TabIndex = 2;
            this.LabelPassword.Text = "Password:";
            // 
            // panel1
            // 
            this.panelPassword.Controls.Add(this.BoxPassword);
            this.panelPassword.Controls.Add(this.LabelPassword);
            this.panelPassword.Controls.Add(this.buttonOK);
            this.panelPassword.Location = new System.Drawing.Point(12, 28);
            this.panelPassword.Name = "panel1";
            this.panelPassword.Size = new System.Drawing.Size(267, 198);
            this.panelPassword.TabIndex = 3;
            // 
            // timerSetTab
            // 
            this.timerSetTab.Tick += new System.EventHandler(this.timerSetTab_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.panelPassword);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stop For Password";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.panelPassword.ResumeLayout(false);
            this.panelPassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox BoxPassword;
        private System.Windows.Forms.Label LabelPassword;
        private System.Windows.Forms.Panel panelPassword;
        private System.Windows.Forms.Timer timerSetTab;
    }
}

