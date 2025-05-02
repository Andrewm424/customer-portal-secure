using CustomerPortal.Data;
using CustomerPortal.Models;
using System.Security.Cryptography;
using System.Text;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (!context.Employees.Any())
        {
            var salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            var password = "employee123";
            var combined = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
            var hash = SHA256.HashData(combined);

            var employee = new Employee
            {
                Username = "admin",
                PasswordHash = Convert.ToBase64String(hash),
                Salt = salt
            };

            context.Employees.Add(employee);
            context.SaveChanges();
        }
    }
}
