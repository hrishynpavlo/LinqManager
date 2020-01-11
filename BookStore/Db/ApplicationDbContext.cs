using BookStore.Db.Entities;
using LinqManager.Client.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinqManager.Client.Db
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(entity => {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Name).IsRequired();              
            });

            modelBuilder.Entity<Author>(entity => {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.FirstName).IsRequired();
                entity.Property(p => p.LastName).IsRequired();
                entity.Property(p => p.CountryId).IsRequired();

                entity.HasOne(r => r.Country).WithMany(r => r.Authors).HasForeignKey(fk => fk.CountryId);
            });

            modelBuilder.Entity<Book>(entity => {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.AuthorId).IsRequired();

                entity.HasOne(r => r.Author).WithMany(r => r.Books).HasForeignKey(fk => fk.AuthorId);
            });

            modelBuilder.Entity<PublishingHouse>(entity => {
                entity.HasKey(k => k.Id);
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
            });

            modelBuilder.Entity<BookPublishingHouse>(entity => {
                entity.HasKey(k => new { k.BookId, k.PublishingHouseId });
                entity.HasOne(r => r.Book).WithMany(r => r.PublishingHouses).HasForeignKey(fk => fk.BookId);
                entity.HasOne(r => r.PublishingHouse).WithMany(r => r.Books).HasForeignKey(fk => fk.PublishingHouseId);
            });
        }

        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
    }
}
