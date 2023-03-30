/* uxMapViewer.cs
 * Made By: Max Shafer
 */
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
    /// <summary>
    /// A class for the Map Veiwer
    /// </summary>
    public partial class uxMapViewer : Form
    {
        /// <summary>
        /// A int to keep track of max zoom
        /// </summary>
        private int _maxZoom;

        /// <summary>
        /// constrictor for the map view
        /// </summary>
        public uxMapViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// IGNORE*********
        /// </summary>
        /// <param name="sender">ignore</param>
        /// <param name="e">ignore</param>
        private void uxMapViewer_Load(object sender, EventArgs e)
        {
            //ignore!
        }

        /// <summary>
        /// An even handler to open file
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
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

        /// <summary>
        /// ************IGNORE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxMap_Load(object sender, EventArgs e)
        {
            ///   IGNORE
        }

        /// <summary>
        /// Zoom in event hadler
        /// </summary>
        /// <param name="sender">sendeer</param>
        /// <param name="e"> the E</param>
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

        /// <summary>
        /// A event handler for the zoom out 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e"> The E</param>
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
