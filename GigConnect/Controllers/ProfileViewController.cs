using GigConnect.Models;
using GigConnect.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GigConnect.Controllers
{
    public class ProfileViewController : Controller
    {
        // GET: ProfileView
        ApplicationDbContext context;

       public ProfileViewController()
        {
            context = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ViewBandProfile(int bandId)
        {
            Band band = GetBand(bandId);
            BandProfileViewModel model = new BandProfileViewModel();

            string imageUrl = await Services.FacebookAPI.GetProfilePicture(Private.Keys.facebookPageId);
            imageUrl = "test";

            //      public Band band { get; set; }

            //public List<GigInfoViewModel> gigs { get; set; }

            //public List<Review> reviews { get; set; }

            //public List<string> facebookPermalinks { get; set; }

            //public string twitterId { get; set; }

            // public List<string> youtubeUrls { get; set; }


            return View(model);

        }

        public ActionResult ViewVenueProfile(int venueId)
        {
            Venue venue = GetVenue(venueId);
            VenueProfileViewModel model = new VenueProfileViewModel();

            //      public Band band { get; set; }

            //public List<GigInfoViewModel> gigs { get; set; }

            //public List<Review> reviews { get; set; }

            //public List<string> facebookPermalinks { get; set; }

            //public string twitterId { get; set; }

            // public List<string> youtubeUrls { get; set; }


            return View(model);

        }





        // helpers //////////////////////////////////////////////////////////////////


        public Band GetBand(int id)
        {
            Band band = context.Bands.Where(b => b.BandId == id).FirstOrDefault();
            return band;
        }

        public Venue GetVenue(int id)
        {
            Venue venue = context.Venues.Where(b => b.VenueId == id).FirstOrDefault();
            return venue;
        }
        


    }
}