using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface IEmailService
    {
        Task<Status> SendConfirmationLink(string username ,string mail, string link);
        Task<Status> SendRestoreLink(string username, string mail, string link);
    }
}
