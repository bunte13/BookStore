using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookStore.Areas.Identity.Data;
using BookStore.Models;

namespace BookStore.Data
{
    public class BookStoreContext : IdentityDbContext<BookStoreUser>
    {
        public BookStoreContext (DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        public DbSet<BookStore.Models.Author> Author { get; set; } = default!;

        public DbSet<BookStore.Models.BookGenre> BookGenre { get; set; } = default!;

        public DbSet<BookStore.Models.Genres> Genres { get; set; } = default!;

        public DbSet<BookStore.Models.Review> Review { get; set; } = default!;

        public DbSet<BookStore.Models.UserBooks> UserBooks { get; set; } = default!;

        public DbSet<BookStore.Models.Books> Books { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
