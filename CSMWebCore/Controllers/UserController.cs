using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        //public IActionResult Details(string userId)
        //{
        //    return View(_userManager.FindByIdAsync(userId));
        //}
        //[HttpGet]
        //public async Task<IActionResult> Edit(ChipsUser user)
        //{
        //    //var users = _userManager.Users.Cast<ChipsUser>().ToList();
        //    //List<UserEditViewModel> model = new List<UserEditViewModel>();
        //    //foreach (ChipsUser user in users)
        //    //{
        //    //    var roles = await _userManager.GetRolesAsync(user);
        //    //    List<string> rolesList = new List<string>();
        //    //    foreach (var r in roles)
        //    //    {
        //    //        rolesList.Add(r);
        //    //    }
        //    //    model.Add(new UserEditViewModel
        //    //    {
        //    //        User = user,
        //    //        Roles = rolesList,
        //    //    });
        //    //}
            
        //    var roles = await _userManager.GetRolesAsync(user);
        //    List<string> rolesList = new List<string>();
        //    foreach (var r in roles)
        //    {
        //        rolesList.Add(r);
        //    }
        //    UserEditViewModel model = new UserEditViewModel
        //    {
        //        User = user,
        //        Roles = rolesList
        //    };
            

        //    return View(model);
        //}
    }
}