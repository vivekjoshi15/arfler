using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Arfler.Models;
using Arfler.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Arfler.Controllers
{
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        ArflerDBContext _context; private readonly IEmailSender _emailSender;
        public ContactController(ArflerDBContext context, IEmailSender emailSender)
        {
            _context = context; _emailSender = emailSender;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] ContactDetails _contacDetails)
        {
            ContactDetails contDetails = new ContactDetails();
            contDetails = _contacDetails;
            contDetails.createdDate = DateTime.Now;
            _context.ContactDetails.Add(contDetails);
            _context.SaveChanges();
            string msg = string.Empty;
            string subj = string.Empty;
            subj = contDetails.cSubject;
            msg += "A new message is recieved from " + contDetails.contactName + "<br/>";
            msg += contDetails.contactMessage;
            _emailSender.SendEmailAsync(contDetails.contactEmail, subj, msg);

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
