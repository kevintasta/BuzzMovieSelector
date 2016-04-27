using BuzzMovieSelector.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuzzMovieSelector.Models;


namespace BuzzMovieSelector.Controllers
{
    [Authorize(Roles = "User")]
    public class MovieController : Controller
    {
        // GET: Movie
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SearchMovies()
        {
            return View(new SearchQuery());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchMovies(SearchQuery searchValue)
        {
            return RedirectToAction("MovieList", new { query = searchValue.Query });
        }

        public ActionResult MovieList(string query)
        {
            var movies = BuzzMovieSelectorRepository.searchMovies(query);
            return View(movies);
        }

        public ActionResult RateMovie(int movieID)
        {
            var movie = BuzzMovieSelectorRepository.getMovieByID(movieID);
            if (!BuzzMovieSelectorRepository.checkMovieInDatabase(movieID))
            {
                BuzzMovieSelectorRepository.addMovie(movie);
            }
            var dbMovie = BuzzMovieSelectorRepository.getDatabaseMovieById(movieID);
            var rating = BuzzMovieSelectorRepository.getUserRatingForMovie(User.Identity.Name, movieID);
            var rateViewModel = new RateMovieViewModel();
            rateViewModel.UserRating = 0;
            if (rating != null)
            {
                rateViewModel.UserRating = rating.RatingValue;
            }
            rateViewModel.Movie = dbMovie;
            rateViewModel.MajorRating = BuzzMovieSelectorRepository.getMajorRatingForMovie(movieID,
                BuzzMovieSelectorRepository.getMajorOfUser(User.Identity.Name));
            return View(rateViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RateMovie(RateMovieViewModel viewModel)
        {
            BuzzMovieSelectorRepository.processRating(viewModel, User.Identity.Name);
            return RedirectToAction("RateMovie", new { movieID = viewModel.Movie.MovieId });
        }

        public ActionResult DVD()
        {
            return View(BuzzMovieSelectorRepository.getNewDvdReleases());
        }

        public ActionResult InTheaters()
        {
            return View(BuzzMovieSelectorRepository.getNewInTheaters());
        }

        public ActionResult Recommend()
        {
            var viewModel = BuzzMovieSelectorRepository.getRecommendationForMajor(User.Identity.Name);
            if (viewModel == null)
            {
                viewModel = new RecommendViewModel();
                viewModel.Movie = null;
                viewModel.Major = BuzzMovieSelectorRepository.getMajorOfUser(User.Identity.Name);
            }
            return View(viewModel);
        }

        public ActionResult EditProfile()
        {
            return View(BuzzMovieSelectorRepository.getUserProfile(User.Identity.Name));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(User user)
        {
            BuzzMovieSelectorRepository.updateUserInformation(user);
            return RedirectToAction("EditProfile");
        }

        public ActionResult MyRatings()
        {
            return View(BuzzMovieSelectorRepository.getAllRatingsForUser(User.Identity.Name));
        }
    }
}