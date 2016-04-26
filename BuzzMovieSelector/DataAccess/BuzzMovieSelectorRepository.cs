using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuzzMovieSelector.DataAccess
{
    public class BuzzMovieSelectorRepository
    {
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