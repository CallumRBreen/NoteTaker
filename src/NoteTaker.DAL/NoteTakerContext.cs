using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NoteTaker.DAL.Entities;
using NoteTaker.DAL.Extensions;

namespace NoteTaker.DAL
{
    public class NoteTakerContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public NoteTakerContext(DbContextOptions<NoteTakerContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            NoteConfiguration(builder);
            UserConfiguration(builder);
        }

        private void NoteConfiguration(ModelBuilder builder)
        {
            builder.Entity<Note>().ToTable("Note");
            builder.Entity<Note>().Property(x => x.Title).IsRequired();
            builder.Entity<Note>().Property(x => x.Content).IsRequired();
            builder.Entity<Note>().HasIndex(x => x.Modified);
            builder.Entity<Note>().HasIndex(x => x.Created);
            builder.Entity<Note>().HasQueryFilter(x => x.UserId == httpContextAccessor.GetCurrentUserId());
        }

        private void UserConfiguration(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("User");
            builder.Entity<User>().Property(x => x.Username).IsRequired();
            builder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            builder.Entity<User>().Property(x => x.FirstName).IsRequired();
            builder.Entity<User>().Property(x => x.LastName).IsRequired();
            builder.Entity<User>().Property(x => x.PasswordHash).IsRequired();
            builder.Entity<User>().HasMany(x => x.Notes).WithOne(x => x.User).IsRequired();
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
