namespace SecureStore.Api.DomainLayer.DTOs
{
    public class ShoppingCartDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
        public decimal TotalPrice => Items.Sum(item => item.TotalPrice);
    }
}
