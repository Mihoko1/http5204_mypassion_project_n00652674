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
    public class HairstyleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // <example>
        // GET: api/HairstyleData/GetHairstyles
        /// </example>
        [ResponseType(typeof(IEnumerable<HairstyleDto>))]
        public IHttpActionResult GetHairstyles()
        {
            List<Hairstyle> Hairstyles = db.Hairstyles.ToList();
            List<HairstyleDto> HairstyleDtos = new List<HairstyleDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Hairstyle in Hairstyles)
            {
                HairstyleDto NewPlayer = new HairstyleDto
                {
                    HairstyleID = Hairstyle.HairstyleID,
                    DateUpload = Hairstyle.DateUpload,
                    HairstylePhoto = Hairstyle.HairstylePhoto,
                    Type = Hairstyle.Type,
                    Detail = Hairstyle.Detail,
                    MemberID = Hairstyle.MemberID
                };
                HairstyleDtos.Add(NewPlayer);
            }

            return Ok(HairstyleDtos);
        }

        // GET: api/HairstyleData/FindHairstyle/5
        [HttpGet]
        [ResponseType(typeof(HairstyleDto))]
        public IHttpActionResult FindHairstyle(int id)
        {
            Hairstyle Hairstyle = db.Hairstyles.Find(id);
            if (Hairstyle == null)
            {
                return NotFound();
            }

            HairstyleDto HairstyleDto = new HairstyleDto
            {
                HairstyleID = Hairstyle.HairstyleID,
                DateUpload = Hairstyle.DateUpload,
                HairstylePhoto = Hairstyle.HairstylePhoto,
                Type = Hairstyle.Type,
                Detail = Hairstyle.Detail,
                MemberID = Hairstyle.MemberID

            };

            //pass along data as 200 status code OK response
            return Ok(HairstyleDto);
        }


        // <example>
        // GET: api/MemberData/FindMemberForHairstyle/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(MemberDto))]
        public IHttpActionResult FindmemberForHairstyle(int id)
        {
           
            Member Member = db.Members
                .Where(m => m.Hairstyles.Any(p => p.HairstyleID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Member == null)
            {
                return NotFound();
            }

            MemberDto MemberDto = new MemberDto
            {
                MemberID = Member.MemberID,
                FirstName = Member.FirstName,
                LastName = Member.LastName,
            };


            //pass along data as 200 status code OK response
            return Ok(MemberDto);
        }



        /// <example>
        /// POST: api/HairstyleData/UpdateHairstyle/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateHairstyle(int id, [FromBody] Hairstyle hairstyle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hairstyle.HairstyleID)
            {
                return BadRequest();
            }

            db.Entry(hairstyle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HairstyleExists(id))
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


        /// <example>
        /// POST: api/HairstyleData/AddHairstyle
        ///  FORM DATA: Player JSON Object
        /// </example>
        [ResponseType(typeof(Hairstyle))]
        [HttpPost]
        public IHttpActionResult AddHairstyle([FromBody] Hairstyle hairstyle)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Hairstyles.Add(hairstyle);
            db.SaveChanges();

            return Ok(hairstyle.HairstyleID);
        }


        // POST: api/HairstyleDb
        [ResponseType(typeof(Hairstyle))]
        public IHttpActionResult PostHairstyle(Hairstyle hairstyle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Hairstyles.Add(hairstyle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hairstyle.HairstyleID }, hairstyle);
        }

        /// <example>
        /// POST: api/HairstyleData/DeleteHairstyle/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteHairstyle(int id)
        {
            Hairstyle hairstyle = db.Hairstyles.Find(id);
            if (hairstyle == null)
            {
                return NotFound();
            }

            db.Hairstyles.Remove(hairstyle);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Finds a player in the system. Internal use only.
        /// </summary>
        /// <param name="id">The player id</param>
        /// <returns>TRUE if the player exists, false otherwise.</returns>
        private bool HairstyleExists(int id)
        {
            return db.Hairstyles.Count(e => e.HairstyleID == id) > 0;
        }
    }

}
