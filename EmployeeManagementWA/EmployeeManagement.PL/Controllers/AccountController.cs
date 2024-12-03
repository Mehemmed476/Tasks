using EmployeeManagement.BL.Services.Abstractions;
using EmployeeManagement.CORE.DTOs.CreateUserDTOs;
using EmployeeManagement.CORE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.PL.Controllers;

public class AccountController : Controller
{
    private readonly IGenericCRUDService _service;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    
    public AccountController(IGenericCRUDService service, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _service = service;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }

        AppUser user = new();
        user.FirstName = registerDto.FirstName;
        user.LastName = registerDto.LastName;
        user.Email = registerDto.Email;
        user.UserName = registerDto.NickName;
        var result = await _userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
            return View(registerDto);
        }
        
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View();
        }
        AppUser? user = await _userManager.FindByEmailAsync(loginDto.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(loginDto.UsernameOrEmail);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(); 
            }
        }
        
        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, true);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View(); 
        }
        
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}