using ImageServiceWeb.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult HomeView()
        {
            HomeModel model = new HomeModel();
            IGUICLient client = new IGUICLient();
            return View(model.GetStudents());
        }
        [HttpPost]
        public bool GetStatus()
        {

        }
    }
}