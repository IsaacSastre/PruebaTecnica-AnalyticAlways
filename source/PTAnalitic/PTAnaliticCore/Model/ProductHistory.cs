using CsvHelper.Configuration.Attributes;
using System;

namespace PTAnalitic.Core.Model
{
    public class ProductHistory : IEntity
    {
        [Name("PointOfSale")]
        public string PointOfSale { get; set; }
        [Name("Product")]
        public string ProductId { get; set; }
        [Name("Date")]
        public DateTime Date { get; set; }
        [Name("Stock")]
        public long Stock { get; set; }
    }
}