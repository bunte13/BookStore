using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Models;
namespace BookStore.ViewModel
{
    public class BookGenresEditViewModel
    {
        public Books? Book { get; set; }
        public IEnumerable<int>? SelectedGenresEdit { get; set; }
        public IEnumerable<SelectListItem>? GenreListEdit { get; set; }
    }
}
