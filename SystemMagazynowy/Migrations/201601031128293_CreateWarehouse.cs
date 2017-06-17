namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateWarehouse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assortments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        BarCode = c.String(nullable: false),
                        Unit = c.String(nullable: false, maxLength: 10),
                        MinimumQuantity = c.Double(nullable: false),
                        MaximumQuantity = c.Double(nullable: false),
                        InitialQuantity = c.Double(nullable: false),
                        ContractorID = c.Int(nullable: false),
                        AssortmentCategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AssortmentCategories", t => t.AssortmentCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Contractors", t => t.ContractorID, cascadeDelete: true)
                .Index(t => t.ContractorID)
                .Index(t => t.AssortmentCategoryID);
            
            CreateTable(
                "dbo.AssortmentCategories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Contractors",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        ZipCode = c.String(nullable: false),
                        City = c.String(nullable: false),
                        PhoneNumber = c.Int(),
                        Fax = c.Int(),
                        Email = c.String(),
                        NIP = c.String(maxLength: 10),
                        REGON = c.String(maxLength: 14),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AssortmentWarehouses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        WarehouseID = c.Int(nullable: false),
                        AssortmentID = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Assortments", t => t.AssortmentID, cascadeDelete: true)
                .ForeignKey("dbo.Warehouses", t => t.WarehouseID, cascadeDelete: true)
                .Index(t => t.WarehouseID)
                .Index(t => t.AssortmentID);
            
            CreateTable(
                "dbo.WarehouseAssortments",
                c => new
                    {
                        Warehouse_ID = c.Int(nullable: false),
                        Assortment_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Warehouse_ID, t.Assortment_ID })
                .ForeignKey("dbo.Warehouses", t => t.Warehouse_ID, cascadeDelete: true)
                .ForeignKey("dbo.Assortments", t => t.Assortment_ID, cascadeDelete: true)
                .Index(t => t.Warehouse_ID)
                .Index(t => t.Assortment_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssortmentWarehouses", "WarehouseID", "dbo.Warehouses");
            DropForeignKey("dbo.AssortmentWarehouses", "AssortmentID", "dbo.Assortments");
            DropForeignKey("dbo.WarehouseAssortments", "Assortment_ID", "dbo.Assortments");
            DropForeignKey("dbo.WarehouseAssortments", "Warehouse_ID", "dbo.Warehouses");
            DropForeignKey("dbo.Assortments", "ContractorID", "dbo.Contractors");
            DropForeignKey("dbo.Assortments", "AssortmentCategoryID", "dbo.AssortmentCategories");
            DropIndex("dbo.WarehouseAssortments", new[] { "Assortment_ID" });
            DropIndex("dbo.WarehouseAssortments", new[] { "Warehouse_ID" });
            DropIndex("dbo.AssortmentWarehouses", new[] { "AssortmentID" });
            DropIndex("dbo.AssortmentWarehouses", new[] { "WarehouseID" });
            DropIndex("dbo.Assortments", new[] { "AssortmentCategoryID" });
            DropIndex("dbo.Assortments", new[] { "ContractorID" });
            DropTable("dbo.WarehouseAssortments");
            DropTable("dbo.AssortmentWarehouses");
            DropTable("dbo.Warehouses");
            DropTable("dbo.Contractors");
            DropTable("dbo.AssortmentCategories");
            DropTable("dbo.Assortments");
        }
    }
}
