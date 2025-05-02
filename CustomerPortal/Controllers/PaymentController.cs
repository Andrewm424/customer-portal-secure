using CustomerPortal.Data;
using CustomerPortal.Models;
using Microsoft.AspNetCore.Mvc;

public class PaymentController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaymentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult MakePayment()
    {
        // Check if customer is logged in using correct key
        if (HttpContext.Session.GetString("CustomerUsername") == null)
            return RedirectToAction("Login", "Account");

        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MakePayment(Payment model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
        model.CustomerId = customerId;

        _context.Payments.Add(model);
        _context.SaveChanges();

        // Dynamically assign currency symbol
        string symbol = model.Currency switch
        {
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            "ZAR" => "R",
            _ => ""
        };

        TempData["Success"] = $"Payment of {symbol}{model.Amount:N2} to {model.PayeeAccount} via {model.Provider} submitted!";
        ModelState.Clear();

        return View();
    }


    public IActionResult History()
    {
        var customerId = HttpContext.Session.GetInt32("CustomerId") ?? 0;
        var payments = _context.Payments
            .Where(p => p.CustomerId == customerId)
            .OrderByDescending(p => p.SubmittedAt)
            .ToList();

        return View(payments);
    }
}
