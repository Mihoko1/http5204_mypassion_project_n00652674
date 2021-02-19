namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tablechange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Members", "Picture", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Members", "Picture", c => c.Byte(nullable: false));
        }
    }
}
