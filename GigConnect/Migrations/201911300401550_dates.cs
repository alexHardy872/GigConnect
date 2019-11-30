namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Requests", "gigTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Requests", "gigTime", c => c.DateTime(nullable: false));
        }
    }
}
