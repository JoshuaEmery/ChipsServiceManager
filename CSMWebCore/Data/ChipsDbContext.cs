﻿using System;
using System.Collections.Generic;
using System.Text;

using CSMWebCore.Controllers;
using CSMWebCore.Entities;
using CSMWebCore.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CSMWebCore.ViewModels;
using CSMWebCore.Services;

namespace CSMWebCore.Data
{
    public class ChipsDbContext : IdentityDbContext<ChipsUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<TicketProgress> Updates { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Event> Events { get; set; }

        //public DbSet<Customer> MyProperty { get; set; }
        public ChipsDbContext(DbContextOptions<ChipsDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seed log event types (currently unused, eventually to replace LogType/ContactMethod enums and ServicePrice Entity)
            builder.Entity<Event>().HasData(
                // Open Ticket
                new Event { Id = (int)EventEnum.CheckIn, Name = "Check-In", Description = "Initial event when a device is brought in and a ticket is opened", Category = EventCategory.OpenTicket, Price = 0m },
                // Close Ticket
                new Event { Id = (int)EventEnum.CheckOut, Name = "Check-Out", Description = "Final event when a device returned to the customer", Category = EventCategory.CloseTicket, Price = 0m },
                new Event { Id = (int)EventEnum.LostandFound, Name = "Lost and Found", Description = "Final event when a device has been awaiting pickup for 30 days and is sent to Lost and Found", Category = EventCategory.CloseTicket, Price = 0m },
                // General Services
                new Event { Id = (int)EventEnum.Diagnostic, Name = "Diagnostic", Description = "Identification of the issue by inspecting device hardware and software", Category = EventCategory.GeneralService, Price = 50m },
                // Software Services
                new Event { Id = (int)EventEnum.DataBackup, Name = "Data Backup", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = (int)EventEnum.DataRestore, Name = "Data Restore", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = (int)EventEnum.OSInstallation, Name = "OS Installation", Description = "Installation of an OS on a blank drive or over an old OS, writing over existing user data", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = (int)EventEnum.OSUpdates, Name = "OS Updates", Description = "Incremental updates to an OS where user data is preserved", Category = EventCategory.SWService, Price = 50m },
                new Event { Id = (int)EventEnum.ProgramDriverInstallation, Name = "Program/Driver Installation", Category = EventCategory.SWService, Price = 50m },
                new Event { Id = (int)EventEnum.VirusMalwareRemoval, Name = "Virus/Malware Removal", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = (int)EventEnum.MiscSoftware, Name = "Misc. Software", Category = EventCategory.SWService, Price = 50m },
                // Hardware Services
                new Event { Id = (int)EventEnum.BatteryReplacement, Name = "Battery Replacement", Category = EventCategory.HWService, Price = 50m },
                new Event { Id = (int)EventEnum.DisplayReplacement, Name = "Display Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = (int)EventEnum.HingeReplacement, Name = "Hinge Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = (int)EventEnum.KeyboardReplacement, Name = "Keyboard Replacement", Category = EventCategory.HWService, Price = 125m },
                new Event { Id = (int)EventEnum.PowerJackReplacement, Name = "Power Jack Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = (int)EventEnum.RAMReplacement, Name = "RAM Replacement", Category = EventCategory.HWService, Price = 100m },
                new Event { Id = (int)EventEnum.StorageDriveReplacement, Name = "Storage Drive Replacement", Category = EventCategory.HWService, Price = 100m },
                new Event { Id = (int)EventEnum.TrackpadReplacement, Name = "Trackpad Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = (int)EventEnum.MiscHardware, Name = "Misc. Hardware", Category = EventCategory.HWService, Price = 100m },
                // Contact
                new Event { Id = (int)EventEnum.InPerson, Name = "In-Person", Category = EventCategory.Contact, Price = 0m },
                new Event { Id = (int)EventEnum.Email, Name = "Email", Category = EventCategory.Contact, Price = 0m },
                new Event { Id = (int)EventEnum.PhoneCall, Name = "Phone Call", Category = EventCategory.Contact, Price = 0m },
                new Event { Id = (int)EventEnum.Voicemail, Name = "Voicemail", Category = EventCategory.Contact, Price = 0m } 
            );

            // seed user roles
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                },
                new IdentityRole
                {
                    Name = "Technician",
                    NormalizedName = "TECHNICIAN"
                },
                new IdentityRole
                {
                    Name = "ReportReader",
                    NormalizedName = "REPORTREADER"
                }
            );

            // seed sample data
            SampleData.Seed(builder);
        }
    }
}
