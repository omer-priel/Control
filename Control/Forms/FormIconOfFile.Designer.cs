namespace Control
{
    partial class FormIconOfFile
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
            this.BoxListNames = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.BoxIcon = new System.Windows.Forms.PictureBox();
            this.MenuNames = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuNames_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNames_Delet = new System.Windows.Forms.ToolStripMenuItem();
            this.labelName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.BoxIcon)).BeginInit();
            this.MenuNames.SuspendLayout();
            this.SuspendLayout();
            // 
            // BoxListNames
            // 
            this.BoxListNames.ContextMenuStrip = this.MenuNames;
            this.BoxListNames.FormattingEnabled = true;
            this.BoxListNames.Location = new System.Drawing.Point(12, 32);
            this.BoxListNames.Name = "BoxListNames";
            this.BoxListNames.Size = new System.Drawing.Size(75, 173);
            this.BoxListNames.TabIndex = 0;
            this.BoxListNames.SelectedIndexChanged += new System.EventHandler(this.BoxListNames_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = " Icons List";
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Location = new System.Drawing.Point(122, 32);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(63, 23);
            this.buttonOpenFile.TabIndex = 2;
            this.buttonOpenFile.Text = "Open File";
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // BoxIcon
            // 
            this.BoxIcon.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BoxIcon.Location = new System.Drawing.Point(110, 95);
            this.BoxIcon.Name = "BoxIcon";
            this.BoxIcon.Size = new System.Drawing.Size(90, 100);
            this.BoxIcon.TabIndex = 3;
            this.BoxIcon.TabStop = false;
            // 
            // MenuNames
            // 
            this.MenuNames.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuNames_Add,
            this.MenuNames_Delet});
            this.MenuNames.Name = "MenuNames";
            this.MenuNames.Size = new System.Drawing.Size(101, 48);
            // 
            // MenuNames_Add
            // 
            this.MenuNames_Add.Name = "MenuNames_Add";
            this.MenuNames_Add.Size = new System.Drawing.Size(100, 22);
            this.MenuNames_Add.Text = "הוסף";
            this.MenuNames_Add.Click += new System.EventHandler(this.Tool_Add);
            // 
            // MenuNames_Delet
            // 
            this.MenuNames_Delet.Name = "MenuNames_Delet";
            this.MenuNames_Delet.Size = new System.Drawing.Size(100, 22);
            this.MenuNames_Delet.Text = "מחק";
            this.MenuNames_Delet.Click += new System.EventHandler(this.Tool_Delet);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(140, 68);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(23, 13);
            this.labelName.TabIndex = 4;
            this.labelName.Text = "File";
            // 
            // FormIconOfFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 216);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.BoxIcon);
            this.Controls.Add(this.buttonOpenFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BoxListNames);
            this.Name = "FormIconOfFile";
            this.Text = "FormIconOfFile";
            this.Load += new System.EventHandler(this.FormIconOfFile_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BoxIcon)).EndInit();
            this.MenuNames.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox BoxListNames;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.PictureBox BoxIcon;
        private System.Windows.Forms.ContextMenuStrip MenuNames;
        private System.Windows.Forms.ToolStripMenuItem MenuNames_Add;
        private System.Windows.Forms.ToolStripMenuItem MenuNames_Delet;
        private System.Windows.Forms.Label labelName;
    }
}