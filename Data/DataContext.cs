using Microsoft.EntityFrameworkCore;
using todo.Models;

namespace todo.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
    }
}