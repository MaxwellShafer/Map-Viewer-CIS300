namespace Ksu.Cis300.MapViewer
{
    partial class uxMapViewer
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
            this.uxMenuStrip = new System.Windows.Forms.MenuStrip();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uxZoomIn = new System.Windows.Forms.ToolStripMenuItem();
            this.uxZoomOut = new System.Windows.Forms.ToolStripMenuItem();
            this.uxFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.uxOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.uxMap = new Ksu.Cis300.MapViewer.Map();
            this.uxMenuStrip.SuspendLayout();
            this.uxFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxMenuStrip
            // 
            this.uxMenuStrip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.uxMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.uxMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.uxZoomIn,
            this.uxZoomOut});
            this.uxMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.uxMenuStrip.Name = "uxMenuStrip";
            this.uxMenuStrip.Size = new System.Drawing.Size(800, 36);
            this.uxMenuStrip.TabIndex = 1;
            this.uxMenuStrip.Text = "menuStrip1";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.AccessibleName = "uxOpenFile";
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(103, 30);
            this.openFileToolStripMenuItem.Text = "Open File";
            this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
            // 
            // uxZoomIn
            // 
            this.uxZoomIn.AccessibleName = "uxZoomIn";
            this.uxZoomIn.Enabled = false;
            this.uxZoomIn.Name = "uxZoomIn";
            this.uxZoomIn.Size = new System.Drawing.Size(96, 30);
            this.uxZoomIn.Text = "Zoom In";
            this.uxZoomIn.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // uxZoomOut
            // 
            this.uxZoomOut.AccessibleName = "uxZoomOut";
            this.uxZoomOut.Enabled = false;
            this.uxZoomOut.Name = "uxZoomOut";
            this.uxZoomOut.Size = new System.Drawing.Size(111, 30);
            this.uxZoomOut.Text = "Zoom Out";
            this.uxZoomOut.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // uxFlowLayoutPanel
            // 
            this.uxFlowLayoutPanel.AutoScroll = true;
            this.uxFlowLayoutPanel.Controls.Add(this.uxMap);
            this.uxFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxFlowLayoutPanel.Location = new System.Drawing.Point(0, 36);
            this.uxFlowLayoutPanel.Name = "uxFlowLayoutPanel";
            this.uxFlowLayoutPanel.Size = new System.Drawing.Size(800, 414);
            this.uxFlowLayoutPanel.TabIndex = 2;
            // 
            // uxOpenFileDialog
            // 
            this.uxOpenFileDialog.FileName = "uxOpenFileDialog";
            this.uxOpenFileDialog.Filter = "CSV files|*.csv|All files|*.*";
            // 
            // uxMap
            // 
            this.uxMap.BackColor = System.Drawing.Color.White;
            this.uxMap.BinaryTreeNode = null;
            this.uxMap.Enabled = false;
            this.uxMap.Location = new System.Drawing.Point(3, 3);
            this.uxMap.Name = "uxMap";
            this.uxMap.Size = new System.Drawing.Size(150, 150);
            this.uxMap.TabIndex = 0;
            this.uxMap.ZoomLevel = 1;
            this.uxMap.Load += new System.EventHandler(this.uxMap_Load);
            // 
            // uxMapViewer
            // 
            this.AccessibleName = "uxMapViewer";
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uxFlowLayoutPanel);
            this.Controls.Add(this.uxMenuStrip);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.uxMenuStrip;
            this.Name = "uxMapViewer";
            this.Text = "Map Viewer";
            this.Load += new System.EventHandler(this.uxMapViewer_Load);
            this.uxMenuStrip.ResumeLayout(false);
            this.uxMenuStrip.PerformLayout();
            this.uxFlowLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip uxMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uxZoomIn;
        private System.Windows.Forms.ToolStripMenuItem uxZoomOut;
        private System.Windows.Forms.FlowLayoutPanel uxFlowLayoutPanel;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Map uxMap;
        private System.Windows.Forms.OpenFileDialog uxOpenFileDialog;
    }
}

