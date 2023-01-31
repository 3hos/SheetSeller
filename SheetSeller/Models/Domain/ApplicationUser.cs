using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace SheetSeller.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImageProfile { get; set; }
        [AllowNull]
        public List<Sheet>? CreatedSheets { get; set; }
        [AllowNull]
        public List<Sheet>? OwnedSheets { get; set; }
    }
}
