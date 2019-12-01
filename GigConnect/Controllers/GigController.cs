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

        public async Task<ActionResult> AddBandFromRequest(int requestId)
        {
            Request request = GetRequestFromId(requestId);
            int gigId = request.eventId ?? default(int);
           await Task.Run(() => AddBandToGig(request.bandId, gigId));

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

      
        
        public async void AddBandToGig(int bandId, int gigId)
        {
            BandGig junction = new BandGig();
            junction.gigId = gigId;
            junction.bandId = bandId;

            context.BandGigs.Add(junction);
            await context.SaveChangesAsync();
            
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
            return View("Index", "Home");
        }

        public async Task<ActionResult> RemoveBandFromGig(int bandId, int gigId)
        {
            BandGig toRemove = context.BandGigs.Where(g => g.bandId == bandId && g.gigId == gigId).FirstOrDefault();
            context.BandGigs.Remove(toRemove);
            await context.SaveChangesAsync();
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

        public ActionResult ToggleGigOpen(int gigId)
        {
            Gig gig = GetGigFromId(gigId);
            gig.open = !gig.open;
            return RedirectToAction("Home", "Index");


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
            
            List<Gig> gigs = context.Gigs.Include("Venue").ToList();
            List<GigInfoViewModel> gigList = GetGigViewModel(gigs);


            return gigList;
        }

    }
}