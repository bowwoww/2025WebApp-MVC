using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class CategoriesDTO
    {

        public string CateID { get; set; } = null!;

        [StringLength(20)]
        public string CateName { get; set; } = null!;


        public virtual ICollection<Product> Product { get; set; } = new List<Product>();
    }
}
