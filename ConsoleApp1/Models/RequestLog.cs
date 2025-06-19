using System;
using System.ComponentModel.DataAnnotations;

namespace KBIPMobileBackend.Models
{
    public class RequestLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Endpoint { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}