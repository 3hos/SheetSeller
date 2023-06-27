using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface ISheetService
    {
        Task<Status> CreateSheetAsync(Sheet model);
        Task<Status> UpdateSheetAsync(EditSheet model);
        Task<Status> DeleteSheetAsync(Sheet model);
        Task<Status> AddTag(string tag, int id);
        Task<Status> RemoveTag(string tag, int id);
        Task<Status> UploadFileAsync(IFormFile File, int id);
        Sheet GetSheet(int ID);
        IQueryable<Sheet> GetSheets(string username);
        IQueryable<Sheet> GetSheets(ApplicationUser user);
        IQueryable<Sheet> OwnedSheets(ApplicationUser user);
        SheetList GetSheetList(string term = "", string tag = "", bool paging = false, int currentPage = 0, string sorting="pop");
        Task<Status> Own(int SheetID, string username);
        Task<Status> DeOwn(int SheetID, string username);
        IQueryable<Tag> GetTags(string term = "", int maxlen = 5);
    }
}
