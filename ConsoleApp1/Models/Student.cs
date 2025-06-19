using System.ComponentModel.DataAnnotations;

namespace KBIPMobileBackend.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; }

        [Required]
        public string GroupNumber { get; set; }

        public User User { get; set; }  // навигационное свойство
    }
}