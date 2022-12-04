using Kwetter.LikeService.Model;
using Microsoft.EntityFrameworkCore;

namespace Kwetter.LikeService.Data
{
    public class DataContext : DbContext
    {

            public DataContext(DbContextOptions<DataContext> options)
                : base(options)
            {

            }


            public virtual DbSet<Like> Likes { get; set; }

        }
    }
