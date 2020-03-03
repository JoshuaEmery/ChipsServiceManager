using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Models;
using CSMWebCore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CSMWebCore.Controllers
{
    [Authorize]
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
        public IActionResult ActiveUsers()
        {
            return View("Index",_userManager.Users.Cast<ChipsUser>().Where(x => x.Active == true).ToList());
        }
        public IActionResult Details(string userId)
        {
            return View(_userManager.FindByIdAsync(userId));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string userId)
        {
            //var users = _userManager.Users.Cast<ChipsUser>().ToList();
            //List<UserEditViewModel> model = new List<UserEditViewModel>();
            //foreach (ChipsUser user in users)
            //{
            //    var roles = await _userManager.GetRolesAsync(user);
            //    List<string> rolesList = new List<string>();
            //    foreach (var r in roles)
            //    {
            //        rolesList.Add(r);
            //    }
            //    model.Add(new UserEditViewModel
            //    {
            //        User = user,
            //        Roles = rolesList,
            //    });
            //}
            var user =  await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            List<string> rolesList = new List<string>();
            foreach (var r in roles)
            {
                rolesList.Add(r);
            }
            UserEditViewModel model = new UserEditViewModel
            {
                Id = user.Id,
                Active = user.Active,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Email = user.Email,
                CurrentRoles = rolesList               
            };            
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var roles = await _userManager.GetRolesAsync(user);
            int newRoleInt;
            int removeRoleInt;
            if(int.TryParse(model.NewRole, out newRoleInt))
            {
                UserRoles newRole = (UserRoles)newRoleInt;
                if (!roles.Contains(newRole.ToString()))
                {
                    await _userManager.AddToRoleAsync(user, newRole.ToString());
                }
            }
            if (int.TryParse(model.RemoveRole, out removeRoleInt))
            {
                UserRoles removeRole = (UserRoles)removeRoleInt;
                if (roles.Contains(removeRole.ToString()))
                {
                    await _userManager.RemoveFromRoleAsync(user, removeRole.ToString());
                }
            }
            user.Email = model.Email;
            user.Active = model.Active;
            await _userManager.UpdateAsync(user);
            return View("Index", _userManager.Users.Cast<ChipsUser>().ToList());
        }
    }
}