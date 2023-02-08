using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsync(LoginModel model);
        Task<Status> LoginAsync(string email);
        Task LogOutAsync();
        Task<Status> RegisterAsync(RegistrationModel model);
        Task<Status> ChangePasswordAsync(ChangePasswordModel model, string username);
        Task<Status> ChangePasswordAsync(ChangePasswordPass model, string token, string userId);
        Task<Status> ConfirmEmailAsync(string userId, string code);
    }
}
