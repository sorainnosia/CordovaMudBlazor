using System;

namespace CordovaMudBlazor.Shared
{
    public class Sale
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }
    }
}
