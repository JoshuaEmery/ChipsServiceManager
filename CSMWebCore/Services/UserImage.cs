using CSMWebCore.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class UserImage : Controller
    {
        private readonly UserManager<ChipsUser> _userManager;
        private readonly SignInManager<ChipsUser> _signInManager;
        private IImageConverter _imageConverter;

        public UserImage(UserManager<ChipsUser> userManager, SignInManager<ChipsUser> signInManager,
            IImageConverter imageConverter)
        {
            _imageConverter = imageConverter;
            _signInManager = signInManager;
            _userManager = userManager;            
        }
        public async Task<ChipsUser> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            return (user);
        }
        public async Task<Image> GetUserImage()
        {
            var user = await _userManager.GetUserAsync(User);
            return _imageConverter.byteArrayToImage(user.Avatar);
        }
 
    }
}
