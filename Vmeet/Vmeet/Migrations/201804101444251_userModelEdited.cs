namespace Vmeet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userModelEdited : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "DosyaID", "dbo.Dosyas");
            DropIndex("dbo.AspNetUsers", new[] { "DosyaID" });
            AlterColumn("dbo.AspNetUsers", "DosyaID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "DosyaID");
            AddForeignKey("dbo.AspNetUsers", "DosyaID", "dbo.Dosyas", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "DosyaID", "dbo.Dosyas");
            DropIndex("dbo.AspNetUsers", new[] { "DosyaID" });
            AlterColumn("dbo.AspNetUsers", "DosyaID", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "DosyaID");
            AddForeignKey("dbo.AspNetUsers", "DosyaID", "dbo.Dosyas", "ID", cascadeDelete: true);
        }
    }
}
