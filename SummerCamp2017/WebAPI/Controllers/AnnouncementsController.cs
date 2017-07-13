using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AnnouncementsController : ApiController
    {
        private SummerCampDBEntities db = new SummerCampDBEntities();

        // GET: api/Announcements
        [HttpGet]
        public IEnumerable<Announcement> GetAnnouncements()
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
            IEnumerable<Announcement> announcements = db.Announcements;
            List<Announcement> announcementsConfirmed = new List<Announcement>();
            foreach(var item in announcements)
            {
                if (item.Confirmed)
                {
                    announcementsConfirmed.Add(item);
                }
            }
            return announcementsConfirmed;
                //return db.Announcements;
            //}
        }

        // GET: api/Announcements/5
        [HttpGet]
        [ResponseType(typeof(Announcement))]
        public IHttpActionResult GetAnnouncement(int id)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                Announcement announcement = db.Announcements.Find(id);
                if (announcement == null)
                {
                    return NotFound();
                }

                return Ok(announcement);
            //}
        }

        [Route("api/Announcements/ActivateAnnouncement/{id}")]
        public HttpResponseMessage ActivateAnnouncement([FromUri]int id, [FromBody] string email)
        {
            Announcement announcement = db.Announcements.Find(id);
            if(announcement.Email == email)
            {
                announcement.Confirmed = true;
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            else
            {
                HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.Forbidden);
                return response1;
            }
        }

        [HttpGet]
        [Route("api/Announcements/Search/{categoryId?}/{value?}/{startDate?}/{endDate?}")]
        public IEnumerable<Announcement> Search(int? categoryId, string value, DateTime? startDate, DateTime? endDate)
        {
            string searchedVal = value.ToLower();
            IEnumerable<Announcement> ann = db.Announcements;
            if (value != null)
            {
                ann = db.Announcements.Where(a => a.Title.ToLower().Contains(searchedVal) || a.Description.ToLower().Contains(searchedVal));
            }
            if (categoryId.HasValue)
            {
                ann = ann.Where(a => a.CategoryId == categoryId);
            }

            if (startDate.HasValue)
            {
                ann = ann.Where(a => a.PostDate >= startDate);
            }
            if (endDate.HasValue)
            {
                ann = ann.Where(a => a.ExpirationDate <= endDate);
            }

            List<Announcement> announcementsConfirmed = new List<Announcement>();
            foreach (var item in ann)
            {
                if (item.Confirmed)
                {
                    announcementsConfirmed.Add(item);
                }
            }
            return announcementsConfirmed;

           // return ann;
        }


        // PUT: api/Announcements/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnnouncement(int id, [FromBody] Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != announcement.AnnouncementId)
            {
                return BadRequest();
            }

            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                if (db.Announcements.Count(e => e.AnnouncementId == id) < 1)
                {
                    return NotFound();
                }

                Announcement ann = db.Announcements.Find(announcement.AnnouncementId);
                ann.CategoryId = announcement.CategoryId;
                ann.Title = announcement.Title;
                ann.Description = announcement.Description;
                db.SaveChanges();
            //}


            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Announcements
        [HttpPost]
        [ResponseType(typeof(Announcement))]
        public IHttpActionResult PostAnnouncement([FromBody] Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                announcement.Closed = false;
                announcement.PostDate = DateTime.Now;
                announcement.ExpirationDate = DateTime.Now.AddMonths(1);

                db.Announcements.Add(announcement);
                db.SaveChanges();
            //}

            return CreatedAtRoute("DefaultApi", new { id = announcement.AnnouncementId }, announcement);
        }

        //POST: api/Announcements/CloseAnnouncement/5
        //[HttpPost]
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [Route("api/Announcements/CloseAnnouncement/{id}")]
        public HttpResponseMessage CloseAnnouncement(int id, [FromBody] string email)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
            Announcement announcement = db.Announcements.Find(id);

            //if (announcement.Closed)
            //{
            //    HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.BadRequest);
            //    return response1;
            //}
            if (announcement.Email == email)
            {
                if (!(announcement.Closed))
                {
                    announcement.Closed = true;
                    db.SaveChanges();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
                else
                {
                    HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.BadRequest);
                    return response1;
                }
            }
            else
            {
                HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.Forbidden);
                return response2;
            }
            //}
           
        }

        //POST: api/Announcements/ExtendAnnouncement/5
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [HttpPost]
        [Route("api/Announcements/ExtendAnnouncement/{id}")]
        public HttpResponseMessage ExtendAnnouncement(int id, [FromBody] string email)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                Announcement announcement = db.Announcements.Find(id);

                if (announcement.Email == email)
                {
                    if (!(announcement.Closed))
                    {
                        if (announcement.ExpirationDate.AddDays(-3) >= DateTime.Now)
                        {
                            announcement.ExpirationDate = announcement.ExpirationDate.AddMonths(1);
                            db.SaveChanges();
                            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
                            return response;
                        }
                        else
                        {
                            HttpResponseMessage response1 = Request.CreateResponse(HttpStatusCode.MethodNotAllowed);
                            return response1;
                        }
                    }
                    else
                    {
                        HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.BadRequest);
                        return response2;
                    }
                }
                else
                {
                    HttpResponseMessage response3 = Request.CreateResponse(HttpStatusCode.Forbidden);
                    return response3;
                }

            //}

            
        }

        // DELETE: api/Announcements/5
        [HttpDelete]
        [ResponseType(typeof(Announcement))]
        public IHttpActionResult DeleteAnnouncement(int id)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                Announcement announcement = db.Announcements.Find(id);
                if (announcement == null)
                {
                    return NotFound();
                }

                db.Announcements.Remove(announcement);
                db.SaveChanges();

                return Ok(announcement);
            //}
        }

    }
}