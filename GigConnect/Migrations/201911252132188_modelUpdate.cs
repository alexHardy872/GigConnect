namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modelUpdate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Reviews", "reviewOf");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "reviewOf", c => c.String());
        }
    }
}
