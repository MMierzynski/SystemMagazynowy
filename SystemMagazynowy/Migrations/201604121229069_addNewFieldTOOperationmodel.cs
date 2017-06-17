namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewFieldTOOperationmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operations", "OperationID", c => c.Int());
            CreateIndex("dbo.Operations", "OperationID");
            AddForeignKey("dbo.Operations", "OperationID", "dbo.Operations", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operations", "OperationID", "dbo.Operations");
            DropIndex("dbo.Operations", new[] { "OperationID" });
            DropColumn("dbo.Operations", "OperationID");
        }
    }
}
