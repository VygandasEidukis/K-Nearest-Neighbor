using K_nearest_neighbors.Data_Access.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using SQLite.CodeFirst;
using System.Data.SQLite;

namespace K_nearest_neighbors.Data_Access
{
    public class ClassificationContext : DbContext
    {
        public DbSet<DataPoint> DataPoints { get; set; }
        //"name=_mainDB"
        //"Data Source=DataPoints.db"
        public ClassificationContext(): base("_sqliteDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<ClassificationContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<ClassificationContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
