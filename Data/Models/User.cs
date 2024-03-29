﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User : IEntity
    {
        [Required] public Guid Id { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string Name { get; set; }
        public int AdsCount { get; set; }
        public ICollection<Ad> Ads { get; set; }
    }
}