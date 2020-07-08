namespace chatKey
{
    partial class Formkeys
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
            this.textBoxBody = new System.Windows.Forms.TextBox();
            this.comboBoxTvs = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxBody
            // 
            this.textBoxBody.Location = new System.Drawing.Point(12, 12);
            this.textBoxBody.Name = "textBoxBody";
            this.textBoxBody.Size = new System.Drawing.Size(388, 20);
            this.textBoxBody.TabIndex = 0;
            // 
            // comboBoxTvs
            // 
            this.comboBoxTvs.FormattingEnabled = true;
            this.comboBoxTvs.Location = new System.Drawing.Point(12, 38);
            this.comboBoxTvs.Name = "comboBoxTvs";
            this.comboBoxTvs.Size = new System.Drawing.Size(169, 21);
            this.comboBoxTvs.TabIndex = 1;
            this.comboBoxTvs.Leave += new System.EventHandler(this.comboBoxTvs_Leave);
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(157, 86);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(112, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // Formkeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 121);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.comboBoxTvs);
            this.Controls.Add(this.textBoxBody);
            this.Name = "Formkeys";
            this.Text = "keys";
            this.Load += new System.EventHandler(this.Formkeys_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxBody;
        private System.Windows.Forms.ComboBox comboBoxTvs;
        private System.Windows.Forms.Button buttonOk;
    }
}