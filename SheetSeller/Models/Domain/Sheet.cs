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
        [MaxLength(600)]
        public string Description { get; set; }
        [Range(0, 100)]
        public int? Price { get; set; }
        public string File { get; set; }
        public ApplicationUser? Author { get; set; }
        public List<ApplicationUser>? OwnedBy { get; set; }
    }
}
