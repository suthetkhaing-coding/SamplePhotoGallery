using PhotoGallery.Context;
using PhotoGallery.Models;

namespace PhotoGallery.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;

        public LoginService(AppDbContext context) 
        {
            _context = context;
        }

        public UserModel Login(UserViewModel viewModel)
        {
            return _context.Users.Where(x => x.Email == viewModel.UserName && x.Password == viewModel.Password).FirstOrDefault();
        }
    }

    public interface ILoginService
    {
        UserModel Login(UserViewModel viewModel);
    }
}
