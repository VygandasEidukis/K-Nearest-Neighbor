using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Models
{
    public interface IMesurable
    {
        float? Distance { get; set; }
        float X { get; set; }
        float Y { get; set; }
    }
}
