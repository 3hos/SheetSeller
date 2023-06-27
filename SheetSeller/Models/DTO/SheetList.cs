using Microsoft.AspNetCore.Mvc.Rendering;
using SheetSeller.Models.Domain;

namespace SheetSeller.Models.DTO
{
    public class SheetList
    {
        public IQueryable<Sheet> Sheets { get; set; }
        public List<SelectListItem> selectListItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Term { get; set; }
        public string? Tag { get; set; }
        public string? Sorting { get; set; }
    }
}
