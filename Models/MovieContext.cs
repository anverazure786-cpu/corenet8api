using Microsoft.EntityFrameworkCore;

namespace Rest_API_Project.Models
{
    public class MovieContext:DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options):base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; } = null;

    }
}
