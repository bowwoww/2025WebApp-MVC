using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyModel.Models
{
    public class Department
    {
        [Key]
        [DisplayName("科系代碼")]
        public string DepartID { get; set; } = null!;
        [DisplayName("科系名稱")]
        public string DepartName { get; set; } = null!;

        //public ICollection<tStudent> Students { get; set; } = new List<tStudent>();

        public virtual List<tStudent>? tStudents { get; set; }

    }
}
