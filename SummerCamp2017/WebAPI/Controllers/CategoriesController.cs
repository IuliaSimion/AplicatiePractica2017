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
    public class CategoriesController : ApiController
    {
        private SummerCampDBEntities db = new SummerCampDBEntities();

        // GET: api/Categories
        [HttpGet]
        public IQueryable<Category> GetCategories()
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                return db.Categories;
            //}
        }

        // GET: api/Categories/5
        [HttpGet]
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                Category category = db.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);
            //}
        }

        // PUT: api/Categories/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                if (db.Categories.Count(e => e.CategoryId == id) < 1)
                {
                    return NotFound();
                }

                Category categ = db.Categories.Find(category.CategoryId);
                categ.Name = category.Name;
                db.SaveChanges();
            //}
           
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [HttpPost]
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                db.Categories.Add(category);
                db.SaveChanges();
            //}

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete]
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            //using (SummerCampDBEntities db = new SummerCampDBEntities())
            //{
                Category category = db.Categories.Find(id);
                if (category == null)
                {
                    return NotFound();
                }

                db.Categories.Remove(category);
                db.SaveChanges();

                return Ok(category);
            //}
        }

    }
}