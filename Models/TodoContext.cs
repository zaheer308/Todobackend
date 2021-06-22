using Microsoft.EntityFrameworkCore;
namespace TodoBackend.Models
{
    public class DbContextBase : DbContext
    {
        public DbContextBase(DbContextOptions<DbContextBase> options) : base(options)
        {
        }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<User> Users { get; set; }
       



    }
}