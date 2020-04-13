using System;
using System.Collections.Generic;
using System.Text;

using CSMWebCore.Controllers;
using CSMWebCore.Entities;
using CSMWebCore.Models;
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
        public DbSet<TicketHistory> TicketsHistory { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<TicketProgress> Updates { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<ServicePrice> ServicePrices { get; set; }
        public DbSet<Event> Events { get; set; }

        //public DbSet<Customer> MyProperty { get; set; }
        public ChipsDbContext(DbContextOptions<ChipsDbContext> options)
            : base(options)
        {
        }

        private IDeviceRepository devices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seed initial services
            builder.Entity<ServicePrice>().HasData(
                new ServicePrice { Id = 1, Service = LogType.CheckIn, Price = 0m },
                new ServicePrice { Id = 2, Service = LogType.Contact, Price = 0m },
                new ServicePrice { Id = 3, Service = LogType.Checkout, Price = 0m },
                new ServicePrice { Id = 4, Service = LogType.Diagnostic, Price = 50m },
                new ServicePrice { Id = 5, Service = LogType.OsInstallation, Price = 150m },
                new ServicePrice { Id = 6, Service = LogType.OsUpdate, Price = 50m },
                new ServicePrice { Id = 7, Service = LogType.DataBackupRestore, Price = 150m },
                new ServicePrice { Id = 8, Service = LogType.SoftwareInstallation, Price = 50m },
                new ServicePrice { Id = 9, Service = LogType.DriveInstallation, Price = 100m },
                new ServicePrice { Id = 10, Service = LogType.RamInstallation, Price = 100m },
                new ServicePrice { Id = 11, Service = LogType.ScreenReplacement, Price = 150m },
                new ServicePrice { Id = 12, Service = LogType.KeyboardReplacement, Price = 125m },
                new ServicePrice { Id = 13, Service = LogType.TouchpadReplacement, Price = 150m },
                new ServicePrice { Id = 14, Service = LogType.HingeRepair, Price = 150m },
                new ServicePrice { Id = 15, Service = LogType.VirusRemoval, Price = 150m },
                new ServicePrice { Id = 16, Service = LogType.BatteryReplacment, Price = 50m },
                new ServicePrice { Id = 17, Service = LogType.PowerJackReplacement, Price = 150m },
                new ServicePrice { Id = 18, Service = LogType.MiscRepair, Price = 100m });

            // seed log event types (currently unused, eventually to replace LogType/ContactMethod enums and ServicePrice Entity)
            builder.Entity<Event>().HasData(
                // Open Ticket
                new Event { Id = 1, Name = EventName.CheckIn, Description = "Initial event when a device is brought in and a ticket is opened", Category = EventCategory.OpenTicket, Price = 0m },
                // Close Ticket
                new Event { Id = 2, Name = EventName.CheckOut, Description = "Final event when a device returned to the customer", Category = EventCategory.CloseTicket, Price = 0m },
                new Event { Id = 3, Name = EventName.LostandFound, Description = "Final event when a device has been awaiting pickup for 30 days and is sent to Lost and Found", Category = EventCategory.CloseTicket, Price = 0m },
                // General Services
                new Event { Id = 4, Name = EventName.Diagnostic, Description = "Identification of the issue by inspecting device hardware and software", Category = EventCategory.GeneralService, Price = 50m },
                // Software Services
                new Event { Id = 5, Name = EventName.DataBackup, Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 6, Name = EventName.DataRestore, Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 7, Name = EventName.OSInstallation, Description = "Installation of an OS on a blank drive or over an old OS, writing over existing user data", Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 8, Name = EventName.OSUpdates, Description = "Incremental updates to an OS where user data is preserved", Category = EventCategory.SWService, Price = 50m },
                new Event { Id = 9, Name = EventName.ProgramDriverInstallation, Category = EventCategory.SWService, Price = 50m },
                new Event { Id = 10, Name = EventName.VirusMalwareRemoval, Category = EventCategory.SWService, Price = 150m },
                new Event { Id = 11, Name = EventName.MiscSoftware, Category = EventCategory.SWService, Price = 50m },
                // Hardware Services
                new Event { Id = 12, Name = EventName.BatteryReplacement, Category = EventCategory.HWService, Price = 50m },
                new Event { Id = 13, Name = EventName.DisplayReplacement, Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 14, Name = EventName.HingeReplacement, Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 15, Name = EventName.KeyboardReplacement, Category = EventCategory.HWService, Price = 125m },
                new Event { Id = 16, Name = EventName.PowerJackReplacement, Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 17, Name = EventName.RAMReplacement, Category = EventCategory.HWService, Price = 100m },
                new Event { Id = 18, Name = EventName.StorageDriveReplacement, Category = EventCategory.HWService, Price = 100m },
                new Event { Id = 19, Name = EventName.TrackpadReplacement, Category = EventCategory.HWService, Price = 150m },
                new Event { Id = 20, Name = EventName.MiscHardware, Category = EventCategory.HWService, Price = 100m },
                // Contact
                new Event { Id = 21, Name = EventName.InPerson, Category = EventCategory.Contact, Price = 0m },
                new Event { Id = 22, Name = EventName.Email, Category = EventCategory.Contact, Price = 0m },
                new Event { Id = 23, Name = EventName.PhoneCall, Category = EventCategory.Contact, Price = 0m },
                new Event { Id = 24, Name = EventName.Voicemail, Category = EventCategory.Contact, Price = 0m }
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
        public DbSet<CSMWebCore.ViewModels.TicketDeviceCustCreateVM> TicketDeviceCustCreateVM { get; set; }
    }
}
