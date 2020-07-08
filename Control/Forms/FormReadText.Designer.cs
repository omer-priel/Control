namespace Control
{
    partial class FormReadText
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
            this.BoxLine = new System.Windows.Forms.TextBox();
            this.BoxBord = new System.Windows.Forms.RichTextBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.LabelTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BoxLine
            // 
            this.BoxLine.Location = new System.Drawing.Point(59, 46);
            this.BoxLine.Name = "BoxLine";
            this.BoxLine.Size = new System.Drawing.Size(314, 20);
            this.BoxLine.TabIndex = 0;
            // 
            // BoxBord
            // 
            this.BoxBord.Location = new System.Drawing.Point(59, 46);
            this.BoxBord.Name = "BoxBord";
            this.BoxBord.Size = new System.Drawing.Size(314, 103);
            this.BoxBord.TabIndex = 1;
            this.BoxBord.Text = "";
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(170, 21);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // LabelTitle
            // 
            this.LabelTitle.AutoSize = true;
            this.LabelTitle.Location = new System.Drawing.Point(187, 19);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.Size = new System.Drawing.Size(27, 13);
            this.LabelTitle.TabIndex = 3;
            this.LabelTitle.Text = "Title";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 58);
            this.panel1.TabIndex = 4;
            // 
            // FormReadText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 213);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.LabelTitle);
            this.Controls.Add(this.BoxLine);
            this.Controls.Add(this.BoxBord);
            this.Name = "FormReadText";
            this.Text = "FormReadText";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BoxLine;
        private System.Windows.Forms.RichTextBox BoxBord;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label LabelTitle;
        private System.Windows.Forms.Panel panel1;
    }
}