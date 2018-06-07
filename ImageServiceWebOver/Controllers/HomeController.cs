using GUICommunication.Client;
using ImageServiceWeb.Models;
using Newtonsoft.Json.Linq;
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
        private HomeModel model;
        // GET: Home
        public ActionResult HomeView()
        {
            model = new HomeModel();
            return View(model.GetStudents());
        }
        [HttpGet]
        public JObject GetStatus()
        {
            JObject data = new JObject();
            data["stat"] = model.connected;
            data["num"] = model.numOfImages();
            return data;
        }
        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }
    }
}