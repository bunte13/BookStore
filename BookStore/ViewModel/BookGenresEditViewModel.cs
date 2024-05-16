using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace BookStore.ViewModel
{
    public class BookGenresEditViewModel
    {
        public Books? Book { get; set; }
        public IEnumerable<int>? SelectedGenresEdit { get; set; }
        public IEnumerable<SelectListItem>? GenreListEdit { get; set; }
        public IFormFile? FrontPageFile { get; set; }
        public IFormFile? PdfFile { get; set; }
    }
}
