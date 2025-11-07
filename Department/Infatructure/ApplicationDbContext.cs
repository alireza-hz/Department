using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infatructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Letter> Letters { get; set; } 
        public DbSet<LetterType> LetterTypes { get; set; } 
        public DbSet<LetterFlow> LetterFlows { get; set; }
        public DbSet<Note> Notes { get; set; } 
        public DbSet<Attachment> Attachments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Letter)
                .WithMany(l => l.Attachments)
                .HasForeignKey(a => a.LetterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.FirstName)
                      .HasMaxLength(200)
                      .IsRequired();
                entity.Property(u => u.LastName)
                   .HasMaxLength(200)
                   .IsRequired();
                entity.Property(u => u.Email)
                      .HasMaxLength(200)
                      .IsRequired();

                entity.HasIndex(u => u.Email)
                      .IsUnique();
            });


            modelBuilder.Entity<LetterType>(entity =>
            {
                entity.ToTable("LetterTypes");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                      .HasMaxLength(100)
                      .IsRequired();
            });


            modelBuilder.Entity<Letter>(entity =>
            {
                entity.ToTable("Letters");
                entity.HasKey(l => l.Id);

                entity.Property(l => l.Subject)
                      .HasMaxLength(500)
                      .IsRequired();

                entity.Property(l => l.Body)
                      .HasColumnType("nvarchar(max)");

                entity.Property(l => l.CreatedAt)
                      .IsRequired();

                entity.Property(l => l.IsSecret)
                      .HasDefaultValue(false);


                entity.HasOne(l => l.Sender)
                      .WithMany(u => u.SentLetters)
                      .HasForeignKey(l => l.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(l => l.Receiver)
                      .WithMany(u => u.ReceivedLetters)
                      .HasForeignKey(l => l.ReceiverId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(l => l.LetterType)
                      .WithMany(t => t.Letters)
                      .HasForeignKey(l => l.LetterTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(l => l.LetterFlows)
                      .WithOne(f => f.Letter)
                      .HasForeignKey(f => f.LetterId);

                entity.HasMany(l => l.Notes)
                      .WithOne(n => n.Letter)
                      .HasForeignKey(n => n.LetterId);
            });

            modelBuilder.Entity<LetterFlow>(entity =>
            {
                entity.ToTable("LetterFlows");
                entity.HasKey(f => f.Id);

                entity.Property(f => f.SentAt)
                      .IsRequired();

                entity.Property(f => f.Status)
                      .HasMaxLength(100);

                entity.HasOne(f => f.FromUser)
                      .WithMany()
                      .HasForeignKey(f => f.FromUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.ToUser)
                      .WithMany()
                      .HasForeignKey(f => f.ToUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Note>(entity =>
            {
                entity.ToTable("Notes");
                entity.HasKey(n => n.Id);

                entity.Property(n => n.Text)
                      .HasColumnType("nvarchar(max)")
                      .IsRequired();

                entity.Property(n => n.CreatedAt)
                      .IsRequired();

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notes)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LetterType>().HasData(
                new LetterType { Id = 1, Title = "عادی" },
                new LetterType { Id = 2, Title = "فوری" },
                new LetterType { Id = 3, Title = "محرمانه" }
            );
        }
    }
}
