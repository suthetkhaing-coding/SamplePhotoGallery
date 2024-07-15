using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoGallery.Models
{
    [Table("tbl_User")]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set;}
        public string Address { get; set;}
        public string PhoneNumber { get; set;}
        public string UserRole { get; set;}
        public string Email { get; set;}
        public string Password { get; set;}
    }
}
