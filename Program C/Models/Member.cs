namespace Program_C.Models
{
    public class Member
    {
        public int Id { get; set; } 
        //不可為空
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public required string Phone { get; set; }
        public bool Gender { get; set; }

    }
}
