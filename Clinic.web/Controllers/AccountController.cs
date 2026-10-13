using Clinic.Application.Features.RegisterUser.Commands.Models;
using Clinic.Domain.Entities.Identity;
using Clinic.web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

public sealed class AccountController(
    SignInManager<ApplicationUser> signInManager,
    IMediator mediator) : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginInputModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        LoginInputModel input,
        string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(input);

        var result = await signInManager.PasswordSignInAsync(
            input.Email,
            input.Password,
            input.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Signed in successfully.";

            if (!string.IsNullOrWhiteSpace(returnUrl) &&
                Url.IsLocalUrl(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(
            string.Empty,
            "Invalid email or password.");

        return View(input);
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        return View(new SignUpInputModel());
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpInputModel input)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
            return View(input);

        var command = new AdminRegisterCommand
        {
            Email = input.Email,
            Password = input.Password,
            FirstName = input.FirstName.Trim(),
            LastName = input.LastName.Trim()
        };

        var result = await mediator.Send(command);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(
                string.Empty,
                result.Message ?? "Account registration failed.");

            return View(input);
        }

        // The handler returns a JWT, but this MVC application
        // uses Identity cookies for browser authentication.
        // Sign in through Identity to establish the MVC session.
        var signInResult = await signInManager.PasswordSignInAsync(
            input.Email,
            input.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            TempData["SuccessMessage"] =
                "Account created successfully. Please sign in.";

            return RedirectToAction(nameof(Login));
        }

        TempData["SuccessMessage"] = "Account created successfully.";

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}
