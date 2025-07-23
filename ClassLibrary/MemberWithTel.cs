using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    public class MemberWithTel
    {
        [Key]
        public string MemberID { get; set; } = null!;

        public string Name { get; set; } = null!;
        [StringLength(20)]
        public string Tel { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime Birthday { get; set; }
    }
}
