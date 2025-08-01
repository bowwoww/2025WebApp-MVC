using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DTOs;

public partial class CategoryPutDTO
{

    [StringLength(20)]
    [CategoryPostDTOValidator]
    public string CateName { get; set; } = null!;

}
