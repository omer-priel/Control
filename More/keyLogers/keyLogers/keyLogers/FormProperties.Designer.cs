namespace keyLogers
{
    partial class FormProperties
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
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxTimeTab = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxSendMail = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(86, 32);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(117, 20);
            this.textBoxPassword.TabIndex = 0;
            this.textBoxPassword.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxPassword_KeyUp);
            // 
            // textBoxTimeTab
            // 
            this.textBoxTimeTab.Location = new System.Drawing.Point(86, 58);
            this.textBoxTimeTab.Name = "textBoxTimeTab";
            this.textBoxTimeTab.Size = new System.Drawing.Size(117, 20);
            this.textBoxTimeTab.TabIndex = 1;
            this.textBoxTimeTab.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "time tab (s):";
            // 
            // checkBoxSendMail
            // 
            this.checkBoxSendMail.AutoSize = true;
            this.checkBoxSendMail.Checked = true;
            this.checkBoxSendMail.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSendMail.Location = new System.Drawing.Point(13, 92);
            this.checkBoxSendMail.Name = "checkBoxSendMail";
            this.checkBoxSendMail.Size = new System.Drawing.Size(70, 17);
            this.checkBoxSendMail.TabIndex = 4;
            this.checkBoxSendMail.Text = "send mail";
            this.checkBoxSendMail.UseVisualStyleBackColor = true;
            // 
            // FormProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.checkBoxSendMail);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxTimeTab);
            this.Controls.Add(this.textBoxPassword);
            this.Name = "FormProperties";
            this.Text = "Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBoxPassword;
        public System.Windows.Forms.TextBox textBoxTimeTab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox checkBoxSendMail;
    }
}