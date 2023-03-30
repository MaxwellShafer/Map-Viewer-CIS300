using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksu.Cis300.MapViewer
{
    /// <summary>
    /// his structure represents an immutable line segment within the map. 
    /// It contains the information needed to draw the line segment.
    /// </summary>
    public struct LineSegment
    {
        /// <summary>
        ///  gets a PointF (i.e., a point consisting of two floating-point coordinates) giving the starting point of the segment;
        /// </summary>
        public PointF Start { get; }
        /// <summary>
        /// gets a PointF giving the ending point of the segment;
        /// </summary>
        public PointF End { get; }
        /// <summary>
        ///  gets an int giving the minimum zoom level at which this line segment will be visible.
        /// </summary>
        public int MinZoom { get; }
        /// <summary>
        ///  field to store the pen used to draw the line. This pen includes a color and a line width.
        /// </summary>
        private Pen _pen;

        /// <summary>
        /// The constructor should take as its parameters two PointFs, an int,  
        /// and a Pen. It should use them to initialize the three properties and the field. 
        /// Initialize the PointFs so that Start.X is less than or equal to End.X.
        /// </summary>
        /// <param name="start">A PointF</param>
        /// <param name="end">A PointF</param>
        /// <param name="minZoom">the minimum zoom level at which this line segment will be visible.</param>
        /// <param name="pen">pen used to draw the line. This pen includes a color and a line width.</param>
        public LineSegment( PointF start, PointF end, int minZoom, Pen pen)
        {
            if( start.X <= end.X)
            {
                Start = start;
                End = end;
            }
            else
            {
                End = start;
                Start = end;
            }

            MinZoom = minZoom;

            _pen = pen;

        }


        /// <summary>
        /// swapping the x-coordinate with the y-coordinate in both Start and End. For example, 
        /// if Start is (10, 20) and End is (15, 17), 
        /// the LineSegment returned should connect (20, 10) with (17, 15) 
        /// using the same zoom level and pen.
        /// </summary>
        /// <returns>a LineSegment describing the line segment obtained by swapping the x-coordinate with the y-coordinate</returns>
        public LineSegment Reflection()
        {
            PointF temp1 = new PointF(Start.Y, Start.X);
            PointF temp2 = new PointF(End.Y, End.X);

            return new LineSegment(temp1, temp2, MinZoom, _pen);
        }

        /// <summary>
        /// You may assume that the x-coordinates of Start and 
        /// End are different and that the given x-coordinate lies between them. 
        /// Use the values of the MinZoom property and the Pen field for the zoom level 
        /// and pen of both constructed line segments.
        /// </summary>
        /// <param name="xCord">x-coordinate at which to split the line segment.</param>
        /// <param name="leftSide">An out LineSegment through which to return the left portion of the split segment.</param>
        /// <param name="rightSide">An out LineSegment through which to return the right portion of the split segment.</param>
        public void SplitLine(float xCord, out LineSegment leftSide, out LineSegment rightSide)
        {
            float x1 = Start.X;
            float y1 = Start.Y;

            float x2 = End.X;
            float y2 = End.Y;

            float slope = ((y2 - y1) / (x2 - x1));

            float y3 = ((-slope) * (x1 - xCord) + y1);

            PointF midpoint = new PointF(xCord, y3);

            leftSide = new LineSegment(Start, midpoint, MinZoom, _pen);
            rightSide = new LineSegment(midpoint, End, MinZoom, _pen);
        }


        /// <summary>
        /// his method should take as its parameters a Graphics giving the graphics context on which to draw the line
        /// segment and a float giving the scale factor to use for drawing. It should return nothing. 
        /// The given Graphics has a DrawLine method that you should use to draw the line segment. 
        /// Scale each coordinate of each point by multiplying it by the scale factor.
        /// </summary>
        /// <param name="g">the graphics context on which to draw the line </param>
        /// <param name="scaleFactor">scale factor to use for drawing</param>
        public void DrawLineSegemnt(Graphics g, float scaleFactor)
        {
            PointF point1 = new PointF(Start.X * scaleFactor, Start.Y * scaleFactor);
            PointF point2 = new PointF(End.X * scaleFactor, End.Y * scaleFactor);

            g.DrawLine(_pen, point1, point2);
        }

    }
}
