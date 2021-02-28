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
    [Route("api/CategoryDetails")]
    public class CategoryDetailsController : Controller
    {
        private readonly ArflerDBContext _context;

        public CategoryDetailsController(ArflerDBContext context)
        {
            _context = context;
        }

        // GET: api/CategoryDetails
        [HttpGet]
        public IEnumerable<CategoryDetail> GetCategoryDetail(string sortorder = "asc")
        {
            if (sortorder == "desc")
                return _context.CategoryDetail.OrderByDescending(c => c.SortOrder);
            else
                return _context.CategoryDetail;
        }

        // GET: api/CategoryDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryDetail = await _context.CategoryDetail.SingleOrDefaultAsync(m => m.CategoryId == id);

            if (categoryDetail == null)
            {
                return NotFound();
            }

            return Ok(categoryDetail);
        }

        // PUT: api/CategoryDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoryDetail([FromRoute] long id, [FromBody] CategoryDetail categoryDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != categoryDetail.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(categoryDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryDetailExists(id))
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

        // POST: api/CategoryDetails
        [HttpPost]
        public async Task<IActionResult> PostCategoryDetail([FromBody] CategoryDetail categoryDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CategoryDetail.Add(categoryDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoryDetail", new { id = categoryDetail.CategoryId }, categoryDetail);
        }

        // DELETE: api/CategoryDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryDetail = await _context.CategoryDetail.SingleOrDefaultAsync(m => m.CategoryId == id);
            if (categoryDetail == null)
            {
                return NotFound();
            }

            _context.CategoryDetail.Remove(categoryDetail);
            await _context.SaveChangesAsync();

            return Ok(categoryDetail);
        }

        private bool CategoryDetailExists(long id)
        {
            return _context.CategoryDetail.Any(e => e.CategoryId == id);
        }
    }
}