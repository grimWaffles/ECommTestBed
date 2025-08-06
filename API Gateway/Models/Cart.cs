using ApiGateway.Protos;

namespace API_Gateway.Models
{
    public class Cart
    {
        public int Id { get; set; }              // Optional: If you want cart IDs
        public int UserId { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public decimal TotalCost { get; set; }

        public void RecalculateTotal()
        {
            TotalCost = (decimal)Items.Sum(i => i.GrossAmount * i.Quantity);
        }
    }
}