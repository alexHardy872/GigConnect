using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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


        // GET: Request
        public ActionResult Index()
        {
            return View();
        }
    }
}