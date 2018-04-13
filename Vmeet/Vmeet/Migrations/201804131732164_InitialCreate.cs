namespace Vmeet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Avatars",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DosyaID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dosyas", t => t.DosyaID, cascadeDelete: true)
                .Index(t => t.DosyaID);
            
            CreateTable(
                "dbo.Dosyas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DosyaIsmi = c.String(maxLength: 256),
                        DosyaKodu = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Giris",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Tarih = c.DateTime(nullable: false),
                        AvatarID = c.Int(nullable: false),
                        Isim = c.String(nullable: false, maxLength: 256),
                        LinkID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Avatars", t => t.AvatarID, cascadeDelete: true)
                .ForeignKey("dbo.Links", t => t.LinkID, cascadeDelete: true)
                .Index(t => t.AvatarID)
                .Index(t => t.LinkID);
            
            CreateTable(
                "dbo.Links",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OzelMi = c.Boolean(nullable: false),
                        ToplantiID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Toplantis", t => t.ToplantiID, cascadeDelete: true)
                .Index(t => t.ToplantiID);
            
            CreateTable(
                "dbo.Toplantis",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        YoneticiID = c.String(maxLength: 128),
                        ToplantiAdi = c.String(nullable: false, maxLength: 64),
                        Konu = c.String(nullable: false, maxLength: 256),
                        BaslamaZamani = c.DateTime(nullable: false),
                        BitisZamani = c.DateTime(nullable: false),
                        OzelMi = c.Boolean(nullable: false),
                        Cikti = c.String(maxLength: 1024),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.YoneticiID)
                .Index(t => t.YoneticiID);
            
            CreateTable(
                "dbo.Katilimcis",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Izin = c.Int(nullable: false),
                        ToplantiID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Toplantis", t => t.ToplantiID, cascadeDelete: true)
                .Index(t => t.ToplantiID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ad = c.String(maxLength: 32),
                        Soyad = c.String(maxLength: 32),
                        DosyaID = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dosyas", t => t.DosyaID)
                .Index(t => t.DosyaID)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Mesajs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Metin = c.String(),
                        MesajTuru = c.Int(nullable: false),
                        ToplantiID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        DosyaID = c.Int(),
                        Tarih = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Dosyas", t => t.DosyaID)
                .ForeignKey("dbo.Toplantis", t => t.ToplantiID, cascadeDelete: true)
                .Index(t => t.ToplantiID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.DosyaID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Toplantis", "YoneticiID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Links", "ToplantiID", "dbo.Toplantis");
            DropForeignKey("dbo.Katilimcis", "ToplantiID", "dbo.Toplantis");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Mesajs", "ToplantiID", "dbo.Toplantis");
            DropForeignKey("dbo.Mesajs", "DosyaID", "dbo.Dosyas");
            DropForeignKey("dbo.Mesajs", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Katilimcis", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "DosyaID", "dbo.Dosyas");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Giris", "LinkID", "dbo.Links");
            DropForeignKey("dbo.Giris", "AvatarID", "dbo.Avatars");
            DropForeignKey("dbo.Avatars", "DosyaID", "dbo.Dosyas");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Mesajs", new[] { "DosyaID" });
            DropIndex("dbo.Mesajs", new[] { "ApplicationUserID" });
            DropIndex("dbo.Mesajs", new[] { "ToplantiID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "DosyaID" });
            DropIndex("dbo.Katilimcis", new[] { "ApplicationUserID" });
            DropIndex("dbo.Katilimcis", new[] { "ToplantiID" });
            DropIndex("dbo.Toplantis", new[] { "YoneticiID" });
            DropIndex("dbo.Links", new[] { "ToplantiID" });
            DropIndex("dbo.Giris", new[] { "LinkID" });
            DropIndex("dbo.Giris", new[] { "AvatarID" });
            DropIndex("dbo.Avatars", new[] { "DosyaID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Mesajs");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Katilimcis");
            DropTable("dbo.Toplantis");
            DropTable("dbo.Links");
            DropTable("dbo.Giris");
            DropTable("dbo.Dosyas");
            DropTable("dbo.Avatars");
        }
    }
}
