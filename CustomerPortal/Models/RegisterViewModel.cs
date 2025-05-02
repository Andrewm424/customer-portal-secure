using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{13}$")]
    public string IdNumber { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{10,12}$")]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
