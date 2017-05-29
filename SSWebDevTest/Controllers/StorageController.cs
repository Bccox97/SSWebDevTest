using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSWebDevTest.Models;

namespace SSWebDevTest.Controllers
{
    public class StorageController : Controller
    {
        // GET: Storage
        UnitFunctions unitFunctions = new UnitFunctions();

        public ActionResult Dean()
        {
            return View(unitFunctions);
        }
        public ActionResult King()
        {
            return View(unitFunctions);
        }
        public ActionResult Rogers()
        {
            return View(unitFunctions);
        }
        public ActionResult Locations()
        {
            return View(unitFunctions);
        }
        public ActionResult Waitlist_Rent_Reserve()
        {
            return View(unitFunctions);
        }
    }
}