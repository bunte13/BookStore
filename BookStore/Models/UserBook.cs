using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class UserBooks
    {
        public int Id { get; set; }
        [StringLength(450)]
        public string? AppUser { get; set; }
        public int BookId { get; set; }
        public Books? Book { get; set; }
    }
}
