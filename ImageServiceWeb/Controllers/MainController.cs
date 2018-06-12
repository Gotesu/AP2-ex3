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
        //home page model
		static HomeModel model = new HomeModel();
        //config page model
        static ConfigModel configModel = new ConfigModel();
        //photos page model
		static PhotosModel photosModel = new PhotosModel();
		//log page model
		static LogModel logModel = new LogModel();
        //our log entries
        static List<Entry> entries = new List<Entry>();
        // GET: Config
        public ActionResult Config()
        {
            //this is the configuration page control method
            ImageServiceConfig config = configModel.getConfig();

            if (config != null)
            {
                //case we got a configuration
                ViewBag.config = config;
                ViewBag.handlers = config.handlers;
                while (!configModel.removed)
                    //waiting for handler removal and to avoid busy waiting we use delay
                    Task.Delay(1000);
            }
            else
            {
                //if we cannot get our configuration we show blank results
                config = new ImageServiceConfig(null, 0, "not available", "not available", "not available");
                ViewBag.config = config;
            }

            return View();
        }
        //this one is our Home page which reads the students info from appdata xml file
        [HttpGet]
        public ActionResult Home()
        {
            return View(model.GetStudents());
        }
        //photos page control which reads from photo model
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
        /// <summary>
        /// get status is the method which gets us the status and the number of images using json
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// log action results gives us the log page but first transforms log list to show entries in the
        /// wanted format
        /// </summary>
        /// <returns></returns>
        public ActionResult Log()
        {
            //uses entries from log model
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
        /// <summary>
        /// page for handler removal
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ActionResult HandlerRemoval(string path)
        {
            ViewBag.handler = path;
            return View();
        }
        /// <summary>
        /// method to remove handler
        /// </summary>
        /// <param name="path"></param>
        public void Remove(string path)
        {
            configModel.RemoveHandler(path);
        }
    }
}
