﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GigConnect.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Band> Bands { get; set; }
        public DbSet<Venue> Venues { get; set; }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<SocialMediaIds> Socials { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<BandGig> BandGigs { get; set; }

        public DbSet<BandReview> BandReviews { get; set; }

        public DbSet<VenueReview> VenueReviews { get; set; }



    }
}