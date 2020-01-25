using K_nearest_neighbors.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Data_Access.Models
{
    public interface IDtoConvertable
    {
        IGenericDto ToDto();
        void FromDto(IGenericDto dto);
    }
}
