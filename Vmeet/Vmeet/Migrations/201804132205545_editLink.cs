namespace Vmeet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editLink : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Links", "Anahtar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Links", "Anahtar");
        }
    }
}
