namespace GigConnect.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bands",
                c => new
                    {
                        BandId = c.Int(nullable: false, identity: true),
                        bandName = c.String(nullable: false),
                        genre = c.Int(nullable: false),
                        town = c.String(nullable: false),
                        bandWebsite = c.String(nullable: false),
                        LocationId = c.Int(),
                        socialId = c.Int(nullable: false),
                        ApplicationId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.BandId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.SocialMediaIds", t => t.socialId, cascadeDelete: true)
                .Index(t => t.LocationId)
                .Index(t => t.socialId)
                .Index(t => t.ApplicationId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        lat = c.String(),
                        lng = c.String(),
                        address1 = c.String(),
                        address2 = c.String(),
                        city = c.String(),
                        state = c.Int(nullable: false),
                        zip = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.LocationId);
            
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
                "dbo.Gigs",
                c => new
                    {
                        GigId = c.Int(nullable: false, identity: true),
                        locationId = c.Int(nullable: false),
                        venueId = c.Int(nullable: false),
                        bandsOnVenue = c.String(),
                        timeOfGig = c.DateTime(nullable: false),
                        open = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.GigId)
                .ForeignKey("dbo.Locations", t => t.locationId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.venueId, cascadeDelete: true)
                .Index(t => t.locationId)
                .Index(t => t.venueId);
            
            CreateTable(
                "dbo.Venues",
                c => new
                    {
                        VenueId = c.Int(nullable: false, identity: true),
                        venueName = c.String(nullable: false),
                        town = c.String(nullable: false),
                        description = c.String(),
                        websiteUrl = c.String(nullable: false),
                        genre = c.Int(nullable: false),
                        ApplicationId = c.String(maxLength: 128),
                        LocationId = c.Int(),
                    })
                .PrimaryKey(t => t.VenueId)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.ApplicationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        from = c.String(),
                        venueId = c.Int(nullable: false),
                        bandId = c.Int(nullable: false),
                        messageContent = c.String(),
                        timeStamp = c.DateTime(nullable: false),
                        read = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.Bands", t => t.bandId, cascadeDelete: true)
                .ForeignKey("dbo.Venues", t => t.venueId, cascadeDelete: true)
                .Index(t => t.venueId)
                .Index(t => t.bandId);
            
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
                        message = c.String(),
                        fromBand = c.Boolean(nullable: false),
                        fromVenue = c.Boolean(nullable: false),
                        approved = c.Boolean(nullable: false),
                        denied = c.Boolean(nullable: false),
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
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        reviewOf = c.String(),
                        rating = c.Int(nullable: false),
                        content = c.String(),
                        bandId = c.Int(),
                        venueId = c.Int(),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Bands", t => t.bandId)
                .ForeignKey("dbo.Venues", t => t.venueId)
                .Index(t => t.bandId)
                .Index(t => t.venueId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Reviews", "venueId", "dbo.Venues");
            DropForeignKey("dbo.Reviews", "bandId", "dbo.Bands");
            DropForeignKey("dbo.Requests", "venueId", "dbo.Venues");
            DropForeignKey("dbo.Requests", "locationId", "dbo.Locations");
            DropForeignKey("dbo.Requests", "eventId", "dbo.Gigs");
            DropForeignKey("dbo.Requests", "bandId", "dbo.Bands");
            DropForeignKey("dbo.Messages", "venueId", "dbo.Venues");
            DropForeignKey("dbo.Messages", "bandId", "dbo.Bands");
            DropForeignKey("dbo.Gigs", "venueId", "dbo.Venues");
            DropForeignKey("dbo.Venues", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Venues", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Gigs", "locationId", "dbo.Locations");
            DropForeignKey("dbo.Bands", "socialId", "dbo.SocialMediaIds");
            DropForeignKey("dbo.Bands", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Bands", "ApplicationId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Reviews", new[] { "venueId" });
            DropIndex("dbo.Reviews", new[] { "bandId" });
            DropIndex("dbo.Requests", new[] { "venueId" });
            DropIndex("dbo.Requests", new[] { "bandId" });
            DropIndex("dbo.Requests", new[] { "eventId" });
            DropIndex("dbo.Requests", new[] { "locationId" });
            DropIndex("dbo.Messages", new[] { "bandId" });
            DropIndex("dbo.Messages", new[] { "venueId" });
            DropIndex("dbo.Venues", new[] { "LocationId" });
            DropIndex("dbo.Venues", new[] { "ApplicationId" });
            DropIndex("dbo.Gigs", new[] { "venueId" });
            DropIndex("dbo.Gigs", new[] { "locationId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bands", new[] { "ApplicationId" });
            DropIndex("dbo.Bands", new[] { "socialId" });
            DropIndex("dbo.Bands", new[] { "LocationId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Reviews");
            DropTable("dbo.Requests");
            DropTable("dbo.Messages");
            DropTable("dbo.Venues");
            DropTable("dbo.Gigs");
            DropTable("dbo.SocialMediaIds");
            DropTable("dbo.Locations");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bands");
        }
    }
}
