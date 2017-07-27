using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class StationController : Controller
    {
        public YC.Repository.StationRepository stationDb = new YC.Repository.StationRepository();

        // GET: Station
        public ActionResult Index(string search = "")
        {
            
            var stationRepository = new YC.Repository.StationRepository();

            var stations = stationRepository.FindAllStations();
            if (!string.IsNullOrEmpty(search))
                stations = stations
                    .Where(x =>
                    x.ObservatoryName.Contains(search) ||
                    x.LocationAddress.Contains(search))
                    .ToList();
            ViewBag.Search = search;
            //ViewBag.Stations = stations;

            return View(stations);
        }

        [HttpGet]
        public ActionResult Update(string id)
        {
            YC.Models.Station model;
            if (string.IsNullOrEmpty(id))
                model = new YC.Models.Station();
            else
                model = stationDb.FindByID(id);
            

            return View(model);
        }
        [HttpPost]
        public ActionResult Update(YC.Models.Station station)
        {
            stationDb.Update(station);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(YC.Models.Station station)
        {
            return View();
        }


    }
}