using BuzzMovieSelector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BuzzMovieSelector.DataAccess
{
    public class AdminRepository
    {
        public static ICollection<ApplicationUser> getUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users.ToList();
            }
        }

        public static ApplicationUser getUserFromID(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id.Equals(id));
            }
        }

        public static void banUser(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = getUserFromID(id);
                var banRole = context.Roles.FirstOrDefault(x => x.Name.Equals("User"));
                var matchedRole = user.Roles.FirstOrDefault(r => r.RoleId.Equals(banRole.Id));
                user.Roles.Remove(matchedRole);
                banRole = context.Roles.FirstOrDefault(x => x.Name.Equals("Banned"));
                var identityRole = new IdentityUserRole();
                identityRole.RoleId = banRole.Id;
                user.Roles.Add(identityRole);
                context.SaveChanges();
            }
        }

        public static void unbanUser(string id)
        {
            using (var context = new ApplicationDbContext())
            {
                var user = getUserFromID(id);
                var banRole = context.Roles.FirstOrDefault(x => x.Name.Equals("Banned"));
                var matchedRole = user.Roles.FirstOrDefault(r => r.RoleId.Equals(banRole.Id));
                user.Roles.Remove(matchedRole);
                banRole = context.Roles.FirstOrDefault(x => x.Name.Equals("User"));
                var identityRole = new IdentityUserRole();
                identityRole.RoleId = banRole.Id;
                user.Roles.Add(identityRole);
                context.SaveChanges();
            }
        }
    }
}