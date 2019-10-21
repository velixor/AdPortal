using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AdPortalApi.Models
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        [Required] public string Name { get; set; }

        public ICollection<Ad> Ads { get; set; }
    }
}