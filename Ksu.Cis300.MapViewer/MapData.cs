using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ksu.Cis300.MapViewer
{
    /// <summary>
    /// Each instance of this structure contains the data to be stored in one tree node; 
    /// thus, each MapData describes a portion of the map at a particular zoom level. 
    /// This structure is immutable. 
    /// </summary>
    public struct MapData
    {
        /// <summary>
        /// gets a RectangleF giving the bounds of the portion of the map this MapData describes.
        /// </summary>
        public RectangleF Bounds { get; }

        /// <summary>
        /// gets an int giving the zoom level for this MapData
        /// </summary>
        public int Zoom { get; }

        /// <summary>
        /// gets a List<LineSegment> containing the line segments
        /// to be drawn within the bounds and the zoom level 
        /// (or higher zoom) of this MapData. 
        /// </summary>
        public List<LineSegment> Lines { get; }

        /// <summary>
        /// Constuctor for MapData
        /// </summary>
        /// <param name="recF">A RectangleF giving the bounds.</param>
        /// <param name="zoom">An int giving the zoom level.</param>
        public MapData(RectangleF recF, int zoom)
        {
            Bounds = recF;
            Zoom = zoom;

            Lines = new List<LineSegment>();

        }



    }
}
