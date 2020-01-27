using K_nearest_neighbors.Common.Models;
using System.Windows.Media;

namespace K_nearest_neighbors.Models
{
    public class ColoredDataPoint : DataPointDto, IMesurable
    {
        public Brush brush { get; set; }
        public float? Distance { get; set; } = null;

        public ColoredDataPoint()
        {

        }

        public ColoredDataPoint(DataPointDto parent)
        {
            AddParent(parent);
        }

        public void AddParent(DataPointDto parent)
        {
            base.X = parent.X;
            base.Y = parent.Y;
            base.AssignedClassification = parent.AssignedClassification;
            base.Id = parent.Id;
        }
    }
}
