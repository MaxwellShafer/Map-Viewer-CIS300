using KansasStateUniversity.TreeViewer2;
using Ksu.Cis300.ImmutableBinaryTrees;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksu.Cis300.MapViewer
{
    /// <summary>
    /// adds some functionality to the Map control you created (see Section 3.1. GUI Design). 
    /// </summary>
    public partial class Map : UserControl
    {
        /// <summary>
        /// A const float giving the scale factor at zoom level 0 (5). Zoom level 0 won't actually be used,
        /// but using this constant makes the computation of the scale factor slightly simpler.
        /// </summary>
        private const float _scaleFactorAtZero = 5;

        /// <summary>
        ///  const int giving the width and height of the Map when the quad tree is null.
        /// </summary>
        private const int _widthAndHeightWhenNull = 150; 

        /// <summary>
        /// A float storing the current scale factor.
        /// </summary>
        private float _currentScaleFactor;

        /// <summary>
        /// An int storing the current zoom level.
        /// </summary>
        private static int _currentZoomLevel;

        /// <summary>
        /// A BinaryTreeNode<MapData> storing the quad tree containing the map data
        /// </summary>

        private BinaryTreeNode<MapData> _binaryTree;

        public int ZoomLevel
        {
            get { return _currentZoomLevel; }
            set
            {
                //Compute the new scale factor:
                _currentZoomLevel = value;
                float scaleFactor = (float)(_scaleFactorAtZero * Math.Pow(2, _currentZoomLevel));
                _currentScaleFactor = scaleFactor;

                if (_binaryTree == null)
                {
                    // If the tree is null, these values should both
                    // be set to the default stored in the appropriate constant above. 
                    Width = _widthAndHeightWhenNull;
                    Height = _widthAndHeightWhenNull;
                }
                else
                {
                    //Otherwise, multiply the width and height, respectively,
                    //of the bounds stored in the root of the tree by the current
                    //scale factor, and convert the results to ints.
                    int newWidth = (int)(_binaryTree.Data.Bounds.Width * scaleFactor) + 1;
                    int newHeight = (int)(_binaryTree.Data.Bounds.Height * scaleFactor) + 1;
                    Width = newWidth;
                    Height = newHeight;
                }

                //Call Invalidate, which is a method of the Map inherited from Control.
                //Calling this method signals that this control needs to be redrawn. 
                Invalidate();
            }
        }

        /// <summary>
        /// his property should be of type BinaryTreeNode<MapData>. 
        /// Its get accessor should return the BinaryTreeNode<MapData> field. 
        /// The set accessor will need to set this field to the given value, 
        /// but will also need to set the zoom level to 1 using the above property.
        /// </summary>
        public BinaryTreeNode<MapData> BinaryTreeNode
        {
            get { return _binaryTree; }

            set
            {
                _binaryTree = value;
                ZoomLevel = 1;
            }
        }

        public Map()
        {
            InitializeComponent();

            //Following the call to InitializeComponent,
            //set the zoom level to 1 using the appropriate property above.
            ZoomLevel = 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RectangleF recF = e.ClipRectangle;
            Graphics g = e.Graphics;

            if(g != null)
            {
                Region region = new Region(recF);

                g.Clip = region;

                QuadTree.DrawQuadTree(BinaryTreeNode, g, ZoomLevel, _currentScaleFactor);
            }

        }

        private void Map_Load(object sender, EventArgs e)
        {

        }
    }
}
