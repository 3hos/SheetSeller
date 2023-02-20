using SheetSeller.Models.Domain;

namespace SheetSeller.Models.DTO
{
    public class SheetList
    {
        public IQueryable<Sheet> Sheets { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Term { get; set; }
    }
}
