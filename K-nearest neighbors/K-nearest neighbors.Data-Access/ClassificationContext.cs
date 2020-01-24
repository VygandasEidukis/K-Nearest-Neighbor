using K_nearest_neighbors.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace K_nearest_neighbors.Data_Access
{
    public class ClassificationContext : DbContext
    {
        public DbSet<DataPoint> DataPoints { get; set; }

        public ClassificationContext(): base("name=_mainDB")
        {
        }
    }
}
