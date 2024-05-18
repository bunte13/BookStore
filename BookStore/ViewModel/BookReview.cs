using BookStore.Models;

namespace BookStore.ViewModel
{
    public class BookReview
    {
        public string Title { get; set; }
        public int Id { get; set; } // This is the book ID
        public Review Review { get; set; } = new Review();
    }
}
