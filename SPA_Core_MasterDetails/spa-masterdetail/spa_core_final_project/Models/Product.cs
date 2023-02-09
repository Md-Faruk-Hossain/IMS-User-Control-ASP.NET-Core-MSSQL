using System.ComponentModel.DataAnnotations.Schema;

namespace spa_core_final_project.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public int SKUCode { get; set; }
        public System.DateTime EntryDate { get; set; }
        public string PicturePath { get; set; } = null!;
        public bool InStock { get; set; }

        //navigation
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = null!;
    }
}
