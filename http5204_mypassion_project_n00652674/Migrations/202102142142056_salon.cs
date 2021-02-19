namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salon : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Salons", "SalonPicture", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Salons", "SalonPicture", c => c.Byte(nullable: false));
        }
    }
}
