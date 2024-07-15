using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using PhotoGallery.Models;
using PhotoGallery.Services;

namespace PhotoGallery.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public IActionResult Login()
        {
            // Check if the user is already logged in
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Photo");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel userViewModel)
        {
            try
            {
            UserModel userModel = null;
            userModel = _loginService.Login(userViewModel);

            // Here use a query to a database for users
            if (userViewModel.UserName == userModel.Email && userViewModel.Password == userModel.Password)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userModel.Email),
                    new Claim(ClaimTypes.Name, userModel.UserName),  // This line adds the user's name as a claim
                    new Claim(ClaimTypes.Email, userModel.Email),  // This line adds the user's email as a claim
                    new Claim(ClaimTypes.Role, userModel.UserRole),
                    new Claim("OtherProperties", "Example Role")
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = userViewModel.KeepLoggedIn
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);

                return RedirectToAction("Index", "Photo");
            }
            ViewData["ValidateMessage"] = "User not found";
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
