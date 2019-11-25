namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCols : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "timeStamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "timeStamp");
        }
    }
}
