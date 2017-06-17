namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PwViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Number = c.Int(nullable: false),
                        OperationDate = c.DateTime(nullable: false),
                        ToWarehouseID = c.Int(nullable: false),
                        UserID = c.String(),
                        SelectedAssortment = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AssortmentViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BarCode = c.String(),
                        Quantity = c.Double(nullable: false),
                        PwViewModel_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PwViewModels", t => t.PwViewModel_ID)
                .Index(t => t.PwViewModel_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssortmentViewModels", "PwViewModel_ID", "dbo.PwViewModels");
            DropIndex("dbo.AssortmentViewModels", new[] { "PwViewModel_ID" });
            DropTable("dbo.AssortmentViewModels");
            DropTable("dbo.PwViewModels");
        }
    }
}
