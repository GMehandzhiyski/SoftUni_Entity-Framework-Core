﻿using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.Data.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(8)]
        public string PostalCode { get; set; } = null!;

        [Required]
        public Region Region { get; set; } 

        //colection
    }
}
