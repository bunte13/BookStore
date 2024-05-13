using BookStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStore.ViewModel
{
    public class AuthorNameViewModel
    {
        public IList<Author>? Authors { get; set; }
        public string SearchString { get; set; }

    }
}
