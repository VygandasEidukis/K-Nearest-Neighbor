﻿namespace K_nearest_neighbors.Data_Access.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ClassificationContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ClassificationContext context)
        {
        }
    }
}
