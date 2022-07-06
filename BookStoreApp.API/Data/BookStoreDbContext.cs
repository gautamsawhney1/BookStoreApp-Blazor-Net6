using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookStoreApp.API.Data
{
    public partial class BookStoreDbContext : IdentityDbContext<ApiUser>//DbContext
    {
        public BookStoreDbContext()
        {
        }

        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress;Database=BookStoreDb;Trusted_Connection=True;MultipleActiveResultSets=true;");
//            }
       // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(60);

                entity.Property(e => e.LastName).HasMaxLength(60);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasIndex(e => e.Isbn, "UQ__Books__9271CED0816A03E2")
                    .IsUnique();

                entity.Property(e => e.Isbn).HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Summary).HasMaxLength(60);

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK_Books_ToTable");
            });

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {

                    Name = "User",
                    NormalizedName = "USER",
                    Id = "18850188-64b9-494f-9a55-b099c587a0f5"
                },
                
            new IdentityRole
            {

                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = "6c774d80-3f64-4ef6-b6b1-b4728e670b91"
            }
          
           );

            var hasher = new PasswordHasher<ApiUser>();
            modelBuilder.Entity<ApiUser>().HasData(
                new ApiUser
                {

                    Id = "39e52d14-3c85-4d59-8bb9-e770392debd4",
                    Email = "admin@bookstore.com",
                    NormalizedEmail = "ADMIN@BOOKSTORE.COM",
                    UserName = "admin@bookstore.com",
                    NormalizedUserName = "ADMIN@BOOKSTORE.COM",
                    FirstName = "system",
                    LastName = "Admin",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1")

                },

                new ApiUser
                {


                    Id = "f99d93e0-c8e1-445d-b58b-c71229875de1",
                    Email = "user@bookstore.com",
                    NormalizedEmail = "USER@BOOKSTORE.COM",
                    UserName = "user@bookstore.com",
                    NormalizedUserName = "USER@BOOKSTORE.COM",
                    FirstName="System",
                    LastName = "User",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1")
                }
                
           );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "18850188-64b9-494f-9a55-b099c587a0f5",
                    UserId = "39e52d14-3c85-4d59-8bb9-e770392debd4"

                }, 
                new IdentityUserRole<string>
                 {
                    RoleId = "6c774d80-3f64-4ef6-b6b1-b4728e670b91",
                    UserId = "f99d93e0-c8e1-445d-b58b-c71229875de1"

                }
                    );


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
