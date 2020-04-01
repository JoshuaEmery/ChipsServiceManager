using System;
using System.Collections.Generic;
using System.Text;

using CSMWebCore.Controllers;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSMWebCore.Data
{
    public class ChipsDbContext : IdentityDbContext<ChipsUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketHistory> TicketsHistory { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<ServicePrice> ServicePrices { get; set; }


        //public DbSet<Customer> MyProperty { get; set; }
        public ChipsDbContext(DbContextOptions<ChipsDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
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
            base.OnModelCreating(builder);
        }
    }
}
