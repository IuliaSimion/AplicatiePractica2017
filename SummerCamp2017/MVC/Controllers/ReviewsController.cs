using MVC.Models;
using RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class ReviewsController : Controller
    {
        public List<Review> GetReviews()
        {
            RestClient<Review> rc = new RestClient<Review>();
            rc.WebServiceUrl = "http://localhost:61144/api/reviews/";
            List<Review> reviewsList = new List<Review>();
            reviewsList = rc.Get();
            List<Review> orderedList = reviewsList.OrderByDescending(r => r.DatePosted).ToList();

            return orderedList;
        }

        public Review GetReviewById(int id)
        {
            RestClient<Review> rc = new RestClient<Review>();
            rc.WebServiceUrl = "http://localhost:61144/api/reviews/";
            Review review = new Review();
            review = rc.GetById(id);

            return review;
        }

        public List<Review> GetReviewsPerAnnouncement(int id)
        {
            RestClient<Review> rc = new RestClient<Review>();
            rc.WebServiceUrl = "http://localhost:61144/api/reviews/reviewsperannouncement/";
            List<Review> reviewsList = new List<Review>();
            reviewsList = rc.GetByAnnouncement(id);
            List<Review> orderedList = reviewsList.OrderByDescending(r => r.DatePosted).ToList();

            return orderedList;

        }

        public HttpResponseMessage PostReview(ReviewCreateModel review)
        {
            RestClient<ReviewCreateModel> rc = new RestClient<ReviewCreateModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/reviews/";
            var response = rc.Post(review);

            return response;
        }

        public bool PutReview(int id, ReviewCreateModel review)
        {
            RestClient<ReviewCreateModel> rc = new RestClient<ReviewCreateModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/reviews/";
            bool response = rc.Put(id, review);

            return response;
        }

        public bool DeleteReview(int id)
        {
            RestClient<Review> rc = new RestClient<Review>();
            rc.WebServiceUrl = "http://localhost:61144/api/reviews/";
            bool response = rc.Delete(id);

            return response;
        }

        // GET: Announcements
        public ActionResult Create(int idAnn)
        {
            ViewBag.RatingRange = new int[] {1,2,3,4,5};
            return View();
        }

        [HttpPost]
        public ActionResult Create(int idAnn, ReviewCreateModel review)
        {
            review.AnnouncementId = idAnn;
            if (review.Comment != null || review.Rating != null)
            {
                PostReview(review);

                return RedirectToAction("List", new { id = idAnn });
            }
            else
            {
                //ViewBag.RatingRange = new int[] { 1, 2, 3, 4, 5 };
                //ViewBag.Error = "Please rate or comment!";
                ModelState.AddModelError(string.Empty, "Please rate or comment!");
                return View();
            }
        }

        public ActionResult Delete(int idAnn, int id)
        {
            DeleteReview(id);
            return RedirectToAction("List", new { id=idAnn });
        }

        public ActionResult Details(int id)
        {
            Review review = GetReviewById(id);

            return View(review);
        }

        public ActionResult Edit(int id)
        {
            Review review = GetReviewById(id);

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int idAnn, Review review)
        {
            review.AnnouncementId = idAnn;
            PutReview(review.ReviewId, review);
            
            return RedirectToAction("List", new { id=idAnn });
        }

        public ActionResult List(int id)
        {
            ReviewsPerAnnouncement revPerAnn = new ReviewsPerAnnouncement();
            revPerAnn.CurrentAnnouncementId = id;
            List<Review> reviewsList = GetReviewsPerAnnouncement(id);
            revPerAnn.Reviews = reviewsList;

            if (reviewsList.Count == 0)
            {
                revPerAnn.AverageRating = 0;
            }
            else
            {
                revPerAnn.AverageRating = 0;
                foreach (var review in reviewsList)
                {
                    revPerAnn.AverageRating += Convert.ToDecimal(review.Rating);
                }
                revPerAnn.AverageRating /= reviewsList.Count();
            }

            return View(revPerAnn);
        }
    }
}