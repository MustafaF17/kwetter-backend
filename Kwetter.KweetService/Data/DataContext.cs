using Kwetter.KweetService.Dto;
using Kwetter.KweetService.Model;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.KweetService.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {

        }


        public virtual DbSet<Kweet> Kweets { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }

    }
}
