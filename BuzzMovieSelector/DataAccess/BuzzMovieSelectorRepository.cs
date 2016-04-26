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
            return movies.Movies;
        }

        public static IList<RottenTomatoes.Api.Movie> getNewDvdReleases()
        {
            var client = new RottenTomatoesRestClient(apiKey);
            var movies = client.NewReleaseDvds();
            return movies.Movies;
        }

        public static IList<RottenTomatoes.Api.Movie> getNewInTheaters()
        {
            var client = new RottenTomatoesRestClient(apiKey);
            var movies = client.InTheatersMovies();
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
        #endregion



        public static void addUser(User user)
        {
            using (var context = new BuzzMovieSelectorEntities())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }




    }
}