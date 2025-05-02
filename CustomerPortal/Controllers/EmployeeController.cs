using CustomerPortal.Data;
using CustomerPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(EmployeeLoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var employee = _context.Employees.FirstOrDefault(e => e.Username == model.Username);
        if (employee == null)
        {
            ModelState.AddModelError("", "Invalid credentials.");
            return View(model);
        }

        var inputHash = SHA256.HashData(Encoding.UTF8.GetBytes(model.Password).Concat(employee.Salt).ToArray());
        if (Convert.ToBase64String(inputHash) != employee.PasswordHash)
        {
            ModelState.AddModelError("", "Invalid credentials.");
            return View(model);
        }

        HttpContext.Session.SetString("EmployeeUsername", employee.Username);
        return RedirectToAction("Dashboard");
    }


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("EmployeeUsername") == null)
            return RedirectToAction("Login");

        var payments = _context.Payments
            .OrderByDescending(p => p.SubmittedAt)
            .ToList();

        return View(payments);
    }

    [HttpPost]
    public IActionResult Verify(int id)
    {
        var payment = _context.Payments.Find(id);
        if (payment != null)
        {
            payment.IsVerified = true;
            payment.VerifiedAt = DateTime.Now;
            _context.SaveChanges();
        }

        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    public IActionResult SubmitToSwift()
    {
        var verified = _context.Payments
            .Where(p => p.IsVerified && p.VerifiedAt != null)
            .ToList();

        // In real system, you'd send these to SWIFT.
        TempData["Success"] = $"{verified.Count} payment(s) submitted to SWIFT.";

        return RedirectToAction("Dashboard");
    }
}
