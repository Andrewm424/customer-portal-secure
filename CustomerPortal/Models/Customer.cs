using System.ComponentModel.DataAnnotations;

public class Customer
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "ID number must be 13 digits.")]
    public string IdNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{10,12}$")]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public byte[] Salt { get; set; } = Array.Empty<byte>();
}
