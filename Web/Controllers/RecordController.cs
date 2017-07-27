using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class RecordController : Controller
    {
        public YC.Repository.RecordRepository recordDb = new YC.Repository.RecordRepository();

        // GET: Record
        public ActionResult Index(string stationID)
        {
            List<YC.Models.Record> model = new List<YC.Models.Record>();
            model = recordDb.FindByStationID(stationID);

            return View(model);
        }
    }
}