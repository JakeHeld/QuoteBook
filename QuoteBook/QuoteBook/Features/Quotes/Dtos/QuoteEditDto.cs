namespace QuoteBook.Features.Quotes
{
    public class QuoteEditDto
    {
        public String Quotee { get; set; }
        public String Message { get; set; }
        public String Author { get; set; }
        public DateTimeOffset Time { get; set; }
        public bool NSFW { get; set; }
    }
}
