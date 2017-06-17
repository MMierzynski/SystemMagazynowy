namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateStacktaking : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocktakings", "WarehouseID", c => c.Int(nullable: false));
            AlterColumn("dbo.Stocktakings", "EndDate", c => c.DateTime());
            CreateIndex("dbo.Stocktakings", "WarehouseID");
            AddForeignKey("dbo.Stocktakings", "WarehouseID", "dbo.Warehouses", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocktakings", "WarehouseID", "dbo.Warehouses");
            DropIndex("dbo.Stocktakings", new[] { "WarehouseID" });
            AlterColumn("dbo.Stocktakings", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Stocktakings", "WarehouseID");
        }
    }
}
