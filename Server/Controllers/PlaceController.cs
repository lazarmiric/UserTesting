using Microsoft.AspNetCore.Mvc;
using Server.ClientBuisnessLogicLayer;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService service;

        public PlaceController(IPlaceService service)
        {
            this.service = service;
        }


        [HttpGet]
        public IEnumerable<Place> Get()
        {
            return service.SelectAll();
        }

        [HttpGet("{id}")]
        public Place Get(int id)
        {
            return service.SelectById(id);
        }

        
        [HttpPost]
        public void Post([FromBody] Place place)
        {
            service.Insert(place);
        }


        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Place place)
        {
            place.PlaceId = id;
            service.Update(place);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Place place = service.SelectById(id);
            service.Delete(place);
        }
    }
}
