using CustomerPortal.Data;
using CustomerPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;

    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        // Generate salt
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        // Hash password with salt
        var combined = Encoding.UTF8.GetBytes(model.Password).Concat(salt).ToArray();
        var hash = SHA256.HashData(combined);

        // Create customer entity
        var customer = new Customer
        {
            FullName = model.FullName,
            IdNumber = model.IdNumber,
            AccountNumber = model.AccountNumber,
            Username = model.Username,
            PasswordHash = Convert.ToBase64String(hash),
            Salt = salt
        };

        // Save to database
        _context.Customers.Add(customer);
        _context.SaveChanges();

        TempData["Success"] = "Registration successful! Please log in.";
        return RedirectToAction("Login");
    }


    [HttpGet]
    public IActionResult Login()
    {
        // If already logged in, redirect to payment dashboard
        if (HttpContext.Session.GetInt32("CustomerId") != null)
        {
            return RedirectToAction("MakePayment", "Payment");
        }

        return View();
    }






    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var customer = _context.Customers
            .FirstOrDefault(c => c.Username == model.Username && c.AccountNumber == model.AccountNumber);

        if (customer == null)
        {
            ModelState.AddModelError("", "Invalid credentials.");
            return View(model);
        }

        var combined = Encoding.UTF8.GetBytes(model.Password).Concat(customer.Salt).ToArray();
        var hash = SHA256.HashData(combined);
        var enteredHash = Convert.ToBase64String(hash);

        if (enteredHash != customer.PasswordHash)
        {
            ModelState.AddModelError("", "Incorrect password.");
            return View(model);
        }

        // Save to session
        HttpContext.Session.SetString("CustomerUsername", customer.Username);
        HttpContext.Session.SetInt32("CustomerId", customer.Id);

        TempData["Success"] = $"Welcome back, {customer.FullName}!";
        return RedirectToAction("MakePayment", "Payment");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Account");
    }
}
