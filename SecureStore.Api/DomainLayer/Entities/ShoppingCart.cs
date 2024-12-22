﻿

namespace SecureStore.Api.DomainLayer.Entities
{
    public class ShoppingCart 
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    }
}
