﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Suscribe_Management_System_Hehe.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly ILogger _loggger;
        private readonly IConfiguration _configuration;
        private readonly DataAccess.DatabaseContext _DBContext;
        public AccountController(ILogger<AccountController> logger, IConfiguration configuration, DataAccess.DatabaseContext DBContext)
        {
            this._loggger = logger;
            this._configuration = configuration;
            this._DBContext = DBContext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.Claims.Where(x => x.Type == "Id").Select(x => x.Value).FirstOrDefault());
            ViewData["User"] = _DBContext.User
                .Include(table => table.Role)
                .Where(column => column.Id == userId)
                .FirstOrDefaultAsync();
            return View();
        }

        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login()
        {
          var properties = new AuthenticationProperties 
          {
              AllowRefresh = true,
              IsPersistent = true,
              ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
              RedirectUri = Url.Action("Validate") 
          };
          return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [Route("Validate")]
        public async Task<IActionResult> Validate()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            List<ViewModels.UserClaim> userClaim = result.Principal
                .Identities.FirstOrDefault().Claims
                .Where(claim => claim.Type == "email" || claim.Type == "name" || claim.Type == "picture")
                .Select(claim => new ViewModels.UserClaim{
                    Type = claim.Type,
                    Value = claim.Value
                })
                .ToList();
            ViewModels.UserIdentity userIdentity = new ViewModels.UserIdentity();
            userIdentity.Email = userClaim.Where(claim => claim.Type == "email").Select(claim => claim.Value).FirstOrDefault();
            userIdentity.Picture= userClaim.Where(claim => claim.Type == "picture").Select(claim => claim.Value).FirstOrDefault();
            userIdentity.Name = userClaim.Where(claim => claim.Type == "name").Select(claim => claim.Value).FirstOrDefault();
            var checkUser = _DBContext.User
                .Any(user => user.Email == userIdentity.Email);
            if (checkUser)
            {
                var user = _DBContext.User
                    .Include(table => table.Role)
                    .Where(user => user.Email == userIdentity.Email)
                    .FirstOrDefault();
                user.Image = userIdentity.Picture;
                user.Name = userIdentity.Name;
                await _DBContext.SaveChangesAsync();
                var claims = new List<Claim>
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Name", user.Name),
                    new Claim("Email", user.Email),
                    new Claim("Role", user.Role.Name),
                    new Claim("Image", user.Image),
                    new Claim("RoleId", user.RoleId.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties();
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Home");
            }
            return Json("User Not Registered");
        }

        [Authorize]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
