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
                throw new Exception("No data points found");
            point.X = dataPoints.OrderByDescending(x => x.X).FirstOrDefault().X;
            point.Y = dataPoints.OrderByDescending(y => y.Y).FirstOrDefault().Y;

            return point;
        }
    }
}
