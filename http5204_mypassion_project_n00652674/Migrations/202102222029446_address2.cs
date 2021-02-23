namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class address2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Salons", "Postal", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Salons", "Postal");
        }
    }
}
