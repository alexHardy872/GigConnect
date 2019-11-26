using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GigConnect.Controllers
{
    public class GigController : Controller
    {
        ApplicationDbContext context;
        public GigController()
        {
            context = new ApplicationDbContext();
        }



        // venue creates a gig
        // adds band to gig in create or edit OR automethode AddBandToGig(int bandId) (adds to junction table!)

            // creates open gig?

            // gig created when venue accepts request?








        // GET: Gig
        public ActionResult Index()
        {

            return View();
        }


        public ActionResult Create()
        {
            // maybe use gig view model? seperate list of bands on gig? able to edit/ remove ect?
            Gig gig = new Gig();

            return View(gig);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Gig gig)
        {

            context.Gigs.Add(gig);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> CreateGigFromRequest(int requestId)
        {
            Gig gig = new Gig();
            Request request = GetRequestFromId(requestId);
            gig.timeOfGig = request.gigTime;
            gig.venueId = request.venueId;
            gig.open = true;
            context.Gigs.Add(gig);
            await AddBandToGig(request.bandId, gig.GigId);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

      


        public async Task<ActionResult> AddBandToGig(int bandId, int gigId)
        {
            BandGig junction = new BandGig();
            junction.gigId = gigId;
            junction.bandId = bandId;

            context.BandGigs.Add(junction);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> RemoveBandFromGig(int bandId, int gigId)
        {
            BandGig toRemove = context.BandGigs.Where(g => g.bandId == bandId && g.gigId == gigId).FirstOrDefault();
            context.BandGigs.Remove(toRemove);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

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

    }
}