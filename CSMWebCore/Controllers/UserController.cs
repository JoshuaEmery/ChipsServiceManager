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
    //This controller enables the ChipsUser : IdentityUser to login and logout
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ChipsUser> _userManager;
        public UserController(UserManager<ChipsUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: User/Index
        //gets a list of all users regardless of whether account is active or not
        public IActionResult Index()
        {
            return View(_userManager.Users.Cast<ChipsUser>().ToList());
        }
        //gets a list of all active ChipsUsers and sends to View,
        //Admin or high will also have an edit button by each user
        public IActionResult ActiveUsers()
        {
            return View("Index",_userManager.Users.Cast<ChipsUser>().Where(x => x.Active == true).ToList());
        }
        //Method currently not used, could be implemented later to look up ChipsUser contact info etc...
        //public IActionResult Details(string userId)
        //{
        //    return View(_userManager.FindByIdAsync(userId));
        //}
        //User/Edit
        //
        [HttpGet]
        [Authorize (Roles = "Administrator,Director,Supervisor")]
        //Get for Edit User, This could be expanded on in the future to allow administrators to change contact information
        //however for now it is primarily for granting or removing administrator access from accounts
        public async Task<IActionResult> Edit(string userId)
        {
            //get user from usermanager
            var user =  await _userManager.FindByIdAsync(userId);
            //get list of roles from usermanager
            var roles = await _userManager.GetRolesAsync(user);
            //GetRoles returns a iList which I am guessing I had trouble working with so i read the contents into
            //a regular list and sent that to the viewModel
            List<string> rolesList = new List<string>();
            foreach (var r in roles)
            {
                rolesList.Add(r);
            }
            //create UserEditViewModel
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
            //return
            return View(model);
        }
        [HttpPost]
        //user edit post
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            //get user accont and roles
            var user = await _userManager.FindByIdAsync(model.Id);
            var roles = await _userManager.GetRolesAsync(user);
            //This is kind of a weird solution, not sure where I got this but
            //in order to check and see if a new role was assigned I created
            //an int variable and then tried to parse NewRole to that int.  If the tryparse
            //fails then no new role was selected
            int newRoleInt;            
            if(int.TryParse(model.NewRole, out newRoleInt))
            {
                //creating a newRole object by casting the int
                //to a UserRoles enum
                UserRoles newRole = (UserRoles)newRoleInt;
                //if the user does not already have this roll
                if (!roles.Contains(newRole.ToString()))
                {
                    //add it
                    await _userManager.AddToRoleAsync(user, newRole.ToString());
                }
            }
            //similar solution for removing a role
            int removeRoleInt;
            if (int.TryParse(model.RemoveRole, out removeRoleInt))
            {
                UserRoles removeRole = (UserRoles)removeRoleInt;
                if (roles.Contains(removeRole.ToString()))
                {
                    await _userManager.RemoveFromRoleAsync(user, removeRole.ToString());
                }
            }
            //update email and active
            user.Email = model.Email;
            user.Active = model.Active;
            //update user and return to users/Index
            await _userManager.UpdateAsync(user);
            return View("Index", _userManager.Users.Cast<ChipsUser>().ToList());
        }
    }
}