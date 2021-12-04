using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using QuoteBook.Data;
using QuoteBook.Data.Entities;
using QuoteBook.Features.Quotes;
using System.Linq.Expressions;


namespace QuoteBook.Features
{
    public abstract class QuoteRepository<T> : IQuoteRepository<T> where T : class
    {
        private readonly DataContext _context;       

        public QuoteRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsNoTracking();
        }

        public async Task<bool> Create(QuoteCreateDto entity)
        {
            Quote quote = new Quote {
                Quotee = entity.Quotee,
                Message = entity.Message,
                Author = entity.Author,
                Time = DateTimeOffset.Now,
                NSFW = entity.NSFW
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Quotes.Add(quote);
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> Update(int id, QuoteEditDto entity)
        {
            if (entity == null)
            {
                return false;
            }

            Quote? quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return false;
            }

            quote.Quotee = entity.Quotee;
            quote.Message = entity.Message;
            quote.Author = entity.Author;
            quote.Time = entity.Time;
            quote.NSFW = entity.NSFW;

            using (var transaction = _context.Database.BeginTransaction())
            {
                
                _context.Entry(quote).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            Quote? quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return false;
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                _context.Quotes.Remove(quote);
                try
                {
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
