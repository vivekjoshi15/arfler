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
    [Route("api/RoleDetails")]
    public class RoleDetailsController : Controller
    {
        private readonly ArflerDBContext _context;

        public RoleDetailsController(ArflerDBContext context)
        {
            _context = context;
        }

        // GET: api/RoleDetails
        [HttpGet]
        public IEnumerable<RoleDetail> GetRoleDetail()
        {
            DbSet<RoleDetail> roles = _context.RoleDetail;
            return roles;
        }

        // GET: api/RoleDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleDetail = await _context.RoleDetail.SingleOrDefaultAsync(m => m.RoleId == id);

            if (roleDetail == null)
            {
                return NotFound();
            }

            return Ok(roleDetail);
        }

        // PUT: api/RoleDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleDetail([FromRoute] long id, [FromBody] RoleDetail roleDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roleDetail.RoleId)
            {
                return BadRequest();
            }

            _context.Entry(roleDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleDetailExists(id))
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

        // POST: api/RoleDetails
        [HttpPost]
        public async Task<IActionResult> PostRoleDetail([FromBody] RoleDetail roleDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.RoleDetail.Add(roleDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoleDetail", new { id = roleDetail.RoleId }, roleDetail);
        }

        // DELETE: api/RoleDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roleDetail = await _context.RoleDetail.SingleOrDefaultAsync(m => m.RoleId == id);
            if (roleDetail == null)
            {
                return NotFound();
            }

            _context.RoleDetail.Remove(roleDetail);
            await _context.SaveChangesAsync();

            return Ok(roleDetail);
        }

        private bool RoleDetailExists(long id)
        {
            return _context.RoleDetail.Any(e => e.RoleId == id);
        }
    }
}