namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testcomp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Gigs", "locationId", "dbo.Locations");
            DropIndex("dbo.Gigs", new[] { "locationId" });
            CreateTable(
                "dbo.BandGigs",
                c => new
                    {
                        gigId = c.Int(nullable: false),
                        bandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.gigId, t.bandId })
                .ForeignKey("dbo.Bands", t => t.bandId, cascadeDelete: true)
                .ForeignKey("dbo.Gigs", t => t.gigId, cascadeDelete: true)
                .Index(t => t.gigId)
                .Index(t => t.bandId);
            
            DropColumn("dbo.Gigs", "locationId");
            DropColumn("dbo.Gigs", "bandsOnVenue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gigs", "bandsOnVenue", c => c.String());
            AddColumn("dbo.Gigs", "locationId", c => c.Int(nullable: false));
            DropForeignKey("dbo.BandGigs", "gigId", "dbo.Gigs");
            DropForeignKey("dbo.BandGigs", "bandId", "dbo.Bands");
            DropIndex("dbo.BandGigs", new[] { "bandId" });
            DropIndex("dbo.BandGigs", new[] { "gigId" });
            DropTable("dbo.BandGigs");
            CreateIndex("dbo.Gigs", "locationId");
            AddForeignKey("dbo.Gigs", "locationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
    }
}
