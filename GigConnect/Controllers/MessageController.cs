using GigConnect.Models;
using GigConnect.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GigConnect.Controllers
{
    public class MessageController : Controller
    {

        ApplicationDbContext context;
        public MessageController()
        {
            context = new ApplicationDbContext();
        }


        // GET: Review/Create
        public ActionResult Create(int recipientId, bool isBand)
        {
            if (isBand == true)
            {
                MessageViewModel model = new MessageViewModel();
                model.message = new Message();
                model.message.timeStamp = DateTime.Now;
                model.message.from = GetSenderType();
                model.message.read = false;
                model.recipientId = recipientId;
                model.recipientType = "Band";
                return View(model);
            }
            else
            {
                MessageViewModel model = new MessageViewModel();
                model.message = new Message();
                model.message.timeStamp = DateTime.Now;
                model.message.from = GetSenderType();
                model.message.read = false;
                model.recipientId = recipientId;
                model.recipientType = "Venue";
                return View(model);
            }
        }


       


        // POST: Review/Create
        [HttpPost]
        public async Task<ActionResult> Create(MessageViewModel model)
        {
            try
            {
                if (model.message.from == "Band")
                {
                   model.message.bandId = GetUserBandId();
                   model.message.venueId = model.recipientId;
                    context.Messages.Add(model.message);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index","Band");
                }
                else 
                {
                    model.message.venueId = GetUserVenueId();
                   model.message.bandId = model.recipientId;
                    context.Messages.Add(model.message);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index", "Venue");

                }


            }
            catch (Exception e)
            {
                return View("Index","Home");
            }
        }

       


            // read

        public async Task<ActionResult> ToggleRead(int messageId)
        {
            Message message = context.Messages.Where(m => m.MessageId == messageId).FirstOrDefault();
            message.read = !message.read;
            await context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
      

        ////// helpers
        ///

        public string GetSenderType()
        {
            if(User.IsInRole("Band"))
            {
                return "Band";
            }
            else
            {
                return "Venue";
            }
        }

        public int GetUserBandId()
        {
            string userId = User.Identity.GetUserId();
            Band band = context.Bands.Where(b => b.ApplicationId == userId).FirstOrDefault();
            return band.BandId;

        }

        public int GetUserVenueId()
        {
            string userId = User.Identity.GetUserId();
            Venue venue = context.Venues.Where(b => b.ApplicationId == userId).FirstOrDefault();
            return venue.VenueId;
        }




    }
}