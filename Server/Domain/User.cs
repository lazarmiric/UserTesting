using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Domain
{
    public class User
    {
        public int UserID { get; set; }
        public string SocialIdentityNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string EyeCollor { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int? PlaceId { get; set; }
        public Place Place { get; set; }
    }


}
