﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;

namespace CSMWebCore.Services
{
    public class SqlDeviceData : IDeviceData
    {
        private ChipsDbContext _db;
        public SqlDeviceData(ChipsDbContext db)
        {
            _db = db;
        }
        public void Add(Device device)
        {
            _db.Add(device);
        }

        public int Commit()
        {
           return _db.SaveChanges();
        }

        public Device Get(int id)
        {
            return _db.Find<Device>(id);
        }

        public IEnumerable<Device> GetAll()
        {
            return _db.Devices;
        }
    }
}
