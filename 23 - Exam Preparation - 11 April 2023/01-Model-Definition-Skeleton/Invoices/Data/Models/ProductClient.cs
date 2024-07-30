using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Data.Models
{
    public class ProductClient
    {
        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]

        [Required]
        public virtual Product Product { get; set; } = null!;

        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]

        [Required]
        public virtual Client Client { get; set; } = null!;
    }
}
