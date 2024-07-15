using PhotoGallery.Context;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Models;
using System.Security.Claims;

namespace PhotoGallery.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly AppDbContext _context;

        public PhotoService(AppDbContext context)
        {
            _context = context;
        }

        public List<PhotoModel> GetPhotos()
        {
            List<PhotoModel> lstPhotos = new List<PhotoModel>();
            lstPhotos = _context.Photos.Where(x => x.IsDeleted == false).OrderByDescending(x => x.UploadedDate).ToList();
            return lstPhotos;
        }

        public PhotoModel GetPhoto(int id)
        {
            PhotoModel photoModel = new PhotoModel();

            try
            {
                photoModel = _context.Photos.FromSqlRaw($"GetPhoto {id}").AsEnumerable().FirstOrDefault();
                if (photoModel != null)
                {
                    var category = _context.Categories.FirstOrDefault(c => c.Id == photoModel.CategoryId);
                    var location = _context.Locations.FirstOrDefault(l => l.Id == photoModel.LocationId);

                   
                    if (category != null)
                    {
                        photoModel.Category = category.Title;
                    }

                    if (location != null)
                    {
                        photoModel.Location = location.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return photoModel;
        }

        public List<CategoryModel> GetCategories()
        {
            var model = _context.Categories.Where(x => x.IsDeleted == false).ToList();
            return model;
        }

        public List<LocationModel> GetLocations()
        {
            var model = _context.Locations.Where(x => x.IsDeleted == false).ToList();
            return model;
        }

        public ResponseModel Create(PhotoModel photoModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var file = photoModel.ImageFile;
                if (file != null && file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyToAsync(ms);
                        photoModel.Image = ms.ToArray();
                    }
                }

                var currDate = DateTime.Now;
                photoModel.UploadedDate = currDate;
                //photoModel.Owner = "Su Su";
                photoModel.IsDeleted = false;
                photoModel.CreatedDate = currDate;
                photoModel.UpdatedDate = currDate;

                _context.Photos.Add(photoModel);
                var count = _context.SaveChanges();
                responseModel.RespCode = count == 1 ? "000" : "999";
                responseModel.RespMessage = count == 1 ? "Saving successful!" : "Saving failed!";
            }
            catch (Exception ex)
            {
                responseModel.RespCode = "999";
                responseModel.RespMessage = "Error: " + ex.Message;
            }
            return responseModel;
        }

        public ResponseModel Update (int id, PhotoModel photoModel)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var file = photoModel.ImageFile;
                if (file != null && file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyToAsync(ms);
                        photoModel.Image = ms.ToArray();
                    }
                }

                var model = _context.Photos.Where(p => p.Id == id).FirstOrDefault();
                if (model != null)
                {
                    if(file != null) { model.Image = photoModel.Image; }
                    model.Title = photoModel.Title; 
                    model.Description = photoModel.Description;
                    model.CategoryId = photoModel.CategoryId;
                    model.LocationId = photoModel.LocationId;
                    //model.Owner = "Su Su";
                    model.IsDeleted = false;
                    var currDate = DateTime.Now;
                    model.UploadedDate = currDate;
                    model.UpdatedDate = currDate;
                }

                _context.Photos.Update(model);
                var count = _context.SaveChanges();
                responseModel.RespCode = count == 1 ? "000" : "999";
                responseModel.RespMessage = count == 1 ? "Saving successful!" : "Saving failed!";
            }
            catch (Exception ex)
            {
                responseModel.RespCode = "999";
                responseModel.RespMessage = "Error: " + ex.Message;
            }
            return responseModel;
        }

        public async Task<ResponseModel> Delete(int id)
        {
            ResponseModel responseModel = new ResponseModel();
            try
            {
                var item = await _context.Photos.Where(x => x.Id == id).FirstOrDefaultAsync();
                item.IsDeleted = true;
                _context.Photos.Update(item);
                var count = await _context.SaveChangesAsync();
                responseModel.RespCode = count == 1 ? "000" : "999";
                responseModel.RespMessage = count == 1 ? "Photo delete successful!" : "Delete failed!";
            }
            catch (Exception ex)
            {
                responseModel.RespCode = "999";
                responseModel.RespMessage = ex.Message + ex.StackTrace;
                //return RedirectToAction(nameof(Index));
            }
            
            return responseModel;
        }
    }

    public interface IPhotoService
    {
        List<CategoryModel> GetCategories();
        List<LocationModel> GetLocations();
        List<PhotoModel> GetPhotos();
        PhotoModel GetPhoto(int id);
        ResponseModel Create(PhotoModel photoModel);
        ResponseModel Update (int id,PhotoModel photoModel);
        Task<ResponseModel> Delete(int id);
    }
}
