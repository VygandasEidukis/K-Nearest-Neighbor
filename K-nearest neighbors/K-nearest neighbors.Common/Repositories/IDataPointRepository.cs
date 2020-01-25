using K_nearest_neighbors.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Common.Repositories
{
    public interface IDataPointRepository
    {
        List<IGenericDto> GetAllDto();
        Point GetPointLimits();
        void SaveAssignedClassification(int id, int assignedClassification);
        void CreateNewDataPoint(DataPointDto dataPointDto);
    }
}
