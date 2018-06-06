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
            return View(model.GetStudents());
        }
    }
}