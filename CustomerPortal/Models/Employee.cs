using System.ComponentModel.DataAnnotations;

namespace CustomerPortal.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public byte[] Salt { get; set; } = Array.Empty<byte>();
    }

}
