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
    [Authorize(Roles = "Band")]
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
            return View();
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

            

            createView.Location = new Location();
            createView.Band = new Band();
            createView.Social = new SocialMediaIds();

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
