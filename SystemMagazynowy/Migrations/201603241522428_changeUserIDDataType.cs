namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeUserIDDataType : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Stocktakings", new[] { "User_Id" });
            DropColumn("dbo.Stocktakings", "UserId");
            RenameColumn(table: "dbo.Stocktakings", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Stocktakings", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Stocktakings", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Stocktakings", new[] { "UserId" });
            AlterColumn("dbo.Stocktakings", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Stocktakings", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Stocktakings", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Stocktakings", "User_Id");
        }
    }
}
