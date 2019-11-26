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
            return View();
        }

        // GET: Venue/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        public ActionResult Edit(int id)
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

                currentVenue.VenueId = input.Venue.VenueId;
                currentVenue.venueName = input.Venue.venueName;
                currentVenue.genre = input.Venue.genre;
                currentVenue.websiteUrl = input.Venue.websiteUrl;
                currentVenue.description = input.Venue.description;

                currentVenue.LocationId = input.Band.LocationId;
                currentVenue.ApplicationId = input.Band.ApplicationId;

                currentLocation.LocationId = input.Location.LocationId;
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




        // HELPER FUNCTIONS //////////////////////////////////////////////



        public Location GetVenueLocation(Venue venue)
        {
            Location location = context.Locations.Where(l => l.LocationId == venue.LocationId).FirstOrDefault();
            return location;
        }
        public Venue GetUserVenue()
        {
            string userId = User.Identity.GetUserId();
            Venue venue = context.Venues.Where(b => b.ApplicationId == userId).FirstOrDefault();
            return venue;
        }

        public List<Gig> GetGigs(Venue venue)
        {
            List<Gig> venueGigs = new List<Gig>();
            List<Gig> allGigs = context.Gigs.Where(g => g.venueId == venue.VenueId).OrderBy(o => o.timeOfGig).ToList();
            return allGigs;
        }
    

        public List<Message> GetAllMessagesIn(int venueId)
        {
            List<Message> messages = context.Messages.Where(m => m.venueId == venueId && m.from == "Band").ToList();
            return messages;
        }

        public bool FilterForUnread(List<Message> messages)
        {
            var unread = messages.Where(u => u.read == false).ToList();
            if (unread.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Message> GetAllMessagesOut(int venueId)
        {
            List<Message> messages = context.Messages.Where(m => m.venueId == venueId && m.from == "Band").OrderBy(d => d.timeStamp).ToList();
            return messages;
        }

        public List<Request> GetRequestsIn(int venueId)
        {
            List<Request> requestsIn = context.Requests.Where(r => r.venueId ==venueId && r.fromVenue == true && r.approved == false && r.denied == false).ToList();
            return requestsIn;
        }
        public List<Request> GetRequestsOut(int venueId)
        {
            List<Request> requestsIn = context.Requests.Where(r => r.venueId == venueId && r.fromVenue == true && r.approved == false && r.denied == false).ToList();
            return requestsIn;
        }






    }
}
