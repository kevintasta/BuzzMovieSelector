using BuzzMovieSelector.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuzzMovieSelector.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageUsers()
        {
            return View(AdminRepository.getUsers());
        }

        public ActionResult BanUser(string userID)
        {
            AdminRepository.banUser(userID);
            return RedirectToAction("ManageUsers");
        }

        public ActionResult UnbanUser(string userID)
        {
            AdminRepository.unbanUser(userID);
            return RedirectToAction("ManageUsers");
        }
    }
}