namespace tiki_shop.Models.Entity
{
    public class User
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public float Balance { get; set; }
        public float Commission { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
