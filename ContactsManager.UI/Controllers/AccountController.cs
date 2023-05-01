using ContactsManager.Core.Domain.IdentityEntities;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ContactsManager.Core.Enums;
using UserOptions = ContactsManager.Core.Enums.UserOptions;

namespace ContactsManager.UI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [Route("account/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("account/register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
            };

            IdentityResult result = 
                await _userManager.CreateAsync(user, registerDTO.Password);

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);

                if (registerDTO.Options == UserOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserOptions.Admin.ToString()) is null)
                    {
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = UserOptions.Admin.ToString()
                        };

                        await _roleManager.CreateAsync(role);
                    }

                    await _userManager.AddToRoleAsync(user, UserOptions.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserOptions.Customer.ToString()) is null)
                    {
                        ApplicationRole role = new ApplicationRole()
                        {
                            Name = UserOptions.Customer.ToString()
                        };

                        await _roleManager.CreateAsync(role);
                    }

                    await _userManager.AddToRoleAsync(user, UserOptions.Customer.ToString());
                }
                return RedirectToAction("Index", "Person");
            }
            else
            {
                ViewBag.Errors = result.Errors.Select(error => error.Description + "\n");
                return View(registerDTO);
            }
        }

        [Route("account/login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("account/login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if(!ModelState.IsValid)
            {
                return View(loginDTO);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, 
                isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }

                ApplicationUser user = await _userManager.FindByNameAsync(loginDTO.Email);

                if (await _userManager.IsInRoleAsync(user, UserOptions.Admin.ToString()))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                return RedirectToAction("Index", "Persons");
            }
            else
            {
                ViewBag.Errors = "Invalid email or password.";
                return View(loginDTO);
            }
        }

        [Route("account/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Person");
        }
    }
}
