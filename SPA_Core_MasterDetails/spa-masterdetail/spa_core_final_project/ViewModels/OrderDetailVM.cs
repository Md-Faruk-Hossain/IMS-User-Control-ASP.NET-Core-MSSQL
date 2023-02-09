namespace spa_core_final_project.ViewModels
{
    public class OrderDetailVM
    {
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool? Delete { get; set; }
    }
}
