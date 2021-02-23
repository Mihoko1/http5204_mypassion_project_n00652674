using System;
using System.IO;
using System.Web;
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
using System.Diagnostics;

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
                    SalonEmail = Salon.SalonEmail,
                    Address = Salon.Address,
                    City = Salon.City,
                    Postal = Salon.Postal,
                    Area = Salon.Area,
                    Website = Salon.Website,
                    Phone = Salon.Phone,
                    SalonPicture = Salon.SalonPicture,
                    SalonHasPic = Salon.SalonHasPic
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
                SalonEmail = Salon.SalonEmail,
                Address = Salon.Address,
                City = Salon.City,
                Postal = Salon.Postal,    
                Area = Salon.Area,
                Website = Salon.Website,
                Phone = Salon.Phone,
                SalonPicture = Salon.SalonPicture,
                SalonHasPic = Salon.SalonHasPic
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

        // Update Salon Picture
        [HttpPost]
        public IHttpActionResult UpdateSalonPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var SalonPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (SalonPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(SalonPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Salon/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Salons/"), fn);

                                //save the file
                                SalonPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the salon haspic and picextension fields in the database
                                Salon SelectedSalon = db.Salons.Find(id);
                                SelectedSalon.SalonHasPic = haspic;
                                SelectedSalon.SalonPicture = extension;
                                db.Entry(SelectedSalon).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Member Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
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