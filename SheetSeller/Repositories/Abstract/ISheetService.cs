using SheetSeller.Models.Domain;
using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface ISheetService
    {
        Task<Status> CreateSheetAsync(Sheet model);
        Task<Status> UpdateSheetAsync(Sheet model);
        Task<Status> DeleteSheetAsync(Sheet model);
        Task<List<Sheet>> GetSheetsAsync(string username);
    }
}
