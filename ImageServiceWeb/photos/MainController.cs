using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageService.Infrastructure;
using ImageServiceWeb.Models;

namespace ImageServiceWeb.Controllers
{
    public class MainController : Controller
    {
        static HomeModel model = new HomeModel();
        static ConfigModel configModel = new ConfigModel();
		static PhotosModel photosModel = new PhotosModel(@"C:\Users\שירה רקובסקי\Desktop\OPD\OutputDir\Thumbnails");
		static LogModel logModel = new LogModel();
        static List<Employee> employees = new List<Employee>()
        {
          new Employee  { FirstName = "Moshe", LastName = "Aron", Email = "Stam@stam", Salary = 10000, Phone = "08-8888888" },
          new Employee  { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 2000, Phone = "08-8888888" },
          new Employee   { FirstName = "Mor", LastName = "Sinai", Email = "Stam@stam", Salary = 500, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 20, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 700, Phone = "08-8888888" }
        };
        static List<Entry> entries = new List<Entry>();
        static List<Entry> requestedEntries = new List<Entry>();
        // GET: First
        public ActionResult Config()
        {
            
            ImageServiceConfig config = configModel.getConfig();
            ViewBag.config = config;
            ViewBag.handlers = config.handlers;
            return View();
        }

		[HttpGet]
		public ActionResult Photos()
		{
			ViewBag.photos = photosModel.photos;
			return View();
		}

		[HttpGet]
        public ActionResult Home()
        {
            return View(model.GetStudents());
        }

        [HttpGet]
        public JObject GetStatus()
        {
            JObject data = new JObject();
            if(model.connectionStatus())
                data["Status"] = "Active";
            else
                data["Status"] = "Not Active";
            data["Number"] = model.numOfImages().ToString();
            return data;
        }

        [HttpPost]
        public JObject GetEmployee(string name, int salary)
        {
            foreach (var empl in employees)
            {
                if (empl.Salary > salary || name.Equals(name))
                {
                    JObject data = new JObject();
                    data["FirstName"] = empl.FirstName;
                    data["LastName"] = empl.LastName;
                    data["Salary"] = empl.Salary;
                    return data;
                }
            }
            return null;
        }

        // GET: First/Details
        public ActionResult Details()
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

        public ActionResult reqDetails(string type)
        {
            requestedEntries.Clear();
            foreach (Entry ent in entries)
            {
                requestedEntries.Add(ent);
            }
            return View(requestedEntries);
        }

        // GET: First/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: First/Create
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            try
            {
                employees.Add(emp);

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        // GET: First/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (Employee emp in employees) {
                if (emp.ID.Equals(id)) { 
                    return View(emp);
                }
            }
            return View("Error");
        }

        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee empT)
        {
            try
            {
                foreach (Employee emp in employees)
                {
                    if (emp.ID.Equals(id))
                    {
                        emp.copy(empT);
                        return RedirectToAction("Config");
                    }
                }

                return RedirectToAction("Config");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Employee emp in employees)
            {
                if (emp.ID.Equals(id))
                {
                    employees.RemoveAt(i);
                    return RedirectToAction("Details");
                }
                i++;
            }
            return RedirectToAction("Error");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}
