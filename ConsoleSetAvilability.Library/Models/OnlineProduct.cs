namespace ConsoleSetAvilability.Library.Models
{
    public class OnlineProduct
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string? SymbolIS { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public int WebsiteId { get; set; }
        public DateTime DateDelivery { get; set; }
        public string? SymbolAdditional { get; set; }
    }
}