using tiki_shop.Models.Common;

namespace tiki_shop.Models.Entity
{
    public class Recharge
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float Amount { get; set; }
        public RechargeStatus RechargeStatus { get; set; } = RechargeStatus.Pending;
        public string? UserId { get; set; }
        public User User { get; set; }
    }
}
