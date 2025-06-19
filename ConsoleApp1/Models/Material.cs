using System.ComponentModel.DataAnnotations;
using KBIPMobileBackend.Models.Enums;

namespace KBIPMobileBackend.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }


        [Required, MaxLength(200)]
        public string Title { get; set; }
        public Language Language { get; set; } // 👈 вот enum

        [Required]
        public string ContentUrl { get; set; }
    }
}