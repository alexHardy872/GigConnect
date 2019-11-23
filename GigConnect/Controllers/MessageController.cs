using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET: Message
        public ActionResult Index()
        {
            return View();
        }



    }
}