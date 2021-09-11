using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Domain
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string CityName { get; set; }
        public int? Zipcode { get; set; }
        public long? Population { get; set; }
        //public virtual IEnumerable<User> Users { get; set; }
    }
}
