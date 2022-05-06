using Microsoft.EntityFrameworkCore;
using VoteApi.Entities;

namespace VoteApi
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Vote> Votes { get; set; }
    }
}