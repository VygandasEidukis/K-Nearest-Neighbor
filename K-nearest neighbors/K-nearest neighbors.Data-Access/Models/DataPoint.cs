using K_nearest_neighbors.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Data_Access.Models
{
    public class DataPoint : IDtoConvertable
    {
        public int Id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int? AssignedClassification { get; set; }

        public void FromDto(IGenericDto dto)
        {
            this.Id = (dto as DataPointDto).Id;
            this.X = (dto as DataPointDto).X;
            this.Y = (dto as DataPointDto).Y;
            this.AssignedClassification = (dto as DataPointDto).AssignedClassification;
        }

        public IGenericDto ToDto()
        {
            var dataPointDto = new DataPointDto();

            dataPointDto.Id = this.Id;
            dataPointDto.X = this.X;
            dataPointDto.Y = this.Y;
            dataPointDto.AssignedClassification = this.AssignedClassification;

            return dataPointDto;
        }
    }
}
