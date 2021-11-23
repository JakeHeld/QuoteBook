namespace QuoteBook.Features.Quotes
{
    public class QuoteCreateDto
    {
        public String Quotee { get; set; }
        public String Message { get; set; }
        public String Author { get; set; }
        public bool NSFW { get; set; }
    }
}
