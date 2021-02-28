using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arfler.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Arfler.Controllers
{
    [Route("api/MenuItem")]
    public class MenuItemController : Controller
    {
        private readonly ArflerDBContext _context;

        public MenuItemController(ArflerDBContext context)
        {
            _context = context;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> MenuItem()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public JsonResult MenuItem(int id)
        {           
            List<MenuCategoryDetail> menucatList = new List<MenuCategoryDetail>();
            var menuItems = _context.MenuCategoryDetail.Where(a => a.parentMenuCategoryId == id || a.parentMenuCategoryId == null && a.menuCategoryId==id).ToList();
            foreach(var mi in menuItems)
            {
                mi.MenuItems = _context.MenuItemDetail.Where(b => b.menuCategoryId == mi.menuCategoryId).ToList();
                menucatList.Add(mi);
            }

            return Json(menucatList);
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
