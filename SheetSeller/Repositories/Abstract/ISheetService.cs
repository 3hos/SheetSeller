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
        Sheet GetSheet(int ID);
        List<Sheet> GetSheets(string username);
        List<Sheet> GetSheets(ApplicationUser user);
        List<Sheet> OwnedSheets(ApplicationUser user);
        SheetList GetSheetList(string term = "", bool paging = false, int currentPage = 0);
        Task<Status> Own(int SheetID, string username);
        Task<Status> DeOwn(int SheetID, string username);
    }
}
