using Clinic.web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.web.Controllers;

public sealed class ContactController : Controller
{
    [HttpGet]
    public IActionResult Index() => View(new ContactInputModel());

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Index(ContactInputModel input)
    {
        if (!ModelState.IsValid) return View(input);
        TempData["SuccessMessage"] = "Thank you for contacting us. Our team will reply as soon as possible.";
        return RedirectToAction(nameof(Index));
    }
}
