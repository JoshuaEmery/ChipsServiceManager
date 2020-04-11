﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMWebCore.Entities
{
    public class Consultation : Entity
    {
        public string UserName { get; set; }
        public DateTime Time { get; set; }
    }
}
