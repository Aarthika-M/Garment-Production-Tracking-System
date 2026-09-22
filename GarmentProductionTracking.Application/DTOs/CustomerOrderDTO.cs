
using System;

namespace Application.DTOs
{
    public class CustomerOrderDTO
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string GarmentType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string? CustomerInstructions { get; set; }
        public byte[]? ImageContent { get; set; }
        public string Status { get; set; } = "Pending";
    }
}