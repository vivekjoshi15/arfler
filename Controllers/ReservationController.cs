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
    public class ReservationController : Controller
    {
        ArflerDBContext _context;
        private readonly IEmailSender _emailSender;
        public ReservationController(ArflerDBContext context, IEmailSender emailSender)
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
        public void Post([FromBody] ReservationDetail reservationDetail)
        {
            ReservationDetail _resvDetsil = new ReservationDetail();
            _resvDetsil = reservationDetail;
            _resvDetsil.createdDate = DateTime.Now;
            _context.ReservationDetail.Add(_resvDetsil);
            _context.SaveChanges();
            var restaurantName = _context.RestaurantDetail.FirstOrDefault(a => a.id == _resvDetsil.restaurantId).restaurantName;
            string msg = string.Empty;
            msg += "A new Reservation has been made at  " + restaurantName + " by " + _resvDetsil.firstName + " " + _resvDetsil.lastName + "<br/>";
            msg += "<b> Total Number of Guests : </b>" + _resvDetsil.guestNum + "<br/> <b> " + _resvDetsil.firstName + "'s Phone Number </b>" + _resvDetsil.reservationPhone + " and <b> email address :</b>" + _resvDetsil.reservationEmail;
            msg += "<br/> Reservation is made for Day : " + _resvDetsil.reservationDate + " at " + _resvDetsil.reservationTime + " for " + _resvDetsil.reservationType;

            _emailSender.SendEmailAsync(_resvDetsil.reservationEmail, "New Reservation request", msg);
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
