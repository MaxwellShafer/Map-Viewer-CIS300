using KansasStateUniversity.TreeViewer2;
using Ksu.Cis300.ImmutableBinaryTrees;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Ksu.Cis300.MapViewer
{
    public static class QuadTree
    {
        /// <summary>
        /// giving the threshold for increasing the zoom level (100). 
        /// Thus, if the bounding rectangle of a splitting node has an area less than this value, 
        /// its children will be at the next higher zoom level.
        /// </summary>
        private const int _ZoomIncrease = 100;

        /// <summary>
        /// the field index of the width of the map bounds (0). 
        /// This indicates that the width of the map is the first value on its input line.
        /// </summary>
        private const int _mapBoundsWidthIndex = 0;

        /// <summary>
        /// giving the field index of the length of the map bounds (1). 
        /// </summary>
        private const int _mapBoundsLenghtIndex = 1;

        /// <summary>
        /// the field index of the x-coordinate of the first point listed on an input line after the first line (0).
        /// </summary>
        private const int _xCordFirstPointIndex = 0;

        /// <summary>
        /// the field index of the y-coordinate of the first point listed on an input line after the first line (1).
        /// </summary>
        private const int _yCordFirstPointIndex = 1;

        /// <summary>
        /// the field index of the x-coordinate of the second point listed on an input line after the first line (2).
        /// </summary>
        private const int _xCordSecondPointIndex = 2;

        /// <summary>
        /// the field index of the y-coordinate of the second point listed on an input line after the first line (3).
        /// </summary>
        private const int _yCordSecondPointIndex = 3;

        /// <summary>
        /// the field index of the color on an input line after the first line (4).
        /// </summary>
        private const int _colorIndex = 4;

        /// <summary>
        /// the field index of the line segment width on an input line after the first line (5).
        /// </summary>
        private const int _lineWidthIndex = 5;

        /// <summary>
        /// the zoom level on an input line after the first line (6).
        /// </summary>
        private const int _zoomLevelOnLine = 6;

        /// <summary>
        ///  to contain the Pens used for drawing line segments. 
        ///  The int in a key represents the ARGB code of a color, and the float is a line width. 
        ///  The value associated with a key is a Pen drawing this color with this line width.
        /// </summary>
        private static Dictionary<(int, float), Pen> _pens = new Dictionary<(int, float), Pen>();


        /// <summary>
        /// It should return a BinaryTreeNode<MapData> giving the root of a 
        /// tree containing the information given in the parameters. 
        /// If the given List<LineSegment> is empty, 
        /// it should return null, and the out parameter should be set to 0.
        /// This method must be recursive.The base case is that the given 
        /// List<LineSegment> is empty.
        /// Otherwise, you will need to initialize a few local variables, 
        /// some of whose initial values will depend on whether the root is 
        /// to be a quad tree node or a splitting node
        /// </summary>
        /// <param name="list">A List<LineSegment> containing the line segments to be included in the tree if the root is to be a quad tree node, or the reflections of these line segments</param>
        /// <param name="recF">A RectangleF giving the map bounds for the root node;</param>
        /// <param name="zoom">An int giving the zoom level for the root node;</param>
        /// <param name="isQuadTree">A bool indicating whether the root is a quad tree node;</param>
        /// <param name="maxZoom">An out int giving the maximum zoom level in the tree.</param>
        /// <returns></returns>
        private static BinaryTreeNode<MapData> BuildTree(List<LineSegment> list, RectangleF recF, int zoom, bool isQuadTree, out int maxZoom)
        {
           

            if (list == null)
            {
                maxZoom = 0;
                return null;
            }
            else if (list.Count != 0)
            {

                //A MapData containing the bounds and zoom level for the root
                MapData mapData = new MapData(recF, zoom);
                float whereToSplit = default; // look at later
                RectangleF bottomRightBounds = default;
                RectangleF topLeftBounds = default;
                int zoomOfChildren;

                List<LineSegment> leftList = new List<LineSegment>();
                List<LineSegment> rightList = new List<LineSegment>();

                if (isQuadTree || ((recF.Width * recF.Height) < _ZoomIncrease))
                {
                    zoomOfChildren = zoom + 1;
                }
                else
                {
                    zoomOfChildren = zoom;
                }


                if (isQuadTree) // is quadtree node
                {
                    

                    //A float giving the value at which the rectangular bounds will be split to form the two children.
                    //For a quad tree node, this will be the x-coordinate of the left edge of the root node's bounds plus half the width of these bounds.
                    whereToSplit = recF.Left + ((float).5 * recF.Width);

                    //Two RectangleFs giving the bounds for the left child and the right child, respectively. 
                    //For a quad tree node, these rectangles should be formed by splitting the bounds of the root
                    //node at the vertical line whose x-coordinate is the above value.

                    topLeftBounds = new RectangleF(recF.Left, recF.Top, recF.Width * (float).5, recF.Height);
                    bottomRightBounds = new RectangleF(whereToSplit, recF.Top, recF.Width * (float).5, recF.Height);

                    //***** NOT IN DOC ******
                    isQuadTree = false;

                }
                else // is splitting node
                {

                    
                    //A float giving the value at which the rectangular bounds will be split to form the two children.
                    // For a splitting node, this will be the y-coordinate of the top edge of the root node's bounds plus half the height of these bounds.
                    whereToSplit = recF.Top + ((float).5 * recF.Height);

                    //Two RectangleFs giving the bounds for the left child and the right child, respectively. 
                    //For a splitting node, these rectangles should be formed by splitting the bounds of the
                    //root node at the horizontal line whose y-coordinate is the above value.
                    //

                    topLeftBounds = new RectangleF(recF.Left, recF.Top, recF.Width, recF.Height * (float).5);
                    bottomRightBounds = new RectangleF(recF.Left, whereToSplit, recF.Width, recF.Height * (float).5);
                    //***** NOT IN DOC ******
                    isQuadTree = true;
                }

                AddToLists(list, mapData, zoom, whereToSplit, out leftList, out rightList);

                int zoom1;
                int zoom2;
                //recursively build the two children.
                BinaryTreeNode<MapData> left = BuildTree(leftList, topLeftBounds, zoomOfChildren, isQuadTree, out zoom1);

                BinaryTreeNode<MapData> right = BuildTree(rightList, bottomRightBounds, zoomOfChildren, isQuadTree, out zoom2);

                //Set the out parameter to the maximum of the zoom level of the root and the maximum zoom levels of the two children.
                maxZoom = Math.Max(Math.Max(zoom1, zoom2), zoom);
                

                //Finally, construct the root and return it.
                BinaryTreeNode<MapData> root = new BinaryTreeNode<MapData>(mapData, left, right);

                return root;


            }
            else
            {
                maxZoom = 0;
                return null;
            }

        }

        /// <summary>
        /// adds lines to respective lists
        /// </summary>
        /// <param name="list">the list reading from</param>
        /// <param name="mapData">the mapdata</param>
        /// <param name="zoom">what level of zoom</param>
        /// <param name="whereToSplit">where to split the lines</param>
        /// <param name="leftList">the left list</param>
        /// <param name="rightList">the rightlist</param>
        private static void AddToLists(List<LineSegment> list, MapData mapData, int zoom,float whereToSplit, out List<LineSegment> leftList, out List<LineSegment> rightList)
        {

            leftList = new List<LineSegment>();
            rightList = new List<LineSegment>();

            foreach (LineSegment lineSegment in list)
            {

                //If the minimum zoom level for the LineSegment is less than or equal to the zoom level of the root,
                if (lineSegment.MinZoom <= zoom)
                {
                    //add this LineSegment to the list of line segments in the MapData.
                    mapData.Lines.Add(lineSegment);
                }
                //Otherwise,if the x-coordinate of the ending point of the LineSegment is
                //no more than the value at which the rectangular bounds are split
                else if (lineSegment.End.X < whereToSplit)
                {
                    //add the reflection of this LineSegment to the list for the left child
                    //mapData.Lines.Add(lineSegment);
                    leftList.Add(lineSegment);
                }
                //Otherwise, if the x-coordinate of the starting point of the LineSegment
                //is at least the value at which the rectangular bounds are split,
                else if (lineSegment.Start.X >= whereToSplit)
                {
                    //add the reflection of this LineSegment to the list for the right child
                    //mapData.Lines.Add(lineSegment);
                    rightList.Add(lineSegment);
                }
                else
                {

                    LineSegment leftSide;
                    LineSegment rightSide;

                    //split this LineSegment at the value at which the rectangular bounds are split
                    lineSegment.SplitLine(whereToSplit, out leftSide, out rightSide);

                    //and add the reflections of the resulting LineSegments to the lists for the appropriate children.
                    leftList.Add(leftSide);
                    rightList.Add(rightSide);
                }

            }
        }


        /// <summary>
        /// his method needs as its parameters a string giving the name of the file
        /// to be read and an out int giving the maximum zoom level in the file read. 
        /// It should return a BinaryTreeNode<MapData> giving a quad tree containing the 
        /// map data read from the file. It will need to do all of the error checking 
        /// described under "Exception Handling" above, 
        /// throwing the appropriate exception when an error is detected.
        /// </summary>
        /// <param name="fileName">the name of the file to be read</param>
        /// <param name="maxZoom">an out int the max zoom</param>
        /// <returns></returns>
        /// <exception cref="IOException"></exception>
        public static BinaryTreeNode<MapData> ReadFile(string fileName, out int maxZoom)
        {
            using (StreamReader file = new StreamReader(fileName))
            {
                string firstLine = file.ReadLine();
                string[] firstSplit = firstLine.Split(',');
                double bound1 = Convert.ToDouble(firstSplit[_mapBoundsWidthIndex]);
                double bound2 = Convert.ToDouble(firstSplit[_mapBoundsLenghtIndex]);
                maxZoom = 0;

                List<LineSegment> lineList = new List<LineSegment>();

                
                //If the first line contains a value less than or equal to 0,
                //throw an IOException containing the message, "Line 1 contains a non-positive value."
                if (bound1 <= 0 || bound2 <= 0)
                {
                    throw new IOException("Line 1 contains a non-positive value.");
                }

                RectangleF bounds = new RectangleF(0, 0, (float)bound1, (float)bound2);

                //if any line after the first contains a description of a point that is outside the bounds given in line 1,
                //throw an IOException containing the message, "Line n describes a street that is not within the map bounds.",
                //where n gives the number of the line containing the error (the first line in the file is line 1).

                int fileLine = 1; // what line of the file we are on

                while (!file.EndOfStream)
                {
                    string[] line = file.ReadLine().Split(',');
                    fileLine++;

                    float firstXCord = (float)Convert.ToDouble(line[_xCordFirstPointIndex]);
                   
                   
                    
                    if( firstXCord < bounds.Left || firstXCord > bounds.Right)
                    {
                        MessageBox.Show(firstXCord.ToString() + " Bounds: " + bounds.Left + " " + bounds.Right);
                        throw new IOException("Line " + fileLine + " describes a street that is not within the map bounds.");
                    }

                    float firstYCord = (float)Convert.ToDouble(line[_yCordFirstPointIndex]);

                    // might need to be flipped
                    if (firstYCord < bounds.Top || firstYCord > bounds.Bottom)
                    {
                        throw new IOException("Line " + fileLine + " describes a street that is not within the map bounds.");
                    }

                    float secondXCord = (float)Convert.ToDouble(line[_xCordSecondPointIndex]);

                    if (secondXCord < bounds.Left || secondXCord > bounds.Right)
                    {
                        MessageBox.Show(secondXCord.ToString() + " Bounds left: " + bounds.Left + "Bounds Right:  " + bounds.Right);
                        throw new IOException("Line " + fileLine + " describes a street that is not within the map bounds.");

                    }

                    float secondYCord = (float)Convert.ToDouble(line[_yCordSecondPointIndex]);

                    if (secondYCord < bounds.Top || secondYCord > bounds.Bottom)
                    {
                        throw new IOException("Line " + fileLine + " describes a street that is not within the map bounds.");

                    }

                    int ARGB = Convert.ToInt32(line[_colorIndex]);


                    //If any line after the first contains a line width that is less than or equal to 0,
                    //throw an IOException containing the message,
                    //"Line n contains a non-positive line width.", where n is as above.
                    double lineWidth = Convert.ToDouble(line[_lineWidthIndex]);

                    if(lineWidth <= 0)
                    {
                        throw new IOException("Line " + fileLine + " contains a non-positive line width.");
                    }

                    //If any line after the first contains a zoom level that is less than 1,
                    //throw an IOException containing the message,
                    //"Line n contains a non-positive zoom level.", where n is as above.
                    int zoomOfLine = Convert.ToInt16(line[_zoomLevelOnLine]);
                    if(zoomOfLine < 1)
                    {
                        throw new IOException("Line " + fileLine + "contains a non-positive zoom level.");
                    }

                    if(zoomOfLine > maxZoom)
                    {
                        maxZoom = zoomOfLine;
                    }

                    Pen pen = getPen(ARGB, lineWidth);

                    PointF start = new PointF(firstXCord, firstYCord);
                    PointF end = new PointF(secondXCord, secondYCord);

                    lineList.Add(new LineSegment(start, end, zoomOfLine, pen));


                }
                                                                             //check v zoom of root
                BinaryTreeNode<MapData> binaryTreeNode = BuildTree(lineList, bounds, 0, true, out maxZoom);

                return binaryTreeNode;
            }


        }

        /// <summary>
        /// A method to get a pen, checks if in dictionary, if not adds it
        /// </summary>
        /// <param name="ARGB">the color</param>
        /// <param name="lineWidth">the witdth</param>
        /// <returns>the pen containing color and width</returns>
        private static Pen getPen(int ARGB, double lineWidth)
        {
            
            if(_pens.TryGetValue( (ARGB,(float)lineWidth), out Pen pen))
            {
                return pen;
            }
            else
            {
                
                pen = new Pen(Color.FromArgb(ARGB), (float) lineWidth);
                _pens.Add((ARGB, (float)lineWidth), pen);
                return pen;
            }
        }

        /// <summary>
        /// You will first need to make sure the given tree isn't null - if it is, there is nothing to draw. 
        /// Then convert the bounding rectangle at the root of the tree to pixel coordinates by multiplying its 
        /// four components by the given scale factor. The given Graphics has a ClipBounds property that gets a 
        /// RectangleF describing the region of the Map control that needs to be redrawn. You will first need to 
        /// see if the ClipBounds intersects the scaled rectangle by using one of these RectangleFs' IntersectsWith methods.
        /// You should only proceed if they intersect and the zoom level in the tree is no more than the zoom level parameter
        /// - otherwise, there is nothing to draw.
        ///If all of the above conditions are met, iterate through the Lines property of the root node, drawing each LineSegment 
        ///onto the given Graphics using the appropriate method of the LineSegment.
        ///Once all of these lines are drawn, recursively draw the children.
        /// </summary>
        /// <param name="binaryTreeNode">giving the quad tree.</param>
        /// <param name="g">giving the graphics context on which to draw.</param>
        /// <param name="zoomDisplayed">an int giving the zoom level currently being displayed.</param>
        /// <param name="scaleFactor">A float giving the current scale factor by which the map data must be multiplied to obtain pixel coordinates.</param>
        public static void DrawQuadTree(BinaryTreeNode<MapData> binaryTreeNode, Graphics g, int zoomDisplayed,float scaleFactor)
        {
            //You will first need to make sure the given tree isn't null - if it is, there is nothing to draw.
            if (binaryTreeNode != null)
            {
                
                RectangleF bounds = binaryTreeNode.Data.Bounds;

                //Then convert the bounding rectangle at the root of the tree to pixel coordinates
                //by multiplying its four components by the given scale factor. 
                RectangleF newBounds = new RectangleF(bounds.X * scaleFactor, bounds.Y * scaleFactor, bounds.Width * scaleFactor, bounds.Height * scaleFactor);


                //You will first need to see if the ClipBounds intersects the scaled rectangle by using
                //one of these RectangleFs' IntersectsWith methods. You should only proceed if they intersect
                //and the zoom level in the tree is no more than the zoom level parameter - otherwise, there is nothing to draw.
                if (binaryTreeNode.Data.Zoom <= zoomDisplayed)
                {
                    foreach(LineSegment line in binaryTreeNode.Data.Lines)
                    {
                        line.DrawLineSegemnt(g, scaleFactor);
                    }
                }

                //Once all of these lines are drawn, recursively draw the children.
                DrawQuadTree(binaryTreeNode.LeftChild, g, zoomDisplayed, scaleFactor);
                DrawQuadTree(binaryTreeNode.RightChild, g, zoomDisplayed, scaleFactor);


            }
        }


    }
}
