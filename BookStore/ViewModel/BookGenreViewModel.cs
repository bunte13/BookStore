using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.ViewModel


{
    public class BookGenreViewModel
    {
        public IList<Books> Books { get; set; }
        public SelectList Genres { get; set; }  
        public string BookGenre { get; set; }
        public string SearchString { get; set; }
        public IList<Author> Authors { get; set; }
        public string AuthorSearchString { get; set; }
    }
}
