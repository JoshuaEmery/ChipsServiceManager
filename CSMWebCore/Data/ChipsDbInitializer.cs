using CSMWebCore.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Data
{
    public static class ChipsDbInitializer
    {
        public static void SeedUsers(UserManager<ChipsUser> userManager)
        {
            if (userManager.FindByEmailAsync("chips@student.rtc.edu").Result == null)
            {
                ChipsUser user = new ChipsUser
                {
                    FirstName = "CHIPS",
                    LastName = "Admin",
                    Active = true,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "chips@student.rtc.edu",
                    NormalizedEmail = "CHIPS@STUDENT.RTC.EDU",
                    LockoutEnabled = false,
                };

                // initial password
                string password = "Password!1";
                // password must be 6-100 chars and have at least one uppercase, one digit, 
                // and one non-alphanumeric character.

                // TODO add requirement to change password on first sign-in?

                IdentityResult result = userManager.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                } else
                {
                    throw new FormatException("Initial admin password format invalid.");
                    // TODO error handling to pass along failed initial admin pw requirements
                }
            }
        }
    }
}
