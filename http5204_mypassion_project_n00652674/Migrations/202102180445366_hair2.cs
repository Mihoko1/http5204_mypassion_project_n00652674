namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hair2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Hairstyles", "FirstName", c => c.String());
            AddColumn("dbo.Hairstyles", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Hairstyles", "LastName");
            DropColumn("dbo.Hairstyles", "FirstName");
        }
    }
}
