using Microsoft.EntityFrameworkCore;
using Sample.Api.Models;

namespace Sample.Api
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Person> Persons { get; set; }
    }
}
