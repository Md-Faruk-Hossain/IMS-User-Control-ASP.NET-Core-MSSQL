using System.ComponentModel.DataAnnotations.Schema;

namespace spa_core_final_project.Models
{
    public class Order
    {
        public Order()
        {
            this.OrderDetails = new List<OrderDetail>();
        }

        public int OrderId { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        //navigation
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
