namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hairstyles",
                c => new
                    {
                        HairstyleID = c.Int(nullable: false, identity: true),
                        DateUpload = c.DateTime(nullable: false),
                        HairstylePhoto = c.Byte(nullable: false),
                        Type = c.String(),
                        Detail = c.String(),
                        MemberID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HairstyleID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Title = c.String(),
                        Email = c.String(),
                        Picture = c.Byte(nullable: false),
                        SalonID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MemberID)
                .ForeignKey("dbo.Salons", t => t.SalonID, cascadeDelete: true)
                .Index(t => t.SalonID);
            
            CreateTable(
                "dbo.Salons",
                c => new
                    {
                        SalonID = c.Int(nullable: false, identity: true),
                        SalonName = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Area = c.String(),
                        Website = c.String(),
                        Phone = c.String(),
                        SalonPicture = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.SalonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Hairstyles", "MemberID", "dbo.Members");
            DropForeignKey("dbo.Members", "SalonID", "dbo.Salons");
            DropIndex("dbo.Members", new[] { "SalonID" });
            DropIndex("dbo.Hairstyles", new[] { "MemberID" });
            DropTable("dbo.Salons");
            DropTable("dbo.Members");
            DropTable("dbo.Hairstyles");
        }
    }
}
