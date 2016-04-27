using RestSharp;
using RottenTomatoes.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using BuzzMovieSelector.Models;

namespace BuzzMovieSelector.DataAccess
{
    public class BuzzMovieSelectorRepository
    {
        #region RottenTomatoes Methods
        private static string apiKey = "yedukp76ffytfuy24zsqk7f5";
        public static IList<RottenTomatoes.Api.Movie> searchMovies(string query)
        {
            var client = new RottenTomatoesRestClient(apiKey);
            var movies = client.MoviesSearch(query);
            if (movies == null)
            {
                return new List<RottenTomatoes.Api.Movie>();
            }
            return movies.Movies;
        }

        public static IList<RottenTomatoes.Api.Movie> getNewDvdReleases()
        {
            var client = new RottenTomatoesRestClient(apiKey);
            var movies = client.NewReleaseDvds();
            if (movies.Movies == null)
            {
                return new List<RottenTomatoes.Api.Movie>();
            }
            return movies.Movies;
        }

        public static IList<RottenTomatoes.Api.Movie> getNewInTheaters()
        {
            var client = new RottenTomatoesRestClient(apiKey);
            var movies = client.InTheatersMovies();
            if (movies.Movies == null)
            {
                return new List<RottenTomatoes.Api.Movie>();
            }
            return movies.Movies;
        }

        public static RottenTomatoes.Api.Movie getMovieByID(int movieID)
        {
            var client = new RottenTomatoesRestClient(apiKey);
            var movie = client.MovieInfo(movieID);
            return movie;
        }
        #endregion

        #region Movie Methods
        public static bool checkMovieInDatabase(int movieID)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                var movie = context.Movies.FirstOrDefault(m => m.MovieId.Equals(movieID));
                if (movie == null)
                {
                    return false;
                }
                return true;
            }
        }

        public static void addMovie(RottenTomatoes.Api.Movie apiMovie)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                var movie = new Movie();
                movie.MovieId = (int)apiMovie.Id;
                movie.Description = apiMovie.Synopsis;
                movie.Name = apiMovie.Title;
                context.Movies.Add(movie);
                context.SaveChanges();
            }
        }

        public static Movie getDatabaseMovieById(int movieId)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                return context.Movies
                    .Include(x => x.Ratings)
                    .FirstOrDefault(m => m.MovieId.Equals(movieId));
            }
        }
        #endregion

        #region Rating Methods
        public static Rating getUserRatingForMovie(string email, int movieID)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                var user = context.Users.Include(r => r.Ratings).FirstOrDefault(u => u.Email.Equals(email));
                return user.Ratings.FirstOrDefault(r => r.MovieId.Equals(movieID));
            }
        }

        public static void processRating(RateMovieViewModel viewModel, string email)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                var user = context.Users.Include(r => r.Ratings).FirstOrDefault(u => u.Email.Equals(email));
                var movie = context.Movies.Include(r => r.Ratings).FirstOrDefault(m => m.MovieId.Equals(viewModel.Movie.MovieId));
                var rating = context.Ratings.FirstOrDefault(r => r.MovieId.Equals(movie.MovieId) && r.UserId.Equals(user.UserId));
                if (rating == null)
                {
                    rating = new Rating();
                    rating.Major = user.Major;
                    rating.MovieId = movie.MovieId;
                    rating.RatingValue = viewModel.UserRating;
                    rating.UserId = user.UserId;
                    context.Ratings.Add(rating);
                    user.Ratings.Add(rating);
                    movie.Ratings.Add(rating);
                }
                else
                {
                    rating.RatingValue = viewModel.UserRating;
                }
                context.SaveChanges();
            }
        }

        public static double getMajorRaingForMovie(int movieID, string email)
        {
            var major = getMajorOfUser(email);
            return getMajorRatingForMovie(movieID, major);
        }

        public static double getMajorRatingForMovie(int movieID, string major)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                var majorRatings = context.Ratings.Where(r => r.Major.Equals(major) && r.MovieId.Equals(movieID));
                var count = majorRatings.Count();
                if (count == 0)
                {
                    return 0.0;
                }
                var total = 0.0;
                foreach (var rating in majorRatings)
                {
                    total += rating.RatingValue;
                }
                return total / count;
            }
        }

        public static RecommendViewModel getRecommendationForMajor(string email)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                var major = getMajorOfUser(email);
                var ratings = context.Ratings.Where(r => r.Major.Equals(major)).Select(x => x.Movie).Distinct();
                if (ratings == null || ratings.Count() == 0)
                {
                    return null;
                }
                var max = 0.0;
                var maxMovieID = 1;
                foreach (var element in ratings)
                {
                    var majorRating = getMajorRatingForMovie(element.MovieId, major);
                    if (majorRating > max)
                    {
                        max = majorRating;
                        maxMovieID = element.MovieId;
                    }
                }
                var viewModel = new RecommendViewModel();
                viewModel.MajorRating = max;
                var userRating = getUserRatingForMovie(email, maxMovieID);
                if (userRating != null)
                {
                    viewModel.UserRating = getUserRatingForMovie(email, maxMovieID).RatingValue;
                } else
                {
                    viewModel.UserRating = 0;
                }
                viewModel.Major = major;
                viewModel.Movie = getDatabaseMovieById(maxMovieID);
                return viewModel;
            }
        }
        #endregion



        public static void addUser(User user)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static string getMajorOfUser(string email)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                return context.Users.FirstOrDefault(u => u.Email.Equals(email)).Major;
            }
        }




    }
}