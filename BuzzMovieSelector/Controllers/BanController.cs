using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuzzMovieSelector.Controllers
{
    [Authorize(Roles = "Banned")]
    public class BanController : Controller
    {
        // GET: Ban
        public ActionResult Index()
        {
            return View();
        }
    }
}