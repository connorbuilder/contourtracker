namespace ContourTracker03
{
    partial class ContourTrackerForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this._openFileButton = new System.Windows.Forms.ToolStripButton();
            this._setContourLevelButon = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this._smoothButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openFileButton,
            this._setContourLevelButon,
            this._smoothButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(632, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // _openFileButton
            // 
            this._openFileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._openFileButton.Image = global::ContourTracker03.Properties.Resources.openHS;
            this._openFileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._openFileButton.Name = "_openFileButton";
            this._openFileButton.Size = new System.Drawing.Size(23, 22);
            this._openFileButton.Text = "toolStripButton1";
            // 
            // _setContourLevelButon
            // 
            this._setContourLevelButon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._setContourLevelButon.Image = global::ContourTracker03.Properties.Resources.EditInformationHS;
            this._setContourLevelButon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._setContourLevelButon.Name = "_setContourLevelButon";
            this._setContourLevelButon.Size = new System.Drawing.Size(23, 22);
            this._setContourLevelButon.Text = "toolStripButton2";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.ForeColor = System.Drawing.SystemColors.Control;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(632, 420);
            this.panel1.TabIndex = 1;
            // 
            // _smoothButton
            // 
            this._smoothButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this._smoothButton.Image = global::ContourTracker03.Properties.Resources.Run;
            this._smoothButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._smoothButton.Name = "_smoothButton";
            this._smoothButton.Size = new System.Drawing.Size(23, 22);
            this._smoothButton.Text = "toolStripButton1";
            // 
            // ContourTrackerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(632, 445);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ContourTrackerForm";
            this.Text = "ContourTrackerFrom";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton _openFileButton;
        private System.Windows.Forms.ToolStripButton _setContourLevelButon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton _smoothButton;
    }
}

