using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuoteBook.Data;
using QuoteBook.Data.Entities;
using QuoteBook.Features;
using QuoteBook.Features.Quotes;

namespace QuoteBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IQuoteRepository<Quote> _quoteRepository;

        public QuotesController(DataContext context, IQuoteRepository<Quote> quoteRepository)
        {
            _context = context;
            _quoteRepository = quoteRepository;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            return await _context.Quotes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = (Quote) _quoteRepository.GetByCondition(x => x.Id == id).Cast<Quote>();

            if(quote == null)
            {
                return NotFound();
            }

            return quote;
        }

        [HttpGet("random-nsfw")]
        public async Task<ActionResult<Quote>> GetRandomNSFWQuote()
        {
            var rand = new Random();
            Quote? quote = null;
            while(quote == null)
            {
                quote = (Quote)_quoteRepository.GetByCondition(x => x.Id == rand.Next(_context.Quotes.Count())).Cast<Quote>();
            }
            return quote;
        }

        [HttpGet("random-non-nsfw")]
        public async Task<ActionResult<Quote>> GetRandomQuote()
        {
            var rand = new Random();
            Quote? quote = null;
            while (quote == null)
            {
                quote = (Quote)_quoteRepository.GetByCondition(x => x.Id == rand.Next(_context.Quotes.Count()) && x.NSFW == false).Cast<Quote>();
            }
            return quote;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuote(int id, QuoteEditDto quoteDto)
        {
            bool success = await _quoteRepository.Update(id, quoteDto);
            if(success)
            {
                return NoContent();   
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(QuoteCreateDto quoteDto)
        {
            bool success = await _quoteRepository.Create(quoteDto);
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            bool success = await _quoteRepository.Delete(id);
            if (success)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}
