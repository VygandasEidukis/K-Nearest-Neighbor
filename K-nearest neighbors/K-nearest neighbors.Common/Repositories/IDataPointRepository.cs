using K_nearest_neighbors.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Common.Repositories
{
    public interface IDataPointRepository
    {
        List<IGenericDto> GetAllDto();
        Point GetPointLimits();
        Task SaveAssignedClassification(int id, int assignedClassification);
        Task CreateNewDataPoint(DataPointDto dataPointDto);
    }
}
