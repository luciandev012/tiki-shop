namespace tiki_shop.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public float Balance { get; set; }
        public float Commission { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public string Role { get; set; }
        public string Fullname { get; set; }
    }
}
