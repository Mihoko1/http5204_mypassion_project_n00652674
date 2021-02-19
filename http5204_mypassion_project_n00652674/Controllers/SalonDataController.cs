using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using http5204_mypassion_project_n00652674.Models;

namespace http5204_mypassion_project_n00652674.Controllers
{
   
    public class SalonDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SalonData/GetSalons

        [ResponseType(typeof(IEnumerable<SalonDto>))]
        public IHttpActionResult GetSalons()
        {
            List<Salon> Salons = db.Salons.ToList();
            List<SalonDto> SalonDtos = new List<SalonDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Salon in Salons)
            {
                SalonDto NewSalon = new SalonDto
                {
                    SalonID = Salon.SalonID,
                    SalonName = Salon.SalonName,
                    Address = Salon.Address,
                    City = Salon.City,
                    Area = Salon.Area,
                    Website = Salon.Website,
                    Phone = Salon.Phone
                    //SalonPicture = Salon.SalonPicture
                };
                SalonDtos.Add(NewSalon);
            }

            return Ok(SalonDtos);
        }
        // GET: api/SalonData/FindSalon/5
        [HttpGet]
        [ResponseType(typeof(SalonDto))]
        public IHttpActionResult FindSalon(int id)
        {
            Salon Salon = db.Salons.Find(id);
            if (Salon == null)
            {
                return NotFound();
            }
            //put into a 'friendly object format'
            SalonDto SalonDto = new SalonDto
            {
                SalonID = Salon.SalonID,
                SalonName = Salon.SalonName,
                Address = Salon.Address,
                City = Salon.City,
                Area = Salon.Area,
                Website = Salon.Website,
                Phone = Salon.Phone,
                SalonPicture = Salon.SalonPicture
            };

            return Ok(SalonDto);
        }



        //GET: api/SalonData/GetMembersForSalon
        [ResponseType(typeof(IEnumerable<MemberDto>))]
        public IHttpActionResult GetMembersForSalon(int id)
        {
           
            List<Member> Members = db.Members.Where(m => m.SalonID == id)
                .ToList();
            List<MemberDto> MemberDtos = new List<MemberDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Member in Members)
            {
                MemberDto NewMember = new MemberDto
                {
                    MemberID = Member.MemberID,
                    FirstName = Member.FirstName,
                    LastName = Member.LastName,
                    Title = Member.Title,
                    Email = Member.Email,
                    Picture = Member.Picture,
                    SalonID = Member.SalonID
                };
                MemberDtos.Add(NewMember);
            }

            return Ok(MemberDtos);
        }



        /// POST: api/SalonData/UpdateSalon/5
        /// FORM DATA: Team JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSalon(int id, [FromBody] Salon Salon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Salon.SalonID)
            {
                return BadRequest();
            }

            db.Entry(Salon).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// POST: api/SalonData/AddSalon
        ///  FORM DATA: Team JSON Object
        /// </example>
        [ResponseType(typeof(Salon))]
        [HttpPost]
        public IHttpActionResult AddSalon([FromBody] Salon Salon)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Salons.Add(Salon);
            db.SaveChanges();

            return Ok(Salon.SalonID);
        }

        // DELETE: api/SalonData/5
        [HttpPost]
        public IHttpActionResult DeleteSalon(int id)
        {
            Salon salon = db.Salons.Find(id);
            if (salon == null)
            {
                return NotFound();
            }

            db.Salons.Remove(salon);
            db.SaveChanges();

            return Ok(salon);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SalonExists(int id)
        {
            return db.Salons.Count(e => e.SalonID == id) > 0;
        }
    }
}