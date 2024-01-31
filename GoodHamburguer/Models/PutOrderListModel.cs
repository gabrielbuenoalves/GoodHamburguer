namespace GoodHamburguer.Models
{
    public class PutOrderListModel
    {
        /// <summary>
        /// choose your update option by id
        /// </summary>
        public int Id { get; set; }
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
