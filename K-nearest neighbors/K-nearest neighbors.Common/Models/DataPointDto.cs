using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Common.Models
{
    public class DataPointDto : IGenericDto
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int? AssignedClassification { get; set; }
    }
}
