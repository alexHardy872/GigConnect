using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GigConnect.Controllers
{
    public class RequestController : Controller
    {

        ApplicationDbContext context;
        public RequestController()
        {
            context = new ApplicationDbContext();
        }


        // create request
            // request going which way?
            // request attached to a gig?
            // ect ect 
            // request denied change bool
            // request accepted change bool????

        public ActionResult Create(int bandId, int venueId, int? gigId)
        {
            Request request = new Request();
            request.eventId = gigId;
            request.venueId = venueId;
            request.bandId = bandId;
            request.timeStamp = DateTime.Now;
            request.approved = false;
            request.denied = false;
            request.fromBand = IsUserInBand();
            request.fromVenue = !request.fromBand;

            return View(request);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Request request)
        {
            if(request.gigTime == null)
            {
                request.gigTime = DateTime.Now;
            }
            context.Requests.Add(request);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
    
        }

        public async Task<ActionResult> ApproveRequest(int requestId)
        {
            
            Request request = context.Requests.Where(r => r.RequestId == requestId).FirstOrDefault();
            if(request.eventId == null)
            {
                // create gig
                return RedirectToAction("CreateGigFromRequest", "Gig", new { requestId = requestId });
            }
            request.approved = true;
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> DenyRequest(int requestId)
        {
            Request request = context.Requests.Where(r => r.RequestId == requestId).FirstOrDefault();
            request.denied = true;
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }


        public bool IsUserInBand()
        {
            if (User.IsInRole("Band"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      

    }
}