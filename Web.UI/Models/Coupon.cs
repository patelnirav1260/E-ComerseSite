namespace Web.UI.Models
{
    public class Coupon
    {
        public int? CouponId { get; set; } = 0;
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
