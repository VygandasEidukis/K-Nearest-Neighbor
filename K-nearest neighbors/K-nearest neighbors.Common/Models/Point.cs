using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Common.Models
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point()
        {

        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
