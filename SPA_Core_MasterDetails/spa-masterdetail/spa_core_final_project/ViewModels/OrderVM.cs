using spa_core_final_project.Models;

namespace spa_core_final_project.ViewModels
{
    public class OrderVM
    {
        public OrderVM()
        {
            this.OrderDetails = new List<OrderDetailVM>();
        }
        public Order Order { get; set; } = null!;
        public List<OrderDetailVM> OrderDetails { get; set; } = null!;
    }
}
