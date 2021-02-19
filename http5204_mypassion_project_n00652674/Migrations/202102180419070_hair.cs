namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hair : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Hairstyles", "HairstylePhoto", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Hairstyles", "HairstylePhoto", c => c.Byte(nullable: false));
        }
    }
}
