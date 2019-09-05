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
            NoteConfiguration(builder);
        }

        private static void NoteConfiguration(ModelBuilder builder)
        {
            builder.Entity<Note>().ToTable("Note");
            builder.Entity<Note>().Property(x => x.Title).IsRequired();
            builder.Entity<Note>().Property(x => x.Content).IsRequired();
            builder.Entity<Note>().HasIndex(x => x.Modified);
            builder.Entity<Note>().HasIndex(x => x.Created);
        }

        public DbSet<Note> Notes { get; set; }
    }
}
