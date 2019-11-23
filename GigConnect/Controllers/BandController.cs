using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GigConnect.Models.ViewModels;
using System.Threading.Tasks;
using GigConnect.Services;

namespace GigConnect.Controllers
{
   // [Authorize(Roles = "Band")]
    public class BandController : Controller
    {

        public ApplicationDbContext context;
        public BandController()
        {
            context = new ApplicationDbContext();
        }

        // create profile

        // view profile





        // GET: Band
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            Band band = context.Bands.Where(b => b.ApplicationId == userId).FirstOrDefault();
            if(band == null)
            {
                return RedirectToAction("Create", "Band");
            }

            BandIndexViewModel model = AssembleIndexViewModelForBand();
            return View(model);
        }

        public BandIndexViewModel AssembleIndexViewModelForBand()
        {
            BandIndexViewModel bandInfo = new BandIndexViewModel();
 
            bandInfo.band = GetUserBand();
            bandInfo.gigs = GetGigs(bandInfo.band);
            bandInfo.messagesIn = GetAllMessagesIn(bandInfo.band.BandId);
            bandInfo.messagesOut = GetAllMessagesOut(bandInfo.band.BandId);

            bandInfo.requestsIn = GetRequestsIn(bandInfo.band.BandId);
            bandInfo.requestsOut = GetRequestsOut(bandInfo.band.BandId);
            bandInfo.unreadMessages = FilterForUnread(bandInfo.messagesIn);

            return bandInfo;
        }

    public Band GetUserBand()
        {
            string userId = User.Identity.GetUserId();
            Band band = context.Bands.Where(b => b.ApplicationId == userId).FirstOrDefault();
            return band;
        }

    public List<Gig> GetGigs(Band band)
        {
            List<Gig> bandGigs = new List<Gig>();
            List<Gig> allGigs = context.Gigs.ToList();
            foreach( Gig gig in allGigs)
            {
                if (IsBandOnGig(gig.bandsOnVenue, band.BandId) == true)
                {
                    bandGigs.Add(gig);
                }
            }
            return bandGigs;
        }
    public bool IsBandOnGig(string bandsOnVenue, int bandId)
    {   
        List<int> bandIds = bandsOnVenue.Split(',').Select(int.Parse).ToList();
         if (bandIds.Contains(bandId))
            {
                return true;
            }
            else
            {
                return false;
            }
    }

   


        public List<Message> GetAllMessagesIn(int bandId)
        {
            List<Message> messages = context.Messages.Where(m => m.bandId == bandId && m.from == "Venue").ToList();           
            return messages;
        }

        public bool FilterForUnread(List<Message> messages)
        {
            var unread = messages.Where(u => u.read == false).ToList();
            if(unread.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Message> GetAllMessagesOut(int bandId)
        {
            List<Message> messages = context.Messages.Where(m => m.bandId == bandId && m.from == "Band").ToList();
            return messages;
        }

        public List<Request> GetRequestsIn(int bandId)
        {
            List<Request> requestsIn = context.Requests.Where(r => r.bandId == bandId && r.fromVenue == true && r.approved == false && r.denied == false).ToList();
            return requestsIn;
        }
        public List<Request> GetRequestsOut(int bandId)
        {
            List<Request> requestsIn = context.Requests.Where(r => r.bandId == bandId && r.fromVenue == true && r.approved == false && r.denied == false).ToList();
            return requestsIn;
        }

        // GET: Band/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Band/Create
        public ActionResult Create()
        {
            CreateAndEditViewModel createView = new CreateAndEditViewModel();

            Location location = new Location();
            createView.Location = location;
            Band band = new Band();
            createView.Band = band;
            SocialMediaIds social = new SocialMediaIds();
            createView.Social = social;

            return View(createView);
        }

        // POST: Band/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateAndEditViewModel createInfo)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                createInfo.Band.ApplicationId = userId;
                int nextLocationId = context.Database.ExecuteSqlCommand("SELECT IDENT_CURRENT('dbo.Locations')") + 1;
                int nextSocialId = context.Database.ExecuteSqlCommand("SELECT IDENT_CURRENT('dbo.Socials')") + 1;
                createInfo.Band.LocationId = nextLocationId;
                createInfo.Band.socialId = nextSocialId;

                context.Socials.Add(createInfo.Social);

                context.Bands.Add(createInfo.Band);

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

        // GET: Band/Edit/5
        public ActionResult Edit(int id)
        {
            CreateAndEditViewModel toEdit = new CreateAndEditViewModel();
            string userId = User.Identity.GetUserId();
            toEdit.Band = context.Bands.Where(b => b.ApplicationId == userId).FirstOrDefault();
            toEdit.Location = context.Locations.Where(l => l.LocationId == toEdit.Band.LocationId).FirstOrDefault();
            toEdit.Social = context.Socials.Where(s => s.SocialId == toEdit.Band.socialId).FirstOrDefault();
            
            return View(toEdit);
        }

        // POST: Band/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(CreateAndEditViewModel input)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                Band currentBand = context.Bands.Where(b => b.ApplicationId == userId).FirstOrDefault();
                Location currentLocation = context.Locations.Where(l => l.LocationId == currentBand.LocationId).FirstOrDefault();
                SocialMediaIds currentSocials = context.Socials.Where(s => s.SocialId == currentBand.socialId).FirstOrDefault();

                currentBand.BandId = input.Band.BandId;
                currentBand.bandName = input.Band.bandName;
                currentBand.genre = input.Band.genre;
                currentBand.LocationId = input.Band.LocationId;
                currentBand.ApplicationId = input.Band.ApplicationId;

                currentLocation.LocationId = input.Location.LocationId;
                currentLocation.address1 = input.Location.address1;
                currentLocation.address2 = input.Location.address2;
                currentLocation.city = input.Location.city;
                currentLocation.zip = input.Location.zip;
                currentLocation.state = input.Location.state;

                currentSocials.SocialId = input.Social.SocialId;
                currentSocials.facebookPageId = input.Social.facebookPageId;
                currentSocials.twitterHandle = input.Social.twitterHandle;
                currentSocials.youtubeChannelId = input.Social.youtubeChannelId;

                string[] latLng = await GeoCode.GetLatLongFromApi(currentLocation);
                currentLocation.lat = latLng[0];
                currentLocation.lng = latLng[1];
                
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return View();
            }
        }

        // GET: Band/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Band/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
