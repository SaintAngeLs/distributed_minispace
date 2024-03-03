namespace Pacco.Services.Pricing.Api.DTO
{
    public class OrderPricingDto
    {
        public decimal OrderPrice { get; set; }
        public decimal CustomerDiscount { get; set; }
        public decimal OrderDiscountPrice { get; set; }
    }
}