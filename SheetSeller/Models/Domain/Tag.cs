using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SheetSeller.Models.Domain
{
    public class Tag
    {
        [Key]
        [MaxLength(10)]
        public string Name { get; set; }
        public List<Sheet> TaggedSheets { get; set; }
        [NotMapped] 
        public int? SheetID { get; set; }
        [NotMapped]
        public int Tagged
        {
            get
            {
                if (TaggedSheets != null)
                { return TaggedSheets.Count; }
                return 0;
            }
        }
    }
}
