using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Create(Gig gig)
        {

            context.Gigs.Add(gig);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}