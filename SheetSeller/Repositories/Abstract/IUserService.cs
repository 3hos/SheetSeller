using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByName(string username);
        Task<Status> SetImgProfile(IFormFile imageFile, string username);
        Task<Status> DeleteImgProfile(string username);

    }
}
