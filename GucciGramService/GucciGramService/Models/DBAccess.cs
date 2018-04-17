using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GucciGramService.Models;

namespace GucciGramService.Models
{
    public static class DBAccess
    {
        private static ApplicationDbContext context;

        public static void SetContext(IApplicationBuilder app)
        {
            context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
        }

        public static void PushUserFromForm(RegistrationFormResponse form)
        {
            context.Database.Migrate();

            User user = new User();
            user.RoleID = 1;
            user.GanderID = int.Parse(form.GenderID);
            user.UserName = form.UserName;
            user.Bio = null;
            user.Email = form.Email;
            user.Password = form.Password;

            context.Users.AddRange(user);
            context.SaveChanges();
        }
    }
}
