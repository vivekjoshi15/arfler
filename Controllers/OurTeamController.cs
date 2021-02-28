using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arfler.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Arfler.Controllers
{
    [Route("api/[controller]")]
    public class OurTeamController : Controller
    {
        private readonly ArflerDBContext _context;

        public OurTeamController(ArflerDBContext context)
        {
            _context = context;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // Get: api/values/2/detail
        [HttpGet("{id}")]
        [Route("{id:int}/detail")]
        public PeopleDetail Get(int id)
        {
            var imgs = _context.PeopleDetail.Where(a => a.id == id).FirstOrDefault();
            return imgs;
        }

        // Get: api/values/2/restaurant
        [HttpGet("{id}")]
        [Route("{id:int}/restaurant")]
        public IEnumerable<PeopleDetail> Get(long id)
        {
            var imgs = _context.PeopleDetail.Where(a => a.restaurantId == id).ToList();
            return imgs;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
