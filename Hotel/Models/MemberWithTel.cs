﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Models
{
    public class MemberWithTel
    {
        public string MemberID { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;

        public string Address { get; set; } = null!;

        public DateTime Birthday { get; set; }
    }
}
