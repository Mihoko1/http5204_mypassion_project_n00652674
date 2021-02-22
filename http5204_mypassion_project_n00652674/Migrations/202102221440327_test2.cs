namespace http5204_mypassion_project_n00652674.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "MemberHasPic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Members", "PlayerHasPic");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Members", "PlayerHasPic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Members", "MemberHasPic");
        }
    }
}
