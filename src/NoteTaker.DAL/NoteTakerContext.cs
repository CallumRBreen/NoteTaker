﻿using Microsoft.EntityFrameworkCore;
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
            UserConfiguration(builder);
        }

        private static void NoteConfiguration(ModelBuilder builder)
        {
            builder.Entity<Note>().ToTable("Note");
            builder.Entity<Note>().Property(x => x.Title).IsRequired();
            builder.Entity<Note>().Property(x => x.Content).IsRequired();
            builder.Entity<Note>().HasIndex(x => x.Modified);
            builder.Entity<Note>().HasIndex(x => x.Created);
        }

        private static void UserConfiguration(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("User");
            builder.Entity<User>().Property(x => x.Username).IsRequired();
            builder.Entity<User>().HasIndex(x => x.Username).IsUnique();
            builder.Entity<User>().Property(x => x.FirstName).IsRequired();
            builder.Entity<User>().Property(x => x.LastName).IsRequired();
            builder.Entity<User>().Property(x => x.PasswordHash).IsRequired();
        }

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
