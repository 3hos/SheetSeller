using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface ISheetService
    {
        Task<Status> CreateSheetAsync(Sheet model);
        Task<Status> UpdateSheetAsync(EditSheet model);
        Task<Status> DeleteSheetAsync(Sheet model);
        Task<Status> UploadFileAsync(IFormFile File, int id);
        List<Sheet> GetSheets(string username);
        Sheet GetSheet(int ID);
        SheetList GetSheetList(string term = "", bool paging = false, int currentPage = 0);
        Task<Status> Own(int SheetID, string username);
    }
}
