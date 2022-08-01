namespace tiki_shop.Models.Request
{
    public class ChangePasswordRequest
    {
        public string PhoneNumber { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
