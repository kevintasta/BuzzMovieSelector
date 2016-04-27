using BuzzMovieSelector.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuzzMovieSelector.Models
{
    public class RateMovieViewModel
    {
        public Movie Movie { get; set; }
        public RottenTomatoes.Api.Movie APIMovie { get; set; }
        public int UserRating { get; set; }
        public string Major { get; set; }
        public double MajorRating { get; set; }
    }

    public class RecommendViewModel
    {
        public Movie Movie { get; set; }
        public double MajorRating { get; set; }
        public int UserRating { get; set; }
        public string Major { get; set; }
    }
}