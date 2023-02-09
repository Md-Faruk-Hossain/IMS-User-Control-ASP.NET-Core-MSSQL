using spa_core_final_project.Models;

namespace spa_core_final_project.ViewModels
{
    public class DataVM
    {
        public IEnumerable<Category> Categories { get; set; } = default!;
        public IEnumerable<Product> Products { get; set; } = default!;
        public IEnumerable<Customer> Customers { get; set; } = default!;
        public IEnumerable<Order> Orders { get; set; } = default!;

    }
}
