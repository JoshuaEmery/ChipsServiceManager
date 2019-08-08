using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSMWebCore.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ChipsUser> _userManager;
        public UserController(UserManager<ChipsUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: Administrator/ApplicationUsers
        public IActionResult Index()
        {
            return View(_userManager.Users.Cast<ChipsUser>().ToList());
        }
        public IActionResult Details(string userId)
        {
            return View(_userManager.FindByIdAsync(userId));
        }
    }
}