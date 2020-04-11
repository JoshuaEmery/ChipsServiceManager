using System;
using System.Collections.Generic;
using System.Text;

using CSMWebCore.Controllers;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSMWebCore.Data
{
    public class ChipsDbContext : IdentityDbContext<ChipsUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<XTicketHistory> TicketsHistory { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<XUpdate> Updates { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<XServicePrice> ServicePrices { get; set; }
        public DbSet<Event> Events { get; set; }

        //public DbSet<Customer> MyProperty { get; set; }
        public ChipsDbContext(DbContextOptions<ChipsDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seed initial services
            builder.Entity<XServicePrice>().HasData(
                new XServicePrice { Id = 1, Service = LogType.CheckIn, Price = 0m },
                new XServicePrice { Id = 2, Service = LogType.Contact, Price = 0m },
                new XServicePrice { Id = 3, Service = LogType.Checkout, Price = 0m },
                new XServicePrice { Id = 4, Service = LogType.Diagnostic, Price = 50m },
                new XServicePrice { Id = 5, Service = LogType.OsInstallation, Price = 150m },
                new XServicePrice { Id = 6, Service = LogType.OsUpdate, Price = 50m },
                new XServicePrice { Id = 7, Service = LogType.DataBackupRestore, Price = 150m },
                new XServicePrice { Id = 8, Service = LogType.SoftwareInstallation, Price = 50m },
                new XServicePrice { Id = 9, Service = LogType.DriveInstallation, Price = 100m },
                new XServicePrice { Id = 10, Service = LogType.RamInstallation, Price = 100m },
                new XServicePrice { Id = 11, Service = LogType.ScreenReplacement, Price = 150m },
                new XServicePrice { Id = 12, Service = LogType.KeyboardReplacement, Price = 125m },
                new XServicePrice { Id = 13, Service = LogType.TouchpadReplacement, Price = 150m },
                new XServicePrice { Id = 14, Service = LogType.HingeRepair, Price = 150m },
                new XServicePrice { Id = 15, Service = LogType.VirusRemoval, Price = 150m },
                new XServicePrice { Id = 16, Service = LogType.BatteryReplacment, Price = 50m },
                new XServicePrice { Id = 17, Service = LogType.PowerJackReplacement, Price = 150m },
                new XServicePrice { Id = 18, Service = LogType.MiscRepair, Price = 100m });

            // seed log event types (currently unused, eventually to replace LogType/ContactMethod enums and ServicePrice Entity)
            builder.Entity<Event>().HasData(
                // Open Ticket
                new Event { Id = 1, Name = "Check-In", Description = "Initial event when a device is brought in and a ticket is opened", Category = EventCategory.OpenTicket, Price = 0m },
                // Close Ticket
                new Event { Id = 2, Name = "Check-Out", Description = "Final event when a device returned to the customer", Category = EventCategory.CloseTicket, Price = 0m },
                new Event { Id = 3, Name = "Lost and Found", Description = "Final event when a device has been awaiting pickup for 30 days and is sent to Lost and Found", Category = EventCategory.CloseTicket, Price = 0m },
                // General Services
                new Event { Id = 4, Name = "Diagnostic", Description = "Identification of the issue by inspecting device hardware and software", Category = EventCategory.GeneralService, Price = 50m },
                // Software Services
                new Event { Id = 5, Name = "Data Backup", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 6, Name = "Data Restore", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 7, Name = "OS Installation", Description = "Installation of an OS on a blank drive or over an old OS, writing over existing user data", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 8, Name = "OS Updates", Description = "Incremental updates to an OS where user data is preserved", Category = EventCategory.SWService, Price = 50m },
                new Event { Id = 9, Name = "Program/Driver Installation", Category = EventCategory.SWService, Price = 50m },
                new Event { Id = 10, Name = "Virus/Malware Removal", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 11, Name = "Other", Category = EventCategory.SWService, Price = 50m },
                // Hardware Services
                new Event { Id = 12, Name = "Battery Replacement", Category = EventCategory.HWService, Price = 50m },
                new Event { Id = 13, Name = "Display Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 14, Name = "Hinge Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 15, Name = "Keyboard Replacement", Category = EventCategory.HWService, Price = 125m },
                new Event { Id = 16, Name = "Power Jack Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 17, Name = "RAM Replacement", Category = EventCategory.HWService, Price = 100m },
                new Event { Id = 18, Name = "Storage Drive Replacement", Category = EventCategory.HWService, Price = 100m },
                new Event { Id = 19, Name = "Trackpad Replacement", Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 20, Name = "Other", Category = EventCategory.HWService, Price = 100m },
                // Contact
                new Event { Id = 21, Name = "In-Person", Category = EventCategory.Contact, Price = 0m },
                new Event { Id = 22, Name = "Email", Category = EventCategory.Contact, Price = 0m },
                new Event { Id = 23, Name = "Phone Call", Category = EventCategory.Contact, Price = 0m },
                new Event { Id = 24, Name = "Voicemail", Category = EventCategory.Contact, Price = 0m }
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
                    Name = "ReadOnly",
                    NormalizedName = "READONLY"
                }
            );

            // seed sample data
            SampleData.Seed(builder);
        }
    }
}
