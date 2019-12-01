using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigConnect.Controllers
{
    public class BandFilterController : Controller
    {
        // GET: BandFilter



        public ActionResult Index(string filter, string searchTerm, List<Band> bands)
        {
            return View();
        }




    }
}