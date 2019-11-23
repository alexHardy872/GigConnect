namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SocialMediaIds",
                c => new
                    {
                        SocialId = c.Int(nullable: false, identity: true),
                        facebookPageId = c.String(),
                        twitterHandle = c.String(),
                        youtubeChannelId = c.String(),
                    })
                .PrimaryKey(t => t.SocialId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestId = c.Int(nullable: false, identity: true),
                        locationId = c.Int(nullable: false),
                        eventId = c.Int(),
                        bandId = c.Int(nullable: false),
                        venueId = c.Int(nullable: false),
                        gigTime = c.DateTime(nullable: false),
                        fromBand = c.Boolean(nullable: false),
                        fromVenue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RequestId)
                .ForeignKey("dbo.Bands", t => t.bandId, cascadeDelete: true)
                .ForeignKey("dbo.Gigs", t => t.eventId)
                .ForeignKey("dbo.Locations", t => t.locationId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.venueId, cascadeDelete: true)
                .Index(t => t.locationId)
                .Index(t => t.eventId)
                .Index(t => t.bandId)
                .Index(t => t.venueId);
            
            AddColumn("dbo.Bands", "socialId", c => c.Int(nullable: false));
            AddColumn("dbo.Venues", "description", c => c.String());
            AddColumn("dbo.Venues", "websiteUrl", c => c.String());
            AddColumn("dbo.Venues", "genre", c => c.Int(nullable: false));
            CreateIndex("dbo.Bands", "socialId");
            AddForeignKey("dbo.Bands", "socialId", "dbo.SocialMediaIds", "SocialId", cascadeDelete: true);
            DropColumn("dbo.Bands", "facebookPageId");
            DropColumn("dbo.Bands", "twitterPageHandle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bands", "twitterPageHandle", c => c.String());
            AddColumn("dbo.Bands", "facebookPageId", c => c.String());
            DropForeignKey("dbo.Requests", "venueId", "dbo.Venues");
            DropForeignKey("dbo.Requests", "locationId", "dbo.Locations");
            DropForeignKey("dbo.Requests", "eventId", "dbo.Gigs");
            DropForeignKey("dbo.Requests", "bandId", "dbo.Bands");
            DropForeignKey("dbo.Bands", "socialId", "dbo.SocialMediaIds");
            DropIndex("dbo.Requests", new[] { "venueId" });
            DropIndex("dbo.Requests", new[] { "bandId" });
            DropIndex("dbo.Requests", new[] { "eventId" });
            DropIndex("dbo.Requests", new[] { "locationId" });
            DropIndex("dbo.Bands", new[] { "socialId" });
            DropColumn("dbo.Venues", "genre");
            DropColumn("dbo.Venues", "websiteUrl");
            DropColumn("dbo.Venues", "description");
            DropColumn("dbo.Bands", "socialId");
            DropTable("dbo.Requests");
            DropTable("dbo.SocialMediaIds");
        }
    }
}
