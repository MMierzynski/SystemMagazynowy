namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOperation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operations", "FromWarehouseID", c => c.Int());
            AddColumn("dbo.Operations", "ToWarehouseID", c => c.Int());
            AddColumn("dbo.Operations", "Warehouse_ID", c => c.Int());
            CreateIndex("dbo.Operations", "Warehouse_ID");
            AddForeignKey("dbo.Operations", "Warehouse_ID", "dbo.Warehouses", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "Warehouse_ID", "dbo.Warehouses");
            DropIndex("dbo.Operations", new[] { "Warehouse_ID" });
            DropColumn("dbo.Operations", "Warehouse_ID");
            DropColumn("dbo.Operations", "ToWarehouseID");
            DropColumn("dbo.Operations", "FromWarehouseID");
        }
    }
}
