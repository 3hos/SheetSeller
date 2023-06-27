using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SheetSeller.Models.Domain
{
    public class Sheet
    {
        public int? ID { get; set; }
        [NotNull]
        [MaxLength(20)]
        public string Title { get; set; }
        [MaxLength(600)]
        public string Description { get; set; }
        [Range(0, 100)]
        public int Price { get; set; }
        public List<Tag>? Tags { get; set; }
        public string? File { get; set; }
        public ApplicationUser? Author { get; set; }
        public List<ApplicationUser>? OwnedBy { get; set; }
        [NotMapped]
        public int Owned 
        { get
            {if (OwnedBy != null)
                { return OwnedBy.Count; }
            return 0;
            }
        }
    }
}
