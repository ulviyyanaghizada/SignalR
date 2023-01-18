using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;
using Pronia.Models.ViewModels;
using Pronia.Utilies.Enums;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {

        UserManager<AppUser> _userManager { get; }
        SignInManager<AppUser> _signInManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(registerVM.UserName);
            if (user!= null)
            {
                ModelState.AddModelError("UserName","This User already exists");
                return View();
            }
            user = new AppUser 
            { 
                FirstName=registerVM.Name,
                LastName=registerVM.Surname,
                Email=registerVM.Email,
                UserName=registerVM.UserName
                
            };
            IdentityResult result =  await _userManager.CreateAsync(user,registerVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("",item.Description);
                }
                return View();
            }
            await _signInManager.SignInAsync(user,true);
            return RedirectToAction("Index","Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVM loginVM,string? ReturnUrl)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("","Login or Password Wrong!");
                    return View();
                }
            }
            
            var result = await _signInManager.PasswordSignInAsync(user,loginVM.Password,loginVM.IsPersistance,true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("","Login or Password Wrong!");
                return View();
            }
            if (ReturnUrl == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(ReturnUrl);
            }

            return RedirectToAction("Index","Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
            public async Task<IActionResult> AddRoles()
            {
                foreach (var item in Enum.GetValues(typeof(Roles)))
                {
                    if (!await _roleManager.RoleExistsAsync(item.ToString()))
                    {
                        await _roleManager.CreateAsync(new IdentityRole { Name = item.ToString() });
                    }
                }
                return View();
            }
            public async Task<IActionResult> Test()
            {
                var user = await _userManager.FindByNameAsync("Ulviyyan1");
                await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
                user = await _userManager.FindByNameAsync("Admin");
                await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                return View();
            }
        

    }
}
