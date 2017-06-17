namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Operations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Number = c.Int(nullable: false),
                        OperationDate = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.OperationAssortments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OperationID = c.Int(nullable: false),
                        AssortmentID = c.Int(nullable: false),
                        WarehouseID = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assortments", t => t.AssortmentID, cascadeDelete: true)
                .ForeignKey("dbo.Operations", t => t.OperationID, cascadeDelete: true)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseID, cascadeDelete: true)
                .Index(t => t.OperationID)
                .Index(t => t.AssortmentID)
                .Index(t => t.WarehouseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OperationAssortments", "WarehouseID", "dbo.Warehouses");
            DropForeignKey("dbo.OperationAssortments", "OperationID", "dbo.Operations");
            DropForeignKey("dbo.OperationAssortments", "AssortmentID", "dbo.Assortments");
            DropIndex("dbo.OperationAssortments", new[] { "WarehouseID" });
            DropIndex("dbo.OperationAssortments", new[] { "AssortmentID" });
            DropIndex("dbo.OperationAssortments", new[] { "OperationID" });
            DropIndex("dbo.Operations", new[] { "User_Id" });
            DropTable("dbo.OperationAssortments");
            DropTable("dbo.Operations");
        }
    }
}
