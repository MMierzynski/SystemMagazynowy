namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_stocktaking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StocktakingAssortments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StocktakingID = c.Int(nullable: false),
                        AssortmentID = c.Int(nullable: false),
                        BeforeQuantity = c.Double(nullable: false),
                        AfterQuantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assortments", t => t.AssortmentID, cascadeDelete: true)
                .ForeignKey("dbo.Stocktakings", t => t.StocktakingID, cascadeDelete: true)
                .Index(t => t.StocktakingID)
                .Index(t => t.AssortmentID);
            
            CreateTable(
                "dbo.Stocktakings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsOpen = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocktakings", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StocktakingAssortments", "StocktakingID", "dbo.Stocktakings");
            DropForeignKey("dbo.StocktakingAssortments", "AssortmentID", "dbo.Assortments");
            DropIndex("dbo.Stocktakings", new[] { "User_Id" });
            DropIndex("dbo.StocktakingAssortments", new[] { "AssortmentID" });
            DropIndex("dbo.StocktakingAssortments", new[] { "StocktakingID" });
            DropTable("dbo.Stocktakings");
            DropTable("dbo.StocktakingAssortments");
        }
    }
}
