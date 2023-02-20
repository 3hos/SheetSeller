using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SheetSeller.Models.DTO
{
    public class EditSheet
    {
        public int ID { get; set; }
        [NotNull]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(600)]
        public string Description { get; set; }
        [Range(0, 100)]
        public int Price { get; set; }
    }
}
