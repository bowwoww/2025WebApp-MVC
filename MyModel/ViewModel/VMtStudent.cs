using System.ComponentModel;
using MyModel.Models;

namespace MyModel.ViewModel
{
    public class VMtStudent
    {
        // view model
        [DisplayName("學生")]
        public List<tStudent>? tStudents { get; set; }
        [DisplayName("科系")]
        public List<Department>? departments { get; set; }
    }
}
