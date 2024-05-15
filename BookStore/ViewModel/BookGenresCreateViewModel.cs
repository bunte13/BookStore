using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.ViewModel
{
    public class BookGenresCreateViewModel
    {
        public Books Book { get; set; }
        public IEnumerable<int>? SelectedGenresCreate { get; set; }
        public IEnumerable<SelectListItem>? GenreListCreate { get; set; }
    }
}
