using GigConnect.Models;
using GigConnect.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GigConnect.Services;

namespace GigConnect.Controllers
{
   // [Authorize(Roles = "Venue")]
    public class VenueController : Controller
    {
        public ApplicationDbContext context;
        public VenueController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Venue
        public ActionResult Index()
        {

            Venue venue = GetUserVenue();
            if (venue == null)
            {
                return RedirectToAction("Create", "Venue");
            }
            VenueIndexViewModel model = AssembleIndexViewModelForVenue();
            return View(model);
        }

      

       

        // GET: Venue/Create
        // GET: Band/Create
        public ActionResult Create()
        {
            CreateAndEditViewModel createView = new CreateAndEditViewModel();

            createView.Location = new Location();
            createView.Venue = new Venue();
            

            return View(createView);
        }

        // POST: Band/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateAndEditViewModel createInfo)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                createInfo.Venue.ApplicationId = userId;
                int nextLocationId = context.Database.ExecuteSqlCommand("SELECT IDENT_CURRENT('dbo.Locations')") + 1;

                createInfo.Venue.LocationId = nextLocationId;

                context.Venues.Add(createInfo.Venue);

                string[] latLng = await GeoCode.GetLatLongFromApi(createInfo.Location);
                createInfo.Location.lat = latLng[0];
                createInfo.Location.lng = latLng[1];

                context.Locations.Add(createInfo.Location);

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View();
            }
        }

     

        // GET: Venue/Edit/5
        public ActionResult Edit()
        {
            CreateAndEditViewModel toEdit = new CreateAndEditViewModel();
            toEdit.Venue = GetUserVenue();
            toEdit.Location = GetVenueLocation(toEdit.Venue);

            return View(toEdit);
        }

        // POST: Venue/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(CreateAndEditViewModel input)
        {
            try
            {
                Venue currentVenue = GetUserVenue();
                Location currentLocation = GetVenueLocation(currentVenue);

               
                currentVenue.venueName = input.Venue.venueName;
                currentVenue.genre = input.Venue.genre;
                currentVenue.websiteUrl = input.Venue.websiteUrl;
                currentVenue.description = input.Venue.description;

  

             
                currentLocation.address1 = input.Location.address1;
                currentLocation.address2 = input.Location.address2;
                currentLocation.city = input.Location.city;
                currentLocation.zip = input.Location.zip;
                currentLocation.state = input.Location.state;

               

                string[] latLng = await GeoCode.GetLatLongFromApi(currentLocation);
                currentLocation.lat = latLng[0];
                currentLocation.lng = latLng[1];

                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult BandDirectory()
        {
            List<Band> bands = context.Bands.ToList();
            return View(bands);
        }

        public VenueIndexViewModel AssembleIndexViewModelForVenue()
        {
            VenueIndexViewModel venueInfo = new VenueIndexViewModel();
            venueInfo.venue = GetUserVenue();
            venueInfo.currentGigs = GetGigViewModel(GetGigs(venueInfo.venue)); // contains Gig, BandsList, Location (formatted)
            venueInfo.openGigs = GetGigViewModel(GetOpenGigs(venueInfo.venue));
            venueInfo.messagesIn = GetAllMessagesIn(venueInfo.venue.VenueId);
            venueInfo.messagesOut = GetAllMessagesOut(venueInfo.venue.VenueId);
            venueInfo.requestsIn = GetRequestsIn(venueInfo.venue.VenueId);
            venueInfo.requestsOut = GetRequestsOut(venueInfo.venue.VenueId);
            venueInfo.requestResponses = GetRespondedRequests(venueInfo.venue.VenueId);
            venueInfo.reviews = GetVenueReviews(venueInfo.venue);
            venueInfo.score = AverageReviews(venueInfo.reviews);
            return venueInfo;
        }




        // HELPER FUNCTIONS //////////////////////////////////////////////



        public Location GetVenueLocation(Venue venue)
        {
            Location location = context.Locations.Where(l => l.LocationId == venue.LocationId).FirstOrDefault();
            return location;
        }
        public Venue GetUserVenue()
        {
            string userId = User.Identity.GetUserId();
            Venue venue = context.Venues
                .Include("Location").Where(b => b.ApplicationId == userId).FirstOrDefault();
            return venue;
        }

        public List<Gig> GetGigs(Venue venue)
        {
            List<Gig> venueGigs = new List<Gig>();
            List<Gig> allGigs = context.Gigs
                .Include("Venue").Where(g => g.venueId == venue.VenueId && g.open == false).OrderBy(o => o.timeOfGig).ToList();
            return allGigs;
        }
        public List<Gig> GetOpenGigs(Venue venue)
        {
            List<Gig> venueGigs = new List<Gig>();
            List<Gig> allGigs = context.Gigs
                .Include("Venue").Where(g => g.venueId == venue.VenueId && g.open == true).OrderBy(o => o.timeOfGig).ToList();
            return allGigs;
        }

        


        public List<Band> GetBandsOnGig(Gig gig)
        {
            List<Band> bands = context.BandGigs
                .Include("Band").Where(g => g.gigId == gig.GigId).Select(s => s.Band).ToList();
            return bands;

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


        public List<Message> GetAllMessagesIn(int venueId)
        {
            List<Message> messages = context.Messages
                .Include("Venue").Include("Band").Where(m => m.venueId == venueId && m.from == "Band").ToList();
            return messages;
        }

  

        public List<Message> GetAllMessagesOut(int venueId)
        {
            List<Message> messages = context.Messages
                .Include("Venue").Include("Band").Where(m => m.venueId == venueId && m.from == "Band").OrderBy(d => d.timeStamp).ToList();
            return messages;
        }

        public List<Request> GetRequestsIn(int venueId)
        {
            List<Request> requestsIn = context.Requests
                .Include("Venue").Include("Band").Where(r => r.venueId ==venueId && r.fromVenue == true && r.approved == false && r.denied == false)
                .OrderBy(o => o.timeStamp).ToList();
            return requestsIn;
        }
        public List<Request> GetRequestsOut(int venueId)
        {
            List<Request> requestsIn = context.Requests
                .Include("Venue").Include("Band").Where(r => r.venueId == venueId && r.fromVenue == true && r.approved == false && r.denied == false)
                .OrderBy(o => o.timeStamp).ToList();
            return requestsIn;
        }

        public List<Request> GetRespondedRequests(int venueId)
        {
            List<Request> requestsOut = context.Requests
                            .Include("Venue").Include("Band").Where(r => r.venueId == venueId && r.fromVenue == true && r.approved == true || r.denied == true)
                            .OrderBy(o => o.timeStamp).ToList();
            return requestsOut;
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
            foreach (Review review in reviews)
            {
                int ratingEnum = (int)review.rating + 1;
                score += Convert.ToDouble(review.rating);
            }
            double average = score / reviews.Count;
            return Math.Round(average, 1);
        }







    }
}
