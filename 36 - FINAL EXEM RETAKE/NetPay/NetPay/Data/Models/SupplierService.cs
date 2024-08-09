using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetPay.Data.Models
{
    public class SupplierService
    {
        [Required]
        public int SupplierId  { get; set; }
        [ForeignKey(nameof(SupplierId))]

        [Required]
        public virtual Supplier Supplier { get; set; } = null!;


        [Required]
        public int ServiceId  { get; set; }
        [ForeignKey(nameof(ServiceId))]

        [Required]
        public virtual Service Service { get; set; }=null!;

    }
}
