using Ksu.Cis300.ImmutableBinaryTrees;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.MapViewer
{
    public partial class uxMapViewer : Form
    {
        private int _maxZoom;
        public uxMapViewer()
        {
            InitializeComponent();
        }

        private void uxMapViewer_Load(object sender, EventArgs e)
        {

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (uxOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //read this file into a BinaryTreeNode<MapData> using the appropriate method of the QuadTree class
                    int zoom;

                    BinaryTreeNode<MapData> binaryTree = QuadTree.ReadFile(uxOpenFileDialog.FileName, out zoom);

                    _maxZoom = zoom;

                    uxMap.BinaryTreeNode = binaryTree;

                    uxFlowLayoutPanel.AutoScrollPosition = new Point(0, 0);


                    //If the user selects a file, the program should attempt to read the file and display the map it describes,
                    //but only showing those streets whose zoom level (i.e., the last field of its input line) is 1.


                    if (_maxZoom >= 2)
                    {
                        uxZoomIn.Enabled = true;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            

        }

        private void uxMap_Load(object sender, EventArgs e)
        {

        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
           Point point = uxFlowLayoutPanel.AutoScrollPosition;

            int x = Math.Abs(point.X);
            int y = Math.Abs(point.Y);

            

            uxMap.ZoomLevel++; //CHECK BACK

            Size clientSize = uxFlowLayoutPanel.ClientSize;

            float centerX = ((2 * x) + ((float).5 * clientSize.Width));
            float centerY = ((2 * y) + ((float).5 * clientSize.Height));

            //f we want this in the center, we need to subtract
            //half the client size to get the new auto-scroll position. 


            uxFlowLayoutPanel.AutoScrollPosition = new Point((int)centerX, (int)centerY);

            if(uxMap.ZoomLevel >= _maxZoom)
            {
                uxZoomIn.Enabled = false;
            }

            if(uxMap.ZoomLevel > 1)
            {
                uxZoomOut.Enabled = true;
            }
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point point = uxFlowLayoutPanel.AutoScrollPosition;

            int x = Math.Abs(point.X);
            int y = Math.Abs(point.Y);

            uxMap.ZoomLevel--; //CHECK BACK

            Size clientSize = uxFlowLayoutPanel.ClientSize;

            float centerX = (((float).5 * x) - ((float).5 * clientSize.Width));
            float centerY = (((float).5 * y) - ((float).5 * clientSize.Height));

            //f we want this in the center, we need to subtract
            //half the client size to get the new auto-scroll position. 



            uxFlowLayoutPanel.AutoScrollPosition = new Point((int)centerX, (int)centerY);

            if (uxMap.ZoomLevel < _maxZoom)
            {
                uxZoomIn.Enabled = true;
            }

            if (uxMap.ZoomLevel <= 1)
            {
                uxZoomOut.Enabled = false;
            }

        }
    }
}
