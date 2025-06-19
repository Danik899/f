using System;
using System.ComponentModel.DataAnnotations;

namespace KBIPMobileBackend.Models
{
    public class ScheduleEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string GroupNumber { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required, MaxLength(150)]
        public string Subject { get; set; }

        [MaxLength(50)]
        public string Location { get; set; }
    }
}