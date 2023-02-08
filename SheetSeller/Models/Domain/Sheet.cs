using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SheetSeller.Models.Domain
{
    public class Sheet
    {
        public int? ID { get; set; }
        [NotNull]
        [MaxLength(100)]
        public string Title { get; set; }
        [NotNull]
        public string Description { get; set; }
        public int? Price { get; set; }
        public ApplicationUser? Author { get; set; }
        public List<ApplicationUser>? OwnedBy { get; set; }
    }
}
