namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "locationId", "dbo.Locations");
            DropIndex("dbo.Requests", new[] { "locationId" });
            AddColumn("dbo.Gigs", "description", c => c.String());
            AddColumn("dbo.Requests", "timeStamp", c => c.DateTime(nullable: false));
            DropColumn("dbo.Requests", "locationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "locationId", c => c.Int(nullable: false));
            DropColumn("dbo.Requests", "timeStamp");
            DropColumn("dbo.Gigs", "description");
            CreateIndex("dbo.Requests", "locationId");
            AddForeignKey("dbo.Requests", "locationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
    }
}
