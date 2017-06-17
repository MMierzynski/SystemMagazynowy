namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOper : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Operations", new[] { "User_Id" });
            DropColumn("dbo.Operations", "UserId");
            RenameColumn(table: "dbo.Operations", name: "User_Id", newName: "UserId");
            AddColumn("dbo.Operations", "ContractorId", c => c.Int());
            AlterColumn("dbo.Operations", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Operations", "ContractorId");
            CreateIndex("dbo.Operations", "UserId");
            AddForeignKey("dbo.Operations", "ContractorId", "dbo.Contractors", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "ContractorId", "dbo.Contractors");
            DropIndex("dbo.Operations", new[] { "UserId" });
            DropIndex("dbo.Operations", new[] { "ContractorId" });
            AlterColumn("dbo.Operations", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.Operations", "ContractorId");
            RenameColumn(table: "dbo.Operations", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Operations", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Operations", "User_Id");
        }
    }
}
