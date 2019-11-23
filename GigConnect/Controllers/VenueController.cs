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
            string userId = User.Identity.GetUserId();
            Venue venue = context.Venues.Where(b => b.ApplicationId == userId).FirstOrDefault();
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
            string userId = User.Identity.GetUserId();
            toEdit.Venue = context.Venues.Where(b => b.ApplicationId == userId).FirstOrDefault();
            toEdit.Location = context.Locations.Where(l => l.LocationId == toEdit.Venue.LocationId).FirstOrDefault();

            return View(toEdit);
        }

        // POST: Venue/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(CreateAndEditViewModel input)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                Venue currentVenue = context.Venues.Where(b => b.ApplicationId == userId).FirstOrDefault();
                Location currentLocation = context.Locations.Where(l => l.LocationId == currentVenue.LocationId).FirstOrDefault();

                currentVenue.VenueId = input.Venue.VenueId;
                currentVenue.venueName = input.Venue.venueName;
                currentVenue.genre = input.Venue.genre;
                currentVenue.websiteUrl = input.Venue.websiteUrl;

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

        // GET: Venue/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Venue/Delete/5
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
