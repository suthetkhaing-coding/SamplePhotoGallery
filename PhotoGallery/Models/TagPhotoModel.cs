using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.Models
{
    [Table("tbl_TagPhoto")]
    [Keyless]
    public class TagPhotoModel
    {
        public string TagId { get; set; }
        public string PhotoId { get; set; }
    }
}
