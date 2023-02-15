using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface ISheetService
    {
        Task<Status> CreateSheetAsync(Sheet model);
        Task<Status> UpdateSheetAsync(Sheet model);
        Task<Status> DeleteSheetAsync(Sheet model);
        Task<Status> UploadFileAsync(IFormFile File, int id);
        List<Sheet> GetSheets(string username);
        Sheet GetSheet(int ID);
    }
}
