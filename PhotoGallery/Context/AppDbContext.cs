using Microsoft.EntityFrameworkCore;
using PhotoGallery.Models;

namespace PhotoGallery.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<PhotoModel> Photos { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<LocationModel> Locations { get; set; }
        public DbSet<OwnerModel> Owners { get; set; }
        public DbSet<TagModel> Tags { get; set; }
        public DbSet<TagPhotoModel> TagsPhoto { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
