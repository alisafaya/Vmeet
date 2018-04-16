namespace Vmeet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class girisAddedToMesaj : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Giris", "LinkID", "dbo.Links");
            DropIndex("dbo.Giris", new[] { "LinkID" });
            AddColumn("dbo.Mesajs", "GirisID", c => c.Int());
            AlterColumn("dbo.Giris", "LinkID", c => c.Int());
            CreateIndex("dbo.Giris", "LinkID");
            CreateIndex("dbo.Mesajs", "GirisID");
            AddForeignKey("dbo.Mesajs", "GirisID", "dbo.Giris", "ID");
            AddForeignKey("dbo.Giris", "LinkID", "dbo.Links", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Giris", "LinkID", "dbo.Links");
            DropForeignKey("dbo.Mesajs", "GirisID", "dbo.Giris");
            DropIndex("dbo.Mesajs", new[] { "GirisID" });
            DropIndex("dbo.Giris", new[] { "LinkID" });
            AlterColumn("dbo.Giris", "LinkID", c => c.Int(nullable: false));
            DropColumn("dbo.Mesajs", "GirisID");
            CreateIndex("dbo.Giris", "LinkID");
            AddForeignKey("dbo.Giris", "LinkID", "dbo.Links", "ID", cascadeDelete: true);
        }
    }
}
