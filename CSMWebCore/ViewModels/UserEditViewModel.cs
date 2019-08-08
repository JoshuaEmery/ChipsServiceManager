using CSMWebCore.Entities;
using CSMWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.ViewModels
{
    public class UserEditViewModel
    {
        public ChipsUser User { get; set; }
        public List<string> Roles { get; set; }



    }
}
