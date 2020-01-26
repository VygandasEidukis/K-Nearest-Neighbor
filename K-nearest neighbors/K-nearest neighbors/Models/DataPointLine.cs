using K_nearest_neighbors.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Models
{
    public class DataPointLine
    {
        public float X1 { get; set; }
        public float X2 { get; set; }
        public float Y1 { get; set; }
        public float Y2 { get; set; }

        public DataPointLine(DataPointDto dataPoint1, DataPointDto dataPoint2, float offset)
        {
            X1 = dataPoint1.X + offset;
            Y1 = dataPoint1.Y + offset;
            X2 = dataPoint2.X + offset;
            Y2 = dataPoint2.Y + offset;
        }
    }
}
