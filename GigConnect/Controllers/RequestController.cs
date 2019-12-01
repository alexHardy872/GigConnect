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


   

        public ActionResult Create(int bandId, int venueId, int? gigId)
        {
            Request request = new Request();
            request.eventId = gigId;
            if(gigId != null)
            {
                int gigId2 = gigId ?? default(int);
                Gig gig = GetGigById(gigId2);
                request.message = "Request to join existing gig on " + gig.timeOfGig.ToShortDateString();
            }
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
                request.gigTime = DateTime.Now; // placeholder to keep from null excepton
            }
            context.Requests.Add(request);
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
    
        }

        public async Task<ActionResult> ApproveRequest(int requestId)
        {
            Request request = GetRequestById(requestId);
           
            request.approved = true;
            await context.SaveChangesAsync();
            if (request.eventId == null)
            {
                // create gig
                return RedirectToAction("CreateGigFromRequest", "Gig", new { requestId = requestId });
            }
            else
            {
                return RedirectToAction("AddBandFromRequest", "Gig", new { requestId = requestId });
            }
            
        }

        public async Task<ActionResult> DenyRequest(int requestId)
        {
            Request request = GetRequestById(requestId);
            request.denied = true;
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Delete(int requestId)
        {
            Request request = GetRequestById(requestId);
            context.Requests.Remove(request);
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



        public Request GetRequestById(int id)
        {
            Request request = context.Requests.Where(r => r.RequestId == id).FirstOrDefault();
            return request;
        }

        public Gig GetGigById(int id)
        {
            Gig gig = context.Gigs.Where(g => g.GigId == id).FirstOrDefault();
            return gig;
        }
      

    }
}