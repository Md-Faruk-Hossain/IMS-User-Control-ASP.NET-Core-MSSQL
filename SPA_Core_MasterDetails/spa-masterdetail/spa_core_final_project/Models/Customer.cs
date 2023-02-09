namespace spa_core_final_project.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Orders = new List<Order>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;
        public string CustomerAddress { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = null!;
    }
}