using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderWebApi.Model
{
    [Table("order")]
    public class Order
    {
        [Column("order_id")]
        [Key]
        public Guid OrderId { get; set; }
        [Column("ordered_on")]
        public DateTime OrderedOn { get; set; }
        [Column("product_Id")]
        public Guid ProductId { get; set; }
        [Column("unit_price")]
        public decimal UnitPrice { get; set; }
        [Column("quantity")]
        public decimal Quantity { get; set; }

    }
}
