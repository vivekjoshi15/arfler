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
    [Route("api/MenuCategoryDetails")]
    public class MenuCategoryDetailsController : Controller
    {
        private readonly ArflerDBContext _context;

        public MenuCategoryDetailsController(ArflerDBContext context)
        {
            _context = context;
        }

        // GET: api/MenuCategoryDetails
        [HttpGet]
        public IEnumerable<MenuCategoryDetail> GetMenuCategoryDetail(string sortorder = "asc")
        {
            if (sortorder == "desc")
                return _context.MenuCategoryDetail.OrderByDescending(c => c.sortOrder);
            else
            {
                var MenuCategoryDetails= _context.MenuCategoryDetail;
                return MenuCategoryDetails;
            }
        }

        // GET: api/MenuCategoryDetails/:id/detail
        [Route("{id:int}/detail")]
        public async Task<IActionResult> GetMenuCategoryDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuCategoryDetail = await _context.MenuCategoryDetail.SingleOrDefaultAsync(m => m.menuCategoryId == id);

            if (menuCategoryDetail == null)
            {
                return NotFound();
            }

            return Ok(menuCategoryDetail);
        }

        // GET: api/MenuCategoryDetails/:id/resturant
        [Route("{id:int}/resturant")]
        public async Task<IActionResult> GetResturantMenuCategories([FromRoute] long id)
        {
            var menuCategories =  _context.MenuCategoryDetail.Where(m => m.restaurantId == id &&  m.parentMenuCategoryId == null ).ToList();

            if (menuCategories == null)
            {
                return NotFound();
            }

            return Ok(menuCategories);
        }

        // PUT: api/MenuCategoryDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryDetail([FromRoute] long id, [FromBody] MenuCategoryDetail menuCategoryDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != menuCategoryDetail.menuCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(menuCategoryDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuCategoryDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MenuCategoryDetails
        [HttpPost]
        public async Task<IActionResult> PostCategoryDetail([FromBody] string menuCategoryDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           // _context.MenuCategoryDetail.Add(menuCategoryDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenuCategoryDetail", new { id = 1 }, menuCategoryDetail);// id = menuCategoryDetail.menuCategoryId}, menuCategoryDetail);
        }

        // DELETE: api/MenuCategoryDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var menuCategoryDetail = await _context.MenuCategoryDetail.SingleOrDefaultAsync(m => m.menuCategoryId == id);
            if (menuCategoryDetail == null)
            {
                return NotFound();
            }

            _context.MenuCategoryDetail.Remove(menuCategoryDetail);
            await _context.SaveChangesAsync();

            return Ok(menuCategoryDetail);
        }

        private bool MenuCategoryDetailExists(long id)
        {
            return _context.MenuCategoryDetail.Any(e => e.menuCategoryId == id);
        }
    }
}