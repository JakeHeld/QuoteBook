using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
    public class NSFWQuotesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly QuoteRepository _quoteRepository;

        public NSFWQuotesController(DataContext context, QuoteRepository quoteRepository)
        {
            _context = context;
            _quoteRepository = quoteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            return await _context.Quotes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);

            if (quote == null)
            {
                return NotFound();
            }

            return quote;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuote(int id, QuoteEditDto quoteDto)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if(quote == null)
            {
                return NotFound();
            }

            quote.Quotee = quoteDto.Quotee;
            quote.Message = quoteDto.Message;
            quote.Author = quoteDto.Author;
            quote.Time = quoteDto.Time;
            quote.NSFW = quoteDto.NSFW;

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Entry(quote).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_quoteRepository.QuoteExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(QuoteCreateDto quoteDto)
        {
            Quote quote = new Quote{ 
                Quotee = quoteDto.Quotee,
                Message = quoteDto.Message,
                Author = quoteDto.Author,
                Time = DateTimeOffset.Now,
                NSFW = quoteDto.NSFW
            };

            using(var transaction = _context.Database.BeginTransaction())
            {
                _context.Quotes.Add(quote);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
