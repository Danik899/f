using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Models;

namespace KBIPMobileBackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public byte[] PasswordSalt { get; set; }

        // связь со студентом (опционально)
        public int? StudentId { get; set; }
        public Student Student { get; set; }
    }
}