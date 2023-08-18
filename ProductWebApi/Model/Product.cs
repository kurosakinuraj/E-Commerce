using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductWebApi.Model
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public Guid ProductId { get; set; }
        [Required]
        [Column("product_name")]
        public string ProductName { get; set; }
        [Column("product_code")]
        public string ProductCode { get; set; }
        [Column("product_description")]
        public string ProductDescription { get; set; }
        [Column("product_price")]
        public decimal ProductPrice { get; set; }
    }
}
