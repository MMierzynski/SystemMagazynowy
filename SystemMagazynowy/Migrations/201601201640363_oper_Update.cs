namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oper_Update : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Operations", "FromWarehouseID");
            RenameColumn(table: "dbo.Operations", name: "Warehouse_ID", newName: "FromWarehouseID");
            RenameIndex(table: "dbo.Operations", name: "IX_Warehouse_ID", newName: "IX_FromWarehouseID");
            CreateIndex("dbo.Operations", "ToWarehouseID");
            AddForeignKey("dbo.Operations", "ToWarehouseID", "dbo.Warehouses", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "ToWarehouseID", "dbo.Warehouses");
            DropIndex("dbo.Operations", new[] { "ToWarehouseID" });
            RenameIndex(table: "dbo.Operations", name: "IX_FromWarehouseID", newName: "IX_Warehouse_ID");
            RenameColumn(table: "dbo.Operations", name: "FromWarehouseID", newName: "Warehouse_ID");
            AddColumn("dbo.Operations", "FromWarehouseID", c => c.Int());
        }
    }
}
