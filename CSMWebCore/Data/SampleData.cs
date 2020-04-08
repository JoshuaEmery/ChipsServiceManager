using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSMWebCore.Data
{
    public class SampleData
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<Customer>().HasData(

                new Customer
                {
                    Id = 1,
                    FirstName = "Andrew",
                    LastName = "Bernard",
                    Email = "andy@yahoo.com",
                    Phone = "2063953029",
                    StudentId ="830549793",
                    ContactPref = Models.ContactPref.Email
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Pam",
                    LastName = "Beesly",
                    Email = "pam01@gmail.com",
                    Phone = "3921235324",
                    StudentId = "223563434",
                    ContactPref = Models.ContactPref.Phone
                }
            );
        }
    }
}
