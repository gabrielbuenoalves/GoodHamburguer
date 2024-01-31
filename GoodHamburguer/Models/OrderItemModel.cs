namespace GoodHamburguer.Models
{
    public class OrderItemModel
    {
        /// <summary>
        /// choose between the options [x burger, x bacon, x egg]
        /// </summary>
        public string? Sandwich { get; set; }
        /// <summary>
        /// choose between the options [fries, soft drink]
        /// </summary>
        public List<string>? Extra { get; set; }
    }
}
