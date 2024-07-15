using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.Models
{
    [Table("tbl_Tag")]
    public class TagModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
