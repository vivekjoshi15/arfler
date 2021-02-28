using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arfler.Models.AccountViewModels;
using Arfler.Models;

namespace Arfler.Controllers
{
    [Produces("application/json")]
    [Route("api/UserDetails")]
    public class UserDetailsController : Controller
    {
        private readonly ArflerDBContext _context;

        public UserDetailsController(ArflerDBContext context)
        {
            _context = context;
        }

    
        // GET: api/UserDetails
        [HttpGet]
        public IEnumerable<UserDetail> GetUserDetail()
        {
            return _context.UserDetail;
        }



        // GET: api/UserDetails/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDetail = await _context.UserDetail.SingleOrDefaultAsync(m => m.UserId == id);

            if (userDetail == null)
            {
                return NotFound();
            }

            return Ok(userDetail);
        }

        // PUT: api/UserDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetail([FromRoute] long id, [FromBody] UserDetail userDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDetail.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailExists(id))
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

        // POST: api/UserDetails
        [HttpPost]
        public async Task<IActionResult> PostUserDetail([FromBody] UserDetail userDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UserDetail.Add(userDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDetail", new { id = userDetail.UserId }, userDetail);
        }

        // DELETE: api/UserDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetail([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userDetail = await _context.UserDetail.SingleOrDefaultAsync(m => m.UserId == id);
            if (userDetail == null)
            {
                return NotFound();
            }

            _context.UserDetail.Remove(userDetail);
            await _context.SaveChangesAsync();

            return Ok(userDetail);
        }

        private bool UserDetailExists(long id)
        {
            return _context.UserDetail.Any(e => e.UserId == id);
        }
    }
}