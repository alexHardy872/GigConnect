namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class junkadd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "bandId", "dbo.Bands");
            DropForeignKey("dbo.Reviews", "venueId", "dbo.Venues");
            DropIndex("dbo.Reviews", new[] { "bandId" });
            DropIndex("dbo.Reviews", new[] { "venueId" });
            CreateTable(
                "dbo.BandReviews",
                c => new
                    {
                        reviewId = c.Int(nullable: false),
                        bandId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.reviewId, t.bandId })
                .ForeignKey("dbo.Bands", t => t.bandId, cascadeDelete: true)
                .ForeignKey("dbo.Reviews", t => t.reviewId, cascadeDelete: true)
                .Index(t => t.reviewId)
                .Index(t => t.bandId);
            
            CreateTable(
                "dbo.VenueReviews",
                c => new
                    {
                        reviewId = c.Int(nullable: false),
                        venueId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.reviewId, t.venueId })
                .ForeignKey("dbo.Reviews", t => t.reviewId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.venueId, cascadeDelete: true)
                .Index(t => t.reviewId)
                .Index(t => t.venueId);
            
            DropColumn("dbo.Reviews", "bandId");
            DropColumn("dbo.Reviews", "venueId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reviews", "venueId", c => c.Int());
            AddColumn("dbo.Reviews", "bandId", c => c.Int());
            DropForeignKey("dbo.VenueReviews", "venueId", "dbo.Venues");
            DropForeignKey("dbo.VenueReviews", "reviewId", "dbo.Reviews");
            DropForeignKey("dbo.BandReviews", "reviewId", "dbo.Reviews");
            DropForeignKey("dbo.BandReviews", "bandId", "dbo.Bands");
            DropIndex("dbo.VenueReviews", new[] { "venueId" });
            DropIndex("dbo.VenueReviews", new[] { "reviewId" });
            DropIndex("dbo.BandReviews", new[] { "bandId" });
            DropIndex("dbo.BandReviews", new[] { "reviewId" });
            DropTable("dbo.VenueReviews");
            DropTable("dbo.BandReviews");
            CreateIndex("dbo.Reviews", "venueId");
            CreateIndex("dbo.Reviews", "bandId");
            AddForeignKey("dbo.Reviews", "venueId", "dbo.Venues", "VenueId");
            AddForeignKey("dbo.Reviews", "bandId", "dbo.Bands", "BandId");
        }
    }
}
