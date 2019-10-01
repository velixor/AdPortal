using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdPortalApi.Models
{
    public class Ad
    {
        public Guid Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public string Content { get; set; }
        public string ImagePath { get; set; }
        [Required]
        public int Rating { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
    }
}
