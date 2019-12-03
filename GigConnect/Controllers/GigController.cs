using GigConnect.Models;
using GigConnect.Models.ViewModels;
using GigConnect.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GigConnect.Controllers
{
    //[Authorize(Roles = "Venue")]
    public class GigController : Controller
    {
        ApplicationDbContext context;
        public GigController()
        {
            context = new ApplicationDbContext();
        }



        // GET: Gig
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult Create()
        {
            Gig gig = new Gig();
            gig.venueId = GetUserVenue().VenueId;
            gig.open = true;
            gig.timeOfGig = DateTime.Now;


            return View(gig);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Gig gig)
        {

            context.Gigs.Add(gig);
            await context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }


        public async Task<ActionResult> CreateGigFromRequest(int requestId)
        {
            Gig gig = new Gig();
            Request request = GetRequestFromId(requestId);
            gig.timeOfGig = (DateTime)request.gigTime;
            gig.venueId = request.venueId;
            gig.open = true;
            context.Gigs.Add(gig);
            AddBandToGig(request.bandId, gig.GigId);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddBandFromRequest(int requestId)
        {
            Request request = GetRequestFromId(requestId);
            int gigId = request.eventId ?? default(int);
           AddBandToGig(request.bandId, gigId);

            return RedirectToAction("Index", "Home");

        }
      
        public ActionResult AddSelectedBandToGig(int bandId) // from venue carrying venue gigs.
        {

            Band band = GetBandFromId(bandId);
            Venue venue = GetUserVenue();
            AddBandToGigViewModel model = new AddBandToGigViewModel();
            List<Gig> openGigs = GetOpenGigs(venue);
            model.gigs =   GetGigViewModel(openGigs);
            model.band = band;
            
            return View(model);
        }

      
        
        public void AddBandToGig(int bandId, int gigId)
        {
            BandGig junction = new BandGig();
            junction.gigId = gigId;
            junction.bandId = bandId;

            context.BandGigs.Add(junction);
            context.SaveChanges();
            
        }

        public ActionResult Details(int gigId)
        {

            GigInfoViewModel model = new GigInfoViewModel();
            model.gig = GetGigFromId(gigId);
            model.bands = GetBandsOnGig(model.gig);
            Location location = context.Locations.Where(l => l.LocationId == model.gig.Venue.LocationId).FirstOrDefault();
            model.formattedAddress = GeoCode.FormatAddress(location);

            return View(gigId);
        }


        public async Task<ActionResult> Delete(int gigId)
        {
            Gig gig = GetGigById(gigId);
            List<Band> bands = GetBandsOnGig(gig);
            List<Request> requests = GetRequestsForGig(gigId);

            foreach (Band band in bands)
            {
                RemoveBandFromGig(band.BandId, gig.GigId); // delete bandGig junction                 
            }
            foreach (Request request in requests)
            {
                context.Requests.Remove(request);
            }
            if (gig.timeOfGig > DateTime.Now)
            {
                string cancellationMessage = "The show on " + gig.timeOfGig.ToShortDateString() + " at " + gig.Venue.venueName + " has been cancelled";
                MessageAllBandsOnGig(bands, gig.venueId, cancellationMessage);
                //return RedirectToAction("MessageAllBandsOnGig", "Message", new { bands = bands, venueId = gig.venueId, content = cancellationMessage });            
            }
            context.Gigs.Remove(gig);
            await context.SaveChangesAsync();

           
                return RedirectToAction("Index", "Home");
            }
        public ActionResult List()
        {
            List<GigInfoViewModel> models = GetAllGigs();
            return View(models);
        }



        public ActionResult Edit(int gigId)
        {
            GigInfoViewModel model = new GigInfoViewModel();
            model.gig = GetGigFromId(gigId);
            model.bands = GetBandsOnGig(model.gig);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(GigInfoViewModel model)
        {
            Gig gigToEdit = GetGigFromId(model.gig.GigId);
            gigToEdit.open = model.gig.open;
            gigToEdit.timeOfGig = model.gig.timeOfGig;
            gigToEdit.venueId = model.gig.venueId;
            gigToEdit.description = model.gig.description;
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult RemoveBandFromGig(int bandId, int gigId)
        {
            BandGig toRemove =  context.BandGigs.Where(g => g.bandId == bandId && g.gigId == gigId).FirstOrDefault();
            context.BandGigs.Remove(toRemove);
            context.SaveChanges();
            return RedirectToAction("RemoveBandsFromLineUp", new { gigId = gigId });
        }

        public ActionResult RemoveBandsFromLineUp(int gigId)
        {
            GigInfoViewModel model = new GigInfoViewModel();
            model.gig = GetGigFromId(gigId);
            model.bands = GetBandsOnGig(model.gig);
            

            return View(model);
        }

        // in edit view have buttons to remove a band from a gig that return the same view but also a description and tim ect

        public async Task<ActionResult> ToggleGigOpen(int gigId)
        {
            Gig gig = GetGigFromId(gigId);
            gig.open = !gig.open;
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");


        }

        public Request GetRequestFromId(int id)
        {
            Request request = context.Requests.Where(r => r.RequestId == id).FirstOrDefault();
            return request;
        }

        public Gig GetGigFromId(int id)
        {
            Gig gig = context.Gigs.Where(r => r.GigId == id).FirstOrDefault();
            return gig;
        }

      

    
        public List<Gig> GetOpenGigs(Venue venue)
        {
            List<Gig> venueGigs = new List<Gig>();
            List<Gig> allGigs = context.Gigs
                .Include("Venue").Where(g => g.venueId == venue.VenueId && g.open == true && g.timeOfGig > DateTime.Now).OrderBy(o => o.timeOfGig).ToList();
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

        public Venue GetUserVenue()
        {
            string userId = User.Identity.GetUserId();
            Venue venue = context.Venues
                .Include("Location").Where(b => b.ApplicationId == userId).FirstOrDefault();
            return venue;
        }

        public Band GetBandFromId(int bandId)
        {
            Band band = context.Bands.Where(b => b.BandId == bandId).FirstOrDefault();
            return band;
        }

        public List<GigInfoViewModel> GetAllGigs()
        {
            
            List<Gig> gigs = context.Gigs.Include("Venue").OrderBy(g => g.timeOfGig).ToList();
            List<GigInfoViewModel> gigList = GetGigViewModel(gigs);


            return gigList;
        }


        public Gig GetGigById(int id)
        {
            Gig gig = context.Gigs.Include("Venue").Where(g => g.GigId == id).FirstOrDefault();
            return gig;
        }

     

        public void DeleteRequests(int gigId)
        {
            List<Request> requestsToDelete = context.Requests.Where(r => r.eventId == gigId).ToList();
            foreach(Request request in requestsToDelete)
            {
                context.Requests.Remove(request);
            }
           context.SaveChanges();
        }

        public List<Request> GetRequestsForGig(int id)
        {
            List<Request> requests = context.Requests.Where(r => r.eventId == id).ToList();
            return requests;
        }



        public void MessageAllBandsOnGig(List<Band> bands, int venueId, string content)
        {
            foreach (Band band in bands)
            {
                    Message temp = new Message();
                    temp.from = "Venue";
                    temp.read = false;
                    temp.bandId = band.BandId;
                    temp.messageContent = content;
                    temp.timeStamp = DateTime.Now;
                    temp.venueId = venueId;
                    context.Messages.Add(temp);
            }
            context.SaveChanges();

        }

    }
}