namespace spa_core_final_project.Models
{
    public class Category
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
