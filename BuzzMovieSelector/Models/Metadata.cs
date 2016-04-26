using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuzzMovieSelector.Models
{
    public class UserMetadata
    {
        [Display(Name = "Email")]
        public string Email;
        [Display(Name = "First Name")]
        public string FirstName;
        [Display(Name = "Last Name")]
        public string LastName;
        [Display(Name = "Major")]
        public string Major;
        public int UserId;
        public bool IsBanned;
        public bool IsLocked;

    }
}