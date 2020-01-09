using System;

namespace PTAnalitic.Core.Model
{
    public class ProductHistory : IEntity
    {
        public long PointOfSale { get; set; }
        public long ProductId { get; set; }
        public DateTime Date { get; set; }
        public long Stock { get; set; }
    }
}