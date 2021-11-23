using Microsoft.EntityFrameworkCore;
using QuoteBook.Data.Entities;

namespace QuoteBook.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Quote> Quotes{ get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Quote>();

        }
    }
}
