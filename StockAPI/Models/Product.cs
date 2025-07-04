using MongoDB.Bson;

namespace StockAPI.Models
{
    public class Product
    {

        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public float UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public Category Category { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }

    public class Category {
        public int Id { get; set; }
        public string CategoryName { get; set; } = "??";
        public string Description { get; set; } = "??";
    }
}