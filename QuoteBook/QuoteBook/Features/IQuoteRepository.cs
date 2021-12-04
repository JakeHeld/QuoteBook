using QuoteBook.Features.Quotes;
using System.Linq.Expressions;

namespace QuoteBook.Features
{
    public interface IQuoteRepository<T>
    {
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        Task<bool> Create(QuoteCreateDto entity);
        Task<bool> Update(int id, QuoteEditDto entity);
        Task<bool> Delete(int id);
    }
}
