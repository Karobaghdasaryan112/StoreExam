﻿namespace SecureStore.Api.DomainLayer.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
    }
}
