using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arfler.Models;

namespace Arfler.Controllers
{
    [Produces("application/json")]
    [Route("api/Restaurants")]
    public class RestaurantsController : Controller
    {
        private readonly ArflerDBContext _context;

        public RestaurantsController(ArflerDBContext context)
        {
            _context = context;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<RestaurantDetail> GetRestaurants()
        {
            return _context.RestaurantDetail;
        }

        // GET api/Restaurants/5
        [HttpGet("{id}")]
        [Route("{id:int}/detail")]
        public async Task<IActionResult> GetRestaurants([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var restaurantDetail = await _context.RestaurantDetail.SingleOrDefaultAsync(m => m.id == id);

            if (restaurantDetail == null)
            {
                return NotFound();
            }

            return Ok(restaurantDetail);
        }
        // GET api/Restaurants/4/resturant
        [HttpGet("{id}")]
        [Route("{id:int}/resturant")]
        public  IEnumerable<RestaurantDetail> GetRestaurants([FromRoute] int id)
        {           

            var restaurantDetail =  _context.RestaurantDetail.Where(a=>a.categoryIds.Contains(id.ToString())).ToList();
            
            return restaurantDetail;
        }


        //[HttpGet("{cid}")]
        //public IEnumerable<RestaurantDetail> GetCategoryRestaurants(string cid)
        //{
        //    var _restaurant = _context.RestaurantDetail.ToList();
        //    List<RestaurantDetail> _rest = new List<RestaurantDetail>();
        //    foreach (var rr in _restaurant)
        //    {
        //        string[] aa = rr.categoryIds.Split(';');
        //        for (int i = 0; i < aa.Length; i++)
        //        {
        //            if (aa[i].ToString() == cid)
        //            {
        //                _rest.Add(rr);
        //            }
        //        }
        //    }
        //    var rests = _rest;
        //    return rests;
        //}

        // POST api/values
        [HttpPost]
        public IActionResult PostRestaurants([FromBody]string value)
        {

            return View();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void PutRestaurants(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void DeleteRestaurants(int id)
        {
        }
    }
}
