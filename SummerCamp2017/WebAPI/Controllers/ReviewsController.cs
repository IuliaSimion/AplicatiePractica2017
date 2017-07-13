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
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class ReviewsController : ApiController
    {
        private SummerCampDBEntities db = new SummerCampDBEntities();

        // GET: api/Reviews
        [HttpGet]
        public IQueryable<Review> GetReviews()
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                return db.Reviews;
            //}
        }

        // GET: api/Reviews/5
        [HttpGet]
        [ResponseType(typeof(Review))]
        public IHttpActionResult GetReview(int id)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                Review review = db.Reviews.Find(id);

                if (review == null)
                {
                    return NotFound();
                }

                return Ok(review);
            //}
        }


        // GET: api/Reviews/ReviewsPerAnnouncement/5
        [HttpGet]
        [Route("api/reviews/ReviewsPerAnnouncement/{id}")]
        public IQueryable<Review> GetReviewsByAnnouncementId(int id)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                return db.Reviews.Where(r => r.AnnouncementId == id);
            //}
        }

        // PUT: api/Reviews/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReview(int id, [FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                if (db.Reviews.Count(e => e.ReviewId == id) < 1)
                {
                    return NotFound();
                }

                Review rev = db.Reviews.Find(review.ReviewId);
                rev.Comment = review.Comment;
                rev.Rating = review.Rating;
                rev.Username = review.Username;
                db.SaveChanges();
            //}
           
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Reviews
        [HttpPost]
        [ResponseType(typeof(Review))]
        public IHttpActionResult PostReview([FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                review.DatePosted = DateTime.Now;
                db.Reviews.Add(review);
                db.SaveChanges();
            //}

            return CreatedAtRoute("DefaultApi", new { id = review.ReviewId }, review);
        }

        // DELETE: api/Reviews/5
        [HttpDelete]
        [ResponseType(typeof(Review))]
        public IHttpActionResult DeleteReview(int id)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                Review review = db.Reviews.Find(id);
                if (review == null)
                {
                    return NotFound();
                }

                db.Reviews.Remove(review);
                db.SaveChanges();

                return Ok(review);
            //}
        }

    }
}