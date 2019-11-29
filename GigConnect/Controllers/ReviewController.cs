using GigConnect.Models;
using GigConnect.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GigConnect.Controllers
{
    public class ReviewController : Controller
    {

        ApplicationDbContext context;
        public ReviewController()
        {
            context = new ApplicationDbContext();
        }
    




  

        // GET: Review/Create
        public ActionResult Create(int reviewedId, bool isBand)
        {
            if(isBand == true)
            {
                ReviewViewModel model = new ReviewViewModel();
                model.review = new Review();
                model.review.timeStamp = DateTime.Now;
                model.reviewedId = reviewedId;
                model.reviewOf = "Band";
                return View(model);
            }
            else
            {
                ReviewViewModel model = new ReviewViewModel();
                model.review = new Review();
                model.review.timeStamp = DateTime.Now;
                model.reviewedId = reviewedId;
                model.reviewOf = "Venue";
                return View(model);
            }   
        }


        // POST: Review/Create
        [HttpPost]
        public async Task<ActionResult> Create(ReviewViewModel model)
        {
            try
            {
                context.Reviews.Add(model.review);

                if (model.reviewOf == "Band")
                {
                    BandReview junction = new BandReview();
                    junction.reviewId = model.review.ReviewId;
                    junction.bandId = model.reviewedId;
                    context.BandReviews.Add(junction);
                }
                if (model.reviewOf == "Venue")
                {
                    VenueReview junction = new VenueReview();
                    junction.reviewId = model.review.ReviewId;
                    junction.venueId = model.reviewedId;
                    context.VenueReviews.Add(junction);
                }
                try
                {
                    await context.SaveChangesAsync();
                }
                catch(Exception e)
                {

                }
                
                return RedirectToAction("Index","Home");
            }
            catch(Exception e)
            {
                return View("Index","Home");
            }
        }

    

       
        

       
    }
}
