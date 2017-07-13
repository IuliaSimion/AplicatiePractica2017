using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestClient;
using System.Net.Http;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MVC.Controllers
{
    public class AnnouncementsController : Controller
    {


        public List<AnnouncementListModel> GetAnnouncements()
        {
            RestClient<AnnouncementListModel> rc = new RestClient<AnnouncementListModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/announcements/";
            List<AnnouncementListModel> announcementsList = rc.Get();
            foreach(var announcement in announcementsList)
            {
                if(announcement.ExpirationDate < DateTime.Now)
                {
                    announcement.Closed = true;
                }
            }
            return announcementsList;
        }

        public List<AnnouncementListModel> GetSearchedAnnouncements(AdvancedSearch searchedAnn)
        {
            RestClient<AnnouncementListModel> rc = new RestClient<AnnouncementListModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/Announcements/Search/";
            List<AnnouncementListModel> searchedList = rc.Search(searchedAnn.CategoryId,searchedAnn.Value,searchedAnn.StartDate,searchedAnn.EndDate);

            return searchedList;
        } 

        public List<Category> GetCategories()
        {
            RestClient<Category> rc = new RestClient<Category>();
            rc.WebServiceUrl = "http://localhost:61144/api/categories/";
            List<Category> categoriesList = rc.Get();

            return categoriesList;
        }

        public AnnouncementDetailsModel GetAnnouncementById(int id)
        {
            RestClient<AnnouncementDetailsModel> rc = new RestClient<AnnouncementDetailsModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/announcements/";
            AnnouncementDetailsModel announcement = rc.GetById(id);

            return announcement;
        }


        public AnnouncementDetailsModel PostAnnouncement(AnnouncementCreateModel announcement)
        {
            RestClient<AnnouncementCreateModel> rc = new RestClient<AnnouncementCreateModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/announcements/";
            HttpResponseMessage response = rc.Post(announcement);
            var obj = JsonConvert.DeserializeObject<AnnouncementDetailsModel>(response.Content.ReadAsStringAsync().Result);

            return obj;

            //return response;
        }

        public bool PutAnnouncement(int id, AnnouncementEditModel announcement)
        {
            RestClient<AnnouncementEditModel> rc = new RestClient<AnnouncementEditModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/announcements/";
            bool response = rc.Put(id, announcement);

            return response;
        }

        public bool DeleteAnnouncement(int id)
        {
            RestClient<AnnouncementDetailsModel> rc = new RestClient<AnnouncementDetailsModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/announcements/";
            bool response = rc.Delete(id);

            return response;
        }

        public HttpResponseMessage CloseAnnouncement(int id, string email)
        {

            RestClient<AnnouncementDetailsModel> rc = new RestClient<AnnouncementDetailsModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/Announcements/CloseAnnouncement/";
            HttpResponseMessage response = rc.Close(id,email);

            return response;
        }

        public HttpResponseMessage ExtendAnnouncement(int id, string email)
        {

            RestClient<AnnouncementDetailsModel> rc = new RestClient<AnnouncementDetailsModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/Announcements/ExtendAnnouncement/";
            HttpResponseMessage response = rc.Extend(id,email);

            return response;
        }


        private HttpResponseMessage ConfirmAnnouncement(int id, string email)
        {
            RestClient<AnnouncementDetailsModel> rc = new RestClient<AnnouncementDetailsModel>();
            rc.WebServiceUrl = "http://localhost:61144/api/Announcements/ActivateAnnouncement/";
            HttpResponseMessage response = rc.Confirm(id, email);

            return response;
        }

        public ActionResult AdvancedSearch()
        {
            List<Category> categories = GetCategories();
            List<Category> dropdownList = new List<Category>();
            dropdownList.Add(null);
            foreach(var categ in categories)
            {
                dropdownList.Add(categ);
            }
            ViewBag.Categories = dropdownList;
            return View();
        }

        [HttpPost]
        public ActionResult AdvancedSearch(AdvancedSearch searchedAnn)
        {
            List<AnnouncementListModel> searchedList = GetSearchedAnnouncements(searchedAnn);
            TempData["searched"] = searchedList;
            return RedirectToAction("List");
        }

       
        public void SendEmail(MailMessage mail)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.EnableSsl = true;
            //smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("", "");
            smtp.Host = "smtp.gmail.com";

            mail.From = new MailAddress("");

            smtp.Send(mail);
        }

        private string ToHash(int id, string email)
        {
            string hash123 = String.Concat(id, email);
            MD5 md5 = new MD5CryptoServiceProvider();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(hash123);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes);
        }

        public ActionResult Confirm(int id, string hash)
        {
            AnnouncementDetailsModel announcement = GetAnnouncementById(id);
            string computedHash = ToHash(id, announcement.Email);

            if (computedHash == hash)
            {
                HttpResponseMessage response = ConfirmAnnouncement(id, announcement.Email);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Confirmation = "Your announcement has been successfully posted!";
                    return View();
                }
                else
                {
                    ViewBag.Confirmation = "There was a problem posting your announcement!";
                    return View();
                }
            }
            else
            {
                ViewBag.Confirmation = "Forbidden!";
                return View();
            }
        }
        // GET: Announcements
        public ActionResult Create()
        {
            ViewBag.Categories = GetCategories();
            return View();
        }

        [HttpPost]
        public ActionResult Create(AnnouncementCreateModel announcement)
        {
            AnnouncementDetailsModel result = PostAnnouncement(announcement);
            MailMessage message = new MailMessage();
            message.To.Add(announcement.Email);
            message.Subject = announcement.Title;
            var callbackUrl = Url.Action("Confirm", "Announcements", new { id = result.AnnouncementId, hash = ToHash(result.AnnouncementId, announcement.Email) }, protocol: Request.Url.Scheme);
            message.Body = "Please confirm your announcement by clicking <a href=\"" + callbackUrl + "\">here</a>";
            message.IsBodyHtml = true;


            SendEmail(message);
            

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            AnnouncementDetailsModel announcement = GetAnnouncementById(id);

            return View(announcement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeleteAnnouncement(id);

            return RedirectToAction("List");
        }

        public ActionResult Details(int id)
        {
            AnnouncementDetailsModel announcement = GetAnnouncementById(id);

            return View(announcement);
        }

        public ActionResult VerifyEmail(int id)
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult VerifyEmail(int id, EmailTextBox txtBox)
        {
            AnnouncementDetailsModel announcement = GetAnnouncementById(id);
            if (announcement.Email == txtBox.Email)
            {
                if (!(announcement.Closed))
                {
                    return RedirectToAction("Edit", new { id = announcement.AnnouncementId });
                }
                else
                {
                    //ViewBag.Error = "This announcement is closed! Cannot edit!";
                    ModelState.AddModelError(string.Empty, "This announcement is closed! Cannot edit!");
                }
            }
            else
            {
                //ViewBag.Error = "Input data is incorrect!";
                ModelState.AddModelError(string.Empty, "Input data is incorrect!");
            }
            return View();
        }

        public ActionResult Close(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Close(int id, EmailTextBox txtBox)
        {
            HttpResponseMessage response = CloseAnnouncement(id, txtBox.Email);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("List");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                //ViewBag.Error = "Input data is incorrect!";
                ModelState.AddModelError(string.Empty, "Input data is incorrect!");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                //ViewBag.Error = "This announcement is already closed!";
                ModelState.AddModelError(string.Empty, "This announcement is already closed!");
            }

            return View();
        }


        public ActionResult Extend(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Extend(int id, EmailTextBox txtBox)
        {

            HttpResponseMessage response = ExtendAnnouncement(id, txtBox.Email);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                    return RedirectToAction("List");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                //ViewBag.Error = "Input data is incorrect!";
                ModelState.AddModelError( string.Empty, "Input data is incorrect!");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.MethodNotAllowed)
            {
                //ViewBag.Error = "This announcement is closed! Cannot extend!";
                ModelState.AddModelError(string.Empty, "The period allowed for extending has expired!");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                    //ViewBag.Error = "This announcement is closed! Cannot extend!";
                    ModelState.AddModelError(string.Empty, "This announcement is closed! Cannot extend!");
            }
            

            return View();

        }
        public ActionResult Edit(int id)
        {
            AnnouncementDetailsModel announcement = GetAnnouncementById(id);
            AnnouncementEditModel newAnnouncement = new AnnouncementEditModel();
            newAnnouncement.AnnouncementId = announcement.AnnouncementId;
            newAnnouncement.Phonenumber = announcement.Phonenumber;
            newAnnouncement.Email = announcement.Email;
            newAnnouncement.Title = announcement.Title;
            newAnnouncement.Description = announcement.Description;
            newAnnouncement.CategoryId = announcement.CategoryId;
            ViewBag.Categories = GetCategories();
            return View(newAnnouncement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AnnouncementEditModel announcement)
        {
            PutAnnouncement(announcement.AnnouncementId, announcement);

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (TempData["searched"] == null)
            {
                List<AnnouncementListModel> announcementList = GetAnnouncements();

                return View(announcementList);
            }
            else
            {
                var annList = TempData["searched"];
                return View(annList);
            }
        }
    }
}