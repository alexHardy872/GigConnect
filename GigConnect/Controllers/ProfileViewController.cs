using GigConnect.Models;
using GigConnect.Models.ViewModels;
using GigConnect.Services;
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

            SocialMediaIds socials = GetBandSocials(band.socialId);

            model.band = band;
            model.facebookImageUrl = await Services.FacebookAPI.GetProfilePicture(socials.facebookPageId);
            model.facebookPermalinks = await Services.FacebookAPI.GetPermaUrlFromPost(socials.facebookPageId);

            //model.youtubeUrls = await Services.YouTubeAPI.
            //model.youtube....

            model.reviews = GetBandReviews(band);
            model.score = AverageReviews(model.reviews);

            model.gigs =  GetGigViewModel(GetGigs(band));


            return View(model);

        }

        public List<string> YoutubeTest()
        {
            List<string> YoutubeUrls = new List<string>();

            Band band = GetBand(1);
            SocialMediaIds socials = context.Socials.Where(s => s.SocialId == band.socialId).FirstOrDefault();
            YouTubeAPI youtube = new YouTubeAPI();
            YoutubeUrls = youtube.GetUrlsOfTopVideos(socials.youtubeChannelId);

            return YoutubeUrls;
        }

        public ActionResult ViewVenueProfile(int venueId)
        {
            Venue venue = GetVenue(venueId);
            VenueProfileViewModel model = new VenueProfileViewModel();

            model.gigs = GetGigViewModel(GetGigs(venue));
            model.venue = venue;
            model.reviews = GetVenueReviews(venue);
            model.score = AverageReviews(model.reviews);

            return View(model);

        }





        // helpers //////////////////////////////////////////////////////////////////


        public Band GetBand(int id)
        {
            Band band = context.Bands
                .Include("Social").Where(b => b.BandId == id).FirstOrDefault();
            return band;
        }

        public Venue GetVenue(int id)
        {
            Venue venue = context.Venues
                .Include("Location").Where(b => b.VenueId == id).FirstOrDefault();
            return venue;
        }

        public SocialMediaIds GetBandSocials(int id)
        {
            SocialMediaIds bandInfo = context.Socials.Where(s => s.SocialId == id).FirstOrDefault();
            return bandInfo;
        }

        ///////////////////////// REVIEWS
        
        public List<Review> GetBandReviews(Band band)
        {
            List<Review> reviews = context.BandReviews
                .Include("Review")
                .Where(r => r.bandId == band.BandId)
                .Select(s => s.Review).ToList();
            return reviews;
        }

        public List<Review> GetVenueReviews(Venue venue)
        {
            List<Review> reviews = context.VenueReviews
                .Include("Review")
                .Where(r => r.venueId == venue.VenueId)
                .Select(s => s.Review).ToList();
            return reviews;
        }

  

        public double AverageReviews(List<Review> reviews)
        {
            double score = 0;
            foreach(Review review in reviews)
            {
                score += review.rating;
            }
            double average = score / reviews.Count;
            return Math.Round(average, 1);
        }



        /////////////////////////// GIGS //////////////////////
        /// BAND ////

        public List<Band> GetBandsOnGig(Gig gig)
        {
            List<Band> bands = context.BandGigs.Where(g => g.gigId == gig.GigId).Select(s => s.Band).ToList();
            return bands;

        }
        public List<Gig> GetGigs(Band band)
        {
            List<Gig> bandGigs = new List<Gig>();
            List<int> allBandGigIds = context.BandGigs.Where(b => b.bandId == band.BandId).Select(s => s.gigId).ToList();
            foreach (int gigId in allBandGigIds)
            {
                bandGigs.Add(context.Gigs
                    .Include("Venue")
                    .Where(g => g.GigId == gigId).FirstOrDefault());
            }
            bandGigs = bandGigs.Where(g => g.timeOfGig > DateTime.Now).OrderBy(o => o.timeOfGig).ToList();
            return bandGigs;
        }

        public List<Gig> GetGigs(Venue venue)
        {
            List<Gig> Gigs = context.Gigs
                .Include("Venue").Where(b => b.venueId == venue.VenueId && b.timeOfGig > DateTime.Now).ToList();
            return Gigs;
        }

        public List<GigInfoViewModel> GetGigViewModel(List<Gig> gigs)
        {
            List<GigInfoViewModel> gigModels = new List<GigInfoViewModel>();
            foreach (Gig gig in gigs)
            {
                GigInfoViewModel tempGigModel = new GigInfoViewModel();
                tempGigModel.gig = gig;
                tempGigModel.bands = GetBandsOnGig(gig);
                Location location = context.Locations.Where(l => l.LocationId == gig.Venue.LocationId).FirstOrDefault();
                tempGigModel.formattedAddress = GeoCode.FormatAddress(location);
                gigModels.Add(tempGigModel);

            }
            return gigModels;
        }


        
        

     
      

        







    }
}