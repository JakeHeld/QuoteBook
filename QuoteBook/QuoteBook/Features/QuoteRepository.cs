using QuoteBook.Data;
using QuoteBook.Data.Entities;

namespace QuoteBook.Features
{
    public class QuoteRepository
    {
        private readonly DataContext _context;       

        public QuoteRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Checks the dataContext to see if the desired quote id exists.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool QuoteExists(int id)
        {
            return _context.Quotes.Any(e => e.Id == id);
        }

        /// <summary>
        /// Returns the number of quotes that contain NSFW content.
        /// </summary>
        /// <returns></returns>
        public int NumberOfNSFWQuotes()
        {
            return _context.Quotes.Count(x => x.NSFW == true);
        }

        /// <summary>
        /// Returns the number of quotes that do not contain NSFW content.
        /// </summary>
        /// <returns></returns>
        public int NumberOfNonNSFWQuotes()
        {
            return _context.Quotes.Count(x => x.NSFW != false);
        }

        /// <summary>
        /// Returns the total number of quotes.
        /// </summary>
        /// <returns></returns>
        public int NumberOfTotalQuotes()
        {
            return _context.Quotes.Count();
        }

        /// <summary>
        /// Returns the quote, that has the associated id, from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Quote> GetQuote(int id)
        {
            Quote quote = await _context.Quotes.FindAsync(id);
            return quote;
        }

        /// <summary>
        /// Returns if the quote, that has the associated id, is NSFW or not. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsQuoteNSFW(int id)
        {
            Quote quote = null;
            if((quote = GetQuote(id).Result) != null){
                return quote.NSFW;
            }
            return false;
        }
    }
}
