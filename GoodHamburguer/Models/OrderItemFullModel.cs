namespace GoodHamburguer.Models
{
    public class OrderItemFullModel
    {
        private static int FirstId = 0;
        public int? Id { get; set; }
        public string? Sandwich { get; set; }
        public List<string>? Extra { get; set; }
        public double? Price { get; set; }
        public OrderItemFullModel()
        {
            Id = ++FirstId;         
        }
     
    }
}
