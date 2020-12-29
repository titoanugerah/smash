using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Suscribe_Management_System_Hehe.Controllers
{
    [AllowAnonymous, Route("Account")]
    public class AccountController : Controller
    {
        private readonly ILoggerFactory _loggger;
        private readonly IConfiguration _configuration;
        private readonly DataAccess.DatabaseContext _DBContext;
        public AccountController(ILoggerFactory loggerFactory, IConfiguration configuration, DataAccess.DatabaseContext DBContext)
        {
            this._loggger = loggerFactory;
            this._configuration = configuration;
            this._DBContext = DBContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("Validate") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

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
                    .Where(user => user.Email == userIdentity.Email)
                    .FirstOrDefault();
                user.Image = userIdentity.Picture;
                user.Name = userIdentity.Name;
                await _DBContext.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name),
                    new Claim("Image", user.Image),
                    new Claim("RoleId", user.Role.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Home")
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            }
            return Json("User Not Registered");
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
