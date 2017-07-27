using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YC.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Station> Stations { get; set; }


    }
}
