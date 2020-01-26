using K_nearest_neighbors.Common.Models;
using K_nearest_neighbors.Common.Repositories;
using K_nearest_neighbors.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Data_Access.Repositories
{
    public class DataPointRepository : GenericRepository<DataPoint>, IDataPointRepository
    {
        public DataPointRepository(ClassificationContext context) : base(context)
        {

        }

        public async Task CreateNewDataPoint(DataPointDto dataPointDto)
        {
            var dataPoint = new DataPoint(dataPointDto);
            await Add(dataPoint);
            await SaveChanges();
        }

        public List<IGenericDto> GetAllDto()
        {
            var dataPoints = base.GetAll();
            List<IGenericDto> dataPointsDto = new List<IGenericDto>();
            foreach(var dataPoint in dataPoints)
            {
                dataPointsDto.Add(dataPoint.ToDto());
            }

            return dataPointsDto;
        }

        public Point GetPointLimits()
        {
            Point point = new Point();
            IQueryable<DataPoint> dataPoints = base.GetQueryables();
            if (dataPoints.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("No data points found", "Fatal error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return null;
            }
            point.X = dataPoints.OrderByDescending(x => x.X).FirstOrDefault().X;
            point.Y = dataPoints.OrderByDescending(y => y.Y).FirstOrDefault().Y;

            return point;
        }

        public async Task SaveAssignedClassification(int id, int assignedClassification)
        {
            DataPoint dataPoint = base.GetQueryables().Where(x => x.Id == id).SingleOrDefault();
            dataPoint.AssignedClassification = assignedClassification;
            await Update(dataPoint);
            await SaveChanges();
        }
    }
}
