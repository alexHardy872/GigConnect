using GigConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GigConnect.Models.ViewModels;

namespace GigConnect.Controllers
{
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

            return View(createView);
        }

        // POST: Band/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string userId = User.Identity.GetUserId();

                // band

                // location info

                // save changes

                // redirect to main dashboard (create profile???)
            

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Band/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Band/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
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
