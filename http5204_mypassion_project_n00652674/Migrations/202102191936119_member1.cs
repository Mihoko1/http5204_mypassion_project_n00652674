namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class member1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "SalonName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Members", "SalonName");
        }
    }
}
