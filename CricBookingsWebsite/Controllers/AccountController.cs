using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CricBookingsWebsite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CricBookingsWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = _mapper.Map<User>(userModel);

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return View(userModel);
            }

            // TODO - Change "Administrator" back to "Visitor"

            await _userManager.AddToRoleAsync(user, "User");

            await _signInManager.SignInAsync(user, isPersistent: true);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var result = await _signInManager.PasswordSignInAsync(userModel.Email, userModel.Password, userModel.RememberMe, false);

            

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }

            else
            {
                ModelState.AddModelError("", "Invalid Username or Password");
                return View();
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //public async Task<IActionResult> AdminIndex()
        //{
        //    var admins = (await _userManager
        //        .GetUsersInRoleAsync("Administrator"))
        //        .ToArray();

        //    var visitors = await _userManager.Users.ToArrayAsync();

        //    var model = new ManageUsersViewModel
        //    {
        //        Administrators = admins,

        //        Visitors = visitors
        //    };

        //    return View(model);
        //}

        
    }
}
