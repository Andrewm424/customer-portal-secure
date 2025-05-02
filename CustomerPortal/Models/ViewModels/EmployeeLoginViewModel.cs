using System.ComponentModel.DataAnnotations;

public class EmployeeLoginViewModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]{4,20}$", ErrorMessage = "Username must be 4–20 alphanumeric characters.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
