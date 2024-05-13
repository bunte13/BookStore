using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BookStore.Models
{
    public class Author
    {
        [Display(Name = "AuthorId")]
        public int Id { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        public DateTime Date { get; set; }
        [StringLength(50)]
        public string? Nationality { get; set; }
        [StringLength(50)]
        public string? Gender { get; set; }
        public ICollection<Books>? Books { get; set; }
        public string? FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }
    }
}
