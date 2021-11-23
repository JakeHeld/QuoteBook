namespace QuoteBook.Data.Entities
{
    public class Quote
    {
        public int Id { get; set; }
        public String Quotee { get; set; }
        public String Message { get; set; }
        public String Author { get; set; }
        public DateTimeOffset Time { get; set; }
        public bool NSFW { get; set; }
    }
}
