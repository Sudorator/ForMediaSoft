using MediaSoft.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaSoft.Data
{
    public class myDBContexts : DbContext
    {
        public myDBContexts(DbContextOptions<myDBContexts> options) : base(options)
        {

        }

        public DbSet<CategoryDB> Category { get; set; }
        public DbSet<NoteDB> Note { get; set; }
    }
}
