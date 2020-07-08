namespace viewImg
{
    partial class Form1
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.vidioBox = new System.Windows.Forms.PictureBox();
            this.labelKeepbord = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonMain = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.vidioBox)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(12, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "הפעל";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(12, 32);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "עצור";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // vidioBox
            // 
            this.vidioBox.Location = new System.Drawing.Point(13, 13);
            this.vidioBox.Name = "vidioBox";
            this.vidioBox.Size = new System.Drawing.Size(259, 176);
            this.vidioBox.TabIndex = 2;
            this.vidioBox.TabStop = false;
            // 
            // labelKeepbord
            // 
            this.labelKeepbord.AutoSize = true;
            this.labelKeepbord.Location = new System.Drawing.Point(55, 153);
            this.labelKeepbord.Name = "labelKeepbord";
            this.labelKeepbord.Size = new System.Drawing.Size(0, 13);
            this.labelKeepbord.TabIndex = 3;
            this.labelKeepbord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelKeepbord.Click += new System.EventHandler(this.labelKeepbord_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonMain
            // 
            this.buttonMain.Location = new System.Drawing.Point(108, 226);
            this.buttonMain.Name = "buttonMain";
            this.buttonMain.Size = new System.Drawing.Size(75, 23);
            this.buttonMain.TabIndex = 4;
            this.buttonMain.Text = "התחל";
            this.buttonMain.UseVisualStyleBackColor = true;
            this.buttonMain.Click += new System.EventHandler(this.Start);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(83, 169);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(124, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = "time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonMain);
            this.Controls.Add(this.labelKeepbord);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.vidioBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.vidioBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.PictureBox vidioBox;
        private System.Windows.Forms.Label labelKeepbord;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonMain;
        private System.Windows.Forms.TextBox textBox1;
    }
}

