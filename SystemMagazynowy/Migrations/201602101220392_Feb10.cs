namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Feb10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operations", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operations", "Discriminator");
        }
    }
}
