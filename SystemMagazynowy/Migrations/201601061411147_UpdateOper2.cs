namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOper2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Operations", new[] { "ContractorId" });
            CreateIndex("dbo.Operations", "ContractorID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Operations", new[] { "ContractorID" });
            CreateIndex("dbo.Operations", "ContractorId");
        }
    }
}
