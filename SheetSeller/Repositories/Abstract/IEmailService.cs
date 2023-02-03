using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface IEmailService
    {
        Task<Status> SendConfirmationLink(string mail, string link);
    }
}
