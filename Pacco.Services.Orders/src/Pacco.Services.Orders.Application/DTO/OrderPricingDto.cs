namespace Pacco.Services.Orders.Application.DTO
{
    public class OrderPricingDto
    {
        public decimal OrderPrice { get; set; }
        public decimal CustomerDiscount { get; set; }
        public decimal OrderDiscountPrice { get; set; }
    }
}