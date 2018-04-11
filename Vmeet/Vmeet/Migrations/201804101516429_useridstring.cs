namespace Vmeet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class useridstring : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Toplantis", new[] { "Yonetici_Id" });
            DropIndex("dbo.Katilimcis", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Mesajs", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Toplantis", "YoneticiID");
            DropColumn("dbo.Katilimcis", "ApplicationUserID");
            DropColumn("dbo.Mesajs", "ApplicationUserID");
            RenameColumn(table: "dbo.Toplantis", name: "Yonetici_Id", newName: "YoneticiID");
            RenameColumn(table: "dbo.Katilimcis", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            RenameColumn(table: "dbo.Mesajs", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            AddColumn("dbo.Links", "Anahtar", c => c.String());
            AlterColumn("dbo.Toplantis", "YoneticiID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Katilimcis", "ApplicationUserID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Mesajs", "ApplicationUserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Toplantis", "YoneticiID");
            CreateIndex("dbo.Katilimcis", "ApplicationUserID");
            CreateIndex("dbo.Mesajs", "ApplicationUserID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Mesajs", new[] { "ApplicationUserID" });
            DropIndex("dbo.Katilimcis", new[] { "ApplicationUserID" });
            DropIndex("dbo.Toplantis", new[] { "YoneticiID" });
            AlterColumn("dbo.Mesajs", "ApplicationUserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Katilimcis", "ApplicationUserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Toplantis", "YoneticiID", c => c.Int(nullable: false));
            DropColumn("dbo.Links", "Anahtar");
            RenameColumn(table: "dbo.Mesajs", name: "ApplicationUserID", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Katilimcis", name: "ApplicationUserID", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Toplantis", name: "YoneticiID", newName: "Yonetici_Id");
            AddColumn("dbo.Mesajs", "ApplicationUserID", c => c.Int(nullable: false));
            AddColumn("dbo.Katilimcis", "ApplicationUserID", c => c.Int(nullable: false));
            AddColumn("dbo.Toplantis", "YoneticiID", c => c.Int(nullable: false));
            CreateIndex("dbo.Mesajs", "ApplicationUser_Id");
            CreateIndex("dbo.Katilimcis", "ApplicationUser_Id");
            CreateIndex("dbo.Toplantis", "Yonetici_Id");
        }
    }
}
