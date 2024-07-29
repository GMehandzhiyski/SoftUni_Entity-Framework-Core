﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
    public class Client
    {
        public Client()
        {
            List<Invoice> Invoices = new List<Invoice>();
            List<Address> Addresses = new List<Address>();
            List<ProductClient> ProductsClients = new List<ProductClient>();
        }


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string NumberVat { get; set; } =null!;

        [Required]
        public ICollection<Invoice> Invoices { get; set; } = null!;

        [Required]
        public ICollection<Address> Addresses { get; set; } = null!;

        [Required]
        public ICollection<ProductClient> ProductsClients { get; set; } = null!;

    }
}
