using System;
using System.Collections.Generic;
using System.Text;
using CSMWebCore.Controllers;
using CSMWebCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CSMWebCore.Data
{
    public class ChipsDbContext : IdentityDbContext<ChipsUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Device> Devices { get; set; }


        //public DbSet<Customer> MyProperty { get; set; }
        public ChipsDbContext(DbContextOptions<ChipsDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
