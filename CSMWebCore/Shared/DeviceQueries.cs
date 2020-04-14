using CSMWebCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Shared
{
    public static class DeviceQueries
    {
        // ================ Devices ================

        /// <summary>
        /// Gets a collection of Devices for a given Customer ID.
        /// </summary>
        public static IQueryable<Device> GetDevicesByCustId(this DbSet<Device> dbSet, int customerId) =>
            dbSet.Where(x => x.Customer.Id == customerId);

        /// <summary>
        /// Searches relevant Device fields for a matching search value and
        /// returns a collection of matching Devices.
        /// </summary>
        public static List<Device> Search(this DbSet<Device> dbSet, string searchValue)
        {
            var result = new List<Device>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                result.AddRange(dbSet.Where(d => d.Make.Contains(searchValue)));
                result.AddRange(dbSet.Where(d => d.ModelNumber.Contains(searchValue)));
                result.AddRange(dbSet.Where(d => d.OperatingSystem.Contains(searchValue)));
                result.AddRange(dbSet.Where(d => d.Password.Contains(searchValue)));
            }
            return result;
        }
    }
}
