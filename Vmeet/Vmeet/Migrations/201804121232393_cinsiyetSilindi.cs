namespace Vmeet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cinsiyetSilindi : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Cinsiyet");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Cinsiyet", c => c.Int(nullable: false));
        }
    }
}
