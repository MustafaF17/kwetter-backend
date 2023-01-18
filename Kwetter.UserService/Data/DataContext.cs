using Kwetter.UserService.Model;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.UserService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }

    }
}
