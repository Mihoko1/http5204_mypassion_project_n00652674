namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salonphoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Salons", "SalonHasPic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salons", "SalonHasPic");
        }
    }
}
