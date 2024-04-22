using PasechnikovaPR33p19.Domain.Entities;
using PasechnikovaPR33p19.Domain.Services;
using PasechnikovaPR33p19.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace PasechnikovaPR33p19.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        private const int adminRoleId = 2;
        private const int clientRoleId = 1;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult RegistrationSuccess()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel registration)
        {
            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            if (await userService.IsUserExistsAsync(registration.Username))
            {
                ModelState.AddModelError("user_exists", $"Имя пользователя {registration.Username} уже существует!");
                return View(registration);
            }


            try
            {
                await userService.RegistrationAsync(registration.Fullname, registration.Username, registration.Password);
                return RedirectToAction("RegistrationSuccess", "User");
            }
            catch
            {
                ModelState.AddModelError("reg_error", $"Не удалось зарегистрироваться, попробуйте попытку регистрации позже");
                return View(registration);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userService.GetUserAsync(loginViewModel.Username, loginViewModel.Password);
                if (user != null)
                {
                    await SignIn(user, returnUrl); // Передаем returnUrl в метод SignIn
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");
                }
            }
            // После аутентификации пользователь будет перенаправлен на указанный URL, если он был передан, или на главную страницу
            return RedirectToAction("Index", "Books");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "User");
        }

        private async Task SignIn(User user, string returnUrl)
        {
            string role = user.RoleId switch
            {
                adminRoleId => "admin",
                clientRoleId => "client",
                _ => throw new ApplicationException("invalid user role")
            };

            List<Claim> claims = new List<Claim>
    {
        new Claim("fullname", user.Fullname),
        new Claim("id", user.Id.ToString()),
        new Claim("role", role),
        new Claim("username", user.Login)
    };
            string authType = CookieAuthenticationDefaults.AuthenticationScheme;
            IIdentity identity = new ClaimsIdentity(claims, authType, "username", "role");
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }
    }
}
