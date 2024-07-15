using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.Models
{
    [Table("tbl_Photo")]
    public class PhotoModel : BaseModel
    {
        [Key]
        public int Id { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public string Category { get; set; }
        public int LocationId { get; set; }
        [NotMapped]
        public string Location { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public byte[] Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public DateTime UploadedDate { get; set; }
        //public int UserId { get; set; }

    }
}
