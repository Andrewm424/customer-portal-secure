using System.ComponentModel.DataAnnotations;



public class Payment

{

    public bool IsVerified { get; set; } = false;
    public DateTime? VerifiedAt { get; set; }

    public int Id { get; set; }

    [Required]
    [Range(1, 1000000)]
    public decimal Amount { get; set; }

    [Required]
    public string Currency { get; set; } = string.Empty;

    [Required]
    public string Provider { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{10,20}$")]
    public string PayeeAccount { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^[A-Z]{6}[A-Z0-9]{2}([A-Z0-9]{3})?$")]
    public string SwiftCode { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; } = DateTime.Now;

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
}
