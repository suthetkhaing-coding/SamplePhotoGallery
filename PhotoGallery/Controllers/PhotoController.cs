using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoGallery.Context;
using PhotoGallery.Models;
using PhotoGallery.Services;
using System.Security.Claims;

namespace PhotoGallery.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService service)
        {
            _photoService = service;
        }

        // GET: PhotoController
        public ActionResult Index()
        {
            var lstPhotos = _photoService.GetPhotos();
            return View(lstPhotos);
        }

        // GET: PhotoController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var photoModel = _photoService.GetPhoto(id);
                return View(photoModel);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: PhotoController/Create
        public ActionResult Create()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            categories.Add(new CategoryModel
            {
                Id = 0,
                Title = "Please select category",
            });
            var lstCategory = _photoService.GetCategories();
            categories.AddRange(lstCategory);

            List<LocationModel> locations = new List<LocationModel>();
            locations.Add(new LocationModel
            {
                Id = 0,
                Name = "Please select location",
            });
            var lstLocation = _photoService.GetLocations();
            locations.AddRange(lstLocation);

            ViewBag.CategoryList = categories;
            ViewBag.LocationList = locations;
            return View();
        }

        // POST: PhotoController/Create
        [HttpPost]
        public ActionResult Create(PhotoModel reqModel)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                if(reqModel != null)
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        var test = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                        reqModel.Owner = username;
                    }
                    else
                    {
                        reqModel.Owner = "";
                    }

                    respModel = _photoService.Create(reqModel);
                }
                else
                {
                    respModel.RespCode = "999";
                    respModel.RespMessage = "Insert failed!";
                }
            }
            catch (Exception ex)
            {
                respModel.RespCode = "999";
                respModel.RespMessage = "Error: " + ex.Message;
            }

            return Json(respModel);
        }

        // GET: PhotoController/Edit/5
        public ActionResult Edit(int id)
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            var lstCategory = _photoService.GetCategories();
            categories.AddRange(lstCategory);

            List<LocationModel> locations = new List<LocationModel>();
            var lstLocation = _photoService.GetLocations();
            locations.AddRange(lstLocation);

            ViewBag.CategoryList = categories;
            ViewBag.LocationList = locations;
            var photoModel = _photoService.GetPhoto(id);
            return View(photoModel);
        }

        // POST: PhotoController/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, PhotoModel model)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                if (model != null)
                {
                    if (HttpContext.User.Identity.IsAuthenticated)
                    {
                        var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                        model.Owner = username;
                    }
                    else
                    {
                        model.Owner = "";
                    }
                    respModel = _photoService.Update(id,model);
                }
                else
                {
                    respModel.RespCode = "999";
                    respModel.RespMessage = "Update failed!";
                }
            }
            catch (Exception ex)
            {
                respModel.RespCode = "999";
                respModel.RespMessage = "Error: " + ex.Message;
            }

            return Json(respModel);
        }

        // POST: PhotoController/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            ResponseModel respModel = new ResponseModel();
            try
            {
                respModel = await _photoService.Delete(id);
            }
            catch (Exception ex)
            {
                respModel.RespCode = "999";
                respModel.RespMessage = ex.Message + ex.StackTrace;
            }
            return Json(respModel);
        }
    }
}
