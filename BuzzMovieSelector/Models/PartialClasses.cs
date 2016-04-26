using BuzzMovieSelector.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}