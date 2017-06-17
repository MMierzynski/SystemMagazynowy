namespace SystemMagazynowy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aa1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Operations", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Operations", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
