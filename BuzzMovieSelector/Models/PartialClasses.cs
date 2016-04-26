using BuzzMovieSelector.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BuzzMovieSelector.DataAccess
{
    public class PartialClasses
    {
        [MetadataType(typeof(UserMetadata))]
        public partial class User
        {

        }

        public partial class Movie
        {
            [NotMapped]
            public RottenTomatoes.Api.Movie APIMovie { get; set; }
        }
    }
}