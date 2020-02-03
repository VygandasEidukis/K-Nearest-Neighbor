namespace K_nearest_neighbors.Data_Access.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Data.SQLite.EF6.Migrations;


    internal sealed class Configuration : DbMigrationsConfiguration<ClassificationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }

        protected override void Seed(ClassificationContext context)
        {
        }
    }
}
