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
        
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string NewRole { get; set; }
        public string RemoveRole { get; set; }
        public List<string> CurrentRoles { get; set; }



    }
}
