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
    public class MemberDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        // GET: api/MemberData/GetMembers
        [ResponseType(typeof(IEnumerable<MemberDto>))]
        public IHttpActionResult GetMembers()
        {
            List<Member> Members = db.Members.ToList();
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
                    SalonID = Member.SalonID,
                    SalonName = Member.SalonName
                  
                };
                MemberDtos.Add(NewMember);
            }

            return Ok(MemberDtos);
        }


        // GET: api/MemberData/FindMember/5
        [HttpGet]
        [ResponseType(typeof(MemberDto))]
        public IHttpActionResult FindMember(int id)
        {
            Member Member = db.Members.Find(id);
            if (Member == null)
            {
                return NotFound();
            }

            MemberDto MemberDto = new MemberDto
            {
                MemberID = Member.MemberID,
                FirstName = Member.FirstName,
                LastName = Member.LastName,
                Title = Member.Title,
                Email = Member.Email,
                Picture = Member.Picture,
                SalonID = Member.SalonID,
                SalonName = Member.SalonName


            };


            //pass along data as 200 status code OK response
            return Ok(MemberDto);
        }

        // PUT: api/MemberData/FindSalonForMember/5
        [HttpGet]
        [ResponseType(typeof(IEnumerable<SalonDto>))]
        public IHttpActionResult FindSalonForMember(int id)
        {

            Salon Salon = db.Salons
                .Where(s => s.Members.Any(m => m.SalonID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Salon == null)
            {
                return NotFound();
            }

            SalonDto SalonDto = new SalonDto
            {
                SalonID = Salon.SalonID,
                SalonName = Salon.SalonName
               
            };


            //pass along data as 200 status code OK response
            return Ok(SalonDto);
        }

        /// POST: api/MemberData/AddMember
        ///  FORM DATA: Player JSON Object
        /// </example>
        [ResponseType(typeof(Member))]
        [HttpPost]
        public IHttpActionResult AddMember([FromBody] Member member)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Members.Add(member);
            db.SaveChanges();

            return Ok(member.MemberID);
        }

        /// POST: api/MemberData/UpdateMember/5
     
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateMember(int id, [FromBody] Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != member.MemberID)
            {
                return BadRequest();
            }

            db.Entry(member).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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



        // DELETE: api/MemberData/5
        [HttpPost]
        public IHttpActionResult DeleteMember(int id)
        {
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            db.SaveChanges();

            return Ok(member);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.MemberID == id) > 0;
        }
    }
}