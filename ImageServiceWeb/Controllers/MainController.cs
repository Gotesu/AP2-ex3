using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using ImageService.Infrastructure;
using ImageServiceWeb.Models;

namespace ImageServiceWeb.Controllers
{
    public class MainController : Controller
    {
		static HomeModel model = new HomeModel();
        static ConfigModel configModel = new ConfigModel();
		static PhotosModel photosModel = new PhotosModel();
		static LogModel logModel = new LogModel();
        static List<Entry> entries = new List<Entry>();
        // GET: First
        public ActionResult Config()
        {
            
            ImageServiceConfig config = configModel.getConfig();

            if (config != null)
            {
                ViewBag.config = config;
                ViewBag.handlers = config.handlers;
                while (!configModel.removed)
                    //waiting for handler removal and to avoid busy waiting we use delay
                    Task.Delay(1000);
            }
            else
            {
                config = new ImageServiceConfig(null, 0, "not available", "not available", "not available");
                ViewBag.config = config;
            }

            return View();
        }

        [HttpGet]
        public ActionResult Home()
        {
            return View(model.GetStudents());
        }

		[HttpGet]
		public ActionResult Photos()
		{
			ViewBag.photos = photosModel.GetPhotoList();
			return View();
		}

		[HttpGet]
		public ActionResult ViewPhoto()
		{
			ViewBag.photos = photosModel.Photos;
			return View();
		}

		[HttpGet]
		public ActionResult DeletePhoto()
		{
			ViewBag.photos = photosModel.Photos;
			return View();
		}

		public void Delete(string photoNum)
		{
			photosModel.DeleteFile(Int32.Parse(photoNum) );
		}

		[HttpGet]
        public JObject GetStatus()
        {
            JObject data = new JObject();
            if(model.connectionStatus())
                data["Status"] = "Active";
            else
                data["Status"] = "Not Active";
            data["Number"] = photosModel.ImagesNumber().ToString();
            return data;
        }

        // GET: First/Details
        public ActionResult Log()
        {
            foreach (EventLogEntry ent in logModel.entries)
            {
                Entry entry = new Entry();
                switch (ent.EntryType.ToString())
                {
                    case "Information":
                        entry.Type = "INFO";
                        break;
                    case "Error":
                        entry.Type = "Error";
                        break;
                    case "Warning":
                        entry.Type =  "WARNING";
                        break;
                    default:
                        //made info the default
                       entry.Type = "INFO";
                        break;
                }

                entry.Message = ent.Message;
                entries.Add(entry);
            }
            return View(entries);
        }

        public ActionResult HandlerRemoval(string path)
        {
            ViewBag.handler = path;
            return View();
        }

        public void Remove(string path)
        {
            configModel.RemoveHandler(path);
        }
    }
}
