using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace SheetSeller.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImageProfile { get; set; }
        public List<Sheet> CreatedSheets { get; set; }
        public List<Sheet> OwnedSheets { get; set; }
    }
}
