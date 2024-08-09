using System.ComponentModel.DataAnnotations;

namespace NetPay.Data.Models
{
    public class Supplier
    {
        public Supplier()
        {
            SuppliersServices = new HashSet<SupplierService>();
        }

        [Key]
        public int Id { get; set;}

        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string SupplierName { get; set; } = null!;

        public virtual ICollection<SupplierService> SuppliersServices { get; set; }
    }
}
