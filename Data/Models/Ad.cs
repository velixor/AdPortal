using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class Ad : IEntity
    {
        public Guid Id { get; set; }
        [Required] public int Number { get; set; }
        public Guid UserId { get; set; }
        [Required] public User User { get; set; }
        [Required] public string Content { get; set; }
        public string ImageName { get; set; }
        [Required] public int Rating { get; set; }
        [Required] public DateTime CreationDate { get; set; }
    }
}