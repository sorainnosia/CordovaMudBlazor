using System;

namespace CordovaMudBlazor.Shared
{
    public class Stock
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public Product Product { get; set; }
    }
}
