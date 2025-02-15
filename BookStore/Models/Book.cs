﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BookStore.Models
{
    public class Books
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string? Title { get; set; }
        [Display(Name = "Year Published")]
        public int? YearPublished { get; set; }
        [Display(Name = "Number od pages")]
        public int NumPages { get; set; }
        public string? Description { get; set; }
        [StringLength(50)]
        public string? Publisher { get; set; }
        [Display(Name = "Front Page")]
        public string? FrontPage { get; set; }
        [Display(Name = "Download here")]
        public string? DownloadUrl { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public ICollection<BookGenre>? BookGenres { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<UserBooks>? UserBooks { get; set; }

    }
}
