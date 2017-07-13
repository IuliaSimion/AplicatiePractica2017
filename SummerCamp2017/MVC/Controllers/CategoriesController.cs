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
    public class CategoriesController : Controller
    {
        public List<Category> GetCategories()
        {
            RestClient<Category> rc = new RestClient<Category>();
            rc.WebServiceUrl = "http://localhost:61144/api/categories/";
            List<Category> categoriesList = new List<Category>();
            categoriesList = rc.Get();

            return categoriesList;
        }

        public Category GetCategoryById(int id)
        {
            RestClient<Category> rc = new RestClient<Category>();
            rc.WebServiceUrl = "http://localhost:61144/api/categories/";
            Category category = new Category();
            category = rc.GetById(id);

            return category;
        }

        public HttpResponseMessage PostCategory(Category category)
        {
            RestClient<Category> rc = new RestClient<Category>();
            rc.WebServiceUrl = "http://localhost:61144/api/categories/";
            var response = rc.Post(category);

            return response;
        }

        public bool PutCategory(int id, Category category)
        {
            RestClient<Category> rc = new RestClient<Category>();
            rc.WebServiceUrl = "http://localhost:61144/api/categories/";
            bool response = rc.Put(id, category);

            return response;
        }

        public bool DeleteCategory(int id)
        {
            RestClient<Category> rc = new RestClient<Category>();
            rc.WebServiceUrl = "http://localhost:61144/api/categories/";
            bool response = rc.Delete(id);

            return response;
        }

        // GET: Announcements
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            PostCategory(category);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            Category category = GetCategoryById(id);

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeleteCategory(id);

            return RedirectToAction("List");
        }

        public ActionResult Details(int id)
        {
            Category category = GetCategoryById(id);

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            Category category = GetCategoryById(id);

            return View(category);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            PutCategory(category.CategoryId, category);

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            List<Category> categoriesList = GetCategories();

            return View(categoriesList);
        }
    }
}