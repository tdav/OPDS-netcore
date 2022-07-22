using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotOPDS.Database.Models
{
    public class Book
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public int[]? Authors { get; set; }
        public int[] Genres { get; set; }
        public int[] BookTags { get; set; }
        public int? ImageId { get; set; }
        public virtual Thumbnail Image { get; set; }
    }
}
