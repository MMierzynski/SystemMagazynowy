namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteWarehouseFormOperaion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OperationAssortments", "WarehouseID", "dbo.Warehouses");
            DropIndex("dbo.OperationAssortments", new[] { "WarehouseID" });
            DropColumn("dbo.OperationAssortments", "WarehouseID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OperationAssortments", "WarehouseID", c => c.Int(nullable: false));
            CreateIndex("dbo.OperationAssortments", "WarehouseID");
            AddForeignKey("dbo.OperationAssortments", "WarehouseID", "dbo.Warehouses", "ID", cascadeDelete: true);
        }
    }
}
