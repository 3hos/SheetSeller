using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface IFileService
    {
        Task<Status> SaveImageAsync(IFormFile imageFile, string ID);
        Status DeleteFile(string FileName);
        Status SavePDF(IFormFile imageFile, string ID);
    }
}
