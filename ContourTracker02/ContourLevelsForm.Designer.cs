namespace ContourTracker02
{
    partial class ContourLevelsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._contourListBox = new System.Windows.Forms.ListBox();
            this._zMaxTextBox = new System.Windows.Forms.TextBox();
            this._zMinTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._intervalTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._genButton = new System.Windows.Forms.Button();
            this._maxTextBox = new System.Windows.Forms.TextBox();
            this._minTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this._addButton = new System.Windows.Forms.Button();
            this._addTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this._deleteButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._contourListBox);
            this.groupBox1.Controls.Add(this._zMaxTextBox);
            this.groupBox1.Controls.Add(this._zMinTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(7, -2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(197, 314);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // _contourListBox
            // 
            this._contourListBox.FormattingEnabled = true;
            this._contourListBox.ItemHeight = 12;
            this._contourListBox.Location = new System.Drawing.Point(10, 90);
            this._contourListBox.Name = "_contourListBox";
            this._contourListBox.Size = new System.Drawing.Size(181, 220);
            this._contourListBox.TabIndex = 3;
            // 
            // _zMaxTextBox
            // 
            this._zMaxTextBox.Location = new System.Drawing.Point(65, 46);
            this._zMaxTextBox.Name = "_zMaxTextBox";
            this._zMaxTextBox.ReadOnly = true;
            this._zMaxTextBox.Size = new System.Drawing.Size(115, 21);
            this._zMaxTextBox.TabIndex = 2;
            this._zMaxTextBox.Text = "0";
            // 
            // _zMinTextBox
            // 
            this._zMinTextBox.Location = new System.Drawing.Point(65, 17);
            this._zMinTextBox.Name = "_zMinTextBox";
            this._zMinTextBox.ReadOnly = true;
            this._zMinTextBox.Size = new System.Drawing.Size(116, 21);
            this._zMinTextBox.TabIndex = 1;
            this._zMinTextBox.TabStop = false;
            this._zMinTextBox.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "ZMax";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "ZMin";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._intervalTextBox);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this._genButton);
            this.groupBox2.Controls.Add(this._maxTextBox);
            this.groupBox2.Controls.Add(this._minTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(210, -2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 163);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // _intervalTextBox
            // 
            this._intervalTextBox.Location = new System.Drawing.Point(75, 83);
            this._intervalTextBox.Name = "_intervalTextBox";
            this._intervalTextBox.Size = new System.Drawing.Size(96, 21);
            this._intervalTextBox.TabIndex = 4;
            this._intervalTextBox.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "间隔";
            // 
            // _genButton
            // 
            this._genButton.Location = new System.Drawing.Point(45, 121);
            this._genButton.Name = "_genButton";
            this._genButton.Size = new System.Drawing.Size(90, 27);
            this._genButton.TabIndex = 2;
            this._genButton.Text = "生成";
            this._genButton.UseVisualStyleBackColor = true;
            this._genButton.MouseDown += new System.Windows.Forms.MouseEventHandler(_genButton_MouseDown);
            // 
            // _maxTextBox
            // 
            this._maxTextBox.Location = new System.Drawing.Point(76, 49);
            this._maxTextBox.Name = "_maxTextBox";
            this._maxTextBox.Size = new System.Drawing.Size(96, 21);
            this._maxTextBox.TabIndex = 1;
            this._maxTextBox.Text = "0";
            // 
            // _minTextBox
            // 
            this._minTextBox.Location = new System.Drawing.Point(76, 17);
            this._minTextBox.Name = "_minTextBox";
            this._minTextBox.Size = new System.Drawing.Size(96, 21);
            this._minTextBox.TabIndex = 1;
            this._minTextBox.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "最大";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "最小";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this._addButton);
            this.groupBox3.Controls.Add(this._addTextBox);
            this.groupBox3.Location = new System.Drawing.Point(210, 167);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(191, 96);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "添加";
            // 
            // _addButton
            // 
            this._addButton.Location = new System.Drawing.Point(45, 59);
            this._addButton.Name = "_addButton";
            this._addButton.Size = new System.Drawing.Size(96, 26);
            this._addButton.TabIndex = 1;
            this._addButton.Text = "添加";
            this._addButton.UseVisualStyleBackColor = true;
            this._addButton.MouseDown += new System.Windows.Forms.MouseEventHandler(_addButton_MouseDown);
            // 
            // _addTextBox
            // 
            this._addTextBox.Location = new System.Drawing.Point(39, 22);
            this._addTextBox.Name = "_addTextBox";
            this._addTextBox.Size = new System.Drawing.Size(107, 21);
            this._addTextBox.TabIndex = 0;
            this._addTextBox.Text = "0";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this._deleteButton);
            this.groupBox4.Location = new System.Drawing.Point(210, 267);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(191, 66);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "删除";
            // 
            // _deleteButton
            // 
            this._deleteButton.Location = new System.Drawing.Point(45, 20);
            this._deleteButton.Name = "_deleteButton";
            this._deleteButton.Size = new System.Drawing.Size(96, 30);
            this._deleteButton.TabIndex = 0;
            this._deleteButton.Text = "删除";
            this._deleteButton.UseVisualStyleBackColor = true;
            this._deleteButton.MouseDown += new System.Windows.Forms.MouseEventHandler(_deleteButton_MouseDown);
            // 
            // _okButton
            // 
            this._okButton.Location = new System.Drawing.Point(13, 322);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(74, 25);
            this._okButton.TabIndex = 2;
            this._okButton.Text = "确定";
            this._okButton.UseVisualStyleBackColor = true;
            this._okButton.MouseDown += new System.Windows.Forms.MouseEventHandler(_okButton_MouseDown);
            // 
            // _cancelButton
            // 
            this._cancelButton.Location = new System.Drawing.Point(113, 322);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(74, 25);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "取消";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.MouseDown += new System.Windows.Forms.MouseEventHandler(_cancelButton_MouseDown);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // ContourLevelsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(413, 356);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContourLevelsForm";
            this.Text = "ContourLevels";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.ActiveControl = _cancelButton;
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button _genButton;
        private System.Windows.Forms.TextBox _maxTextBox;
        private System.Windows.Forms.TextBox _minTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button _addButton;
        private System.Windows.Forms.TextBox _addTextBox;
        private System.Windows.Forms.Button _deleteButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox _contourListBox;
        private System.Windows.Forms.TextBox _zMaxTextBox;
        private System.Windows.Forms.TextBox _zMinTextBox;
        private System.Windows.Forms.TextBox _intervalTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.IO.FileSystemWatcher fileSystemWatcher1;

    }
}