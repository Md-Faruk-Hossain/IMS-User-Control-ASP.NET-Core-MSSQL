using System.ComponentModel.DataAnnotations.Schema;

namespace spa_core_final_project.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        //navigation
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
