using Microsoft.EntityFrameworkCore;
using NoteTaker.DAL.Entities;

namespace NoteTaker.DAL
{
    public class NoteTakerContext : DbContext
    {
        public NoteTakerContext(DbContextOptions<NoteTakerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Note>().ToTable("Note");
        }

        public DbSet<Note> Notes { get; set; }
    }
}
