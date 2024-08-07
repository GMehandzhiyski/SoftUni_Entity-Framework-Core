﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(8)]
        public string RegistrationNumber { get; set; } = null!;

        [Required]
        [MaxLength(17)]
        public string VinNumber { get; set; } = null!;


        [MaxLength(1420)]
        public int TankCapacity { get; set; }

        [MaxLength(29000)]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [Required]
        public int DespatcherId { get; set; }
        [ForeignKey(nameof(DespatcherId))]

        [Required]
        public virtual Despatcher Despatcher { get; set; } = null!;

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; } = null!;
    }
}
