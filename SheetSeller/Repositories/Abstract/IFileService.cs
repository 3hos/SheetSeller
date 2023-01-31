using SheetSeller.Models.DTO;

namespace SheetSeller.Repositories.Abstract
{
    public interface IFileService
    {
        Status SaveImage(IFormFile imageFile, string ID);
        Status DeleteImage(string imageFileName);
    }
}
