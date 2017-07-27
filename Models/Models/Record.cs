using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YC.Models
{
    public class Record
    {

        public int ID { get; set; }
        public string StationID { get; set; }
        public double WaterLevel { get; set; }
        public DateTime RecordTime { get; set; }
        public DateTime CreateTime { get; set; }

        public  Station Station { get; set; }
    }
}
