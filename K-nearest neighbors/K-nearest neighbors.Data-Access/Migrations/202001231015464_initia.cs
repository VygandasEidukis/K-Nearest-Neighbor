namespace K_nearest_neighbors.Data_Access.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        X = c.Single(nullable: false),
                        Y = c.Single(nullable: false),
                        AssignedClassification = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DataPoints");
        }
    }
}
