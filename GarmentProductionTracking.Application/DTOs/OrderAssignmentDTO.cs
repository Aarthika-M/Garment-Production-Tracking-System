using System;
using System.Collections.Generic;
using Domain.Entities; // To use User class

namespace Application.DTOs
{
    public class OrderAssignmentDTO
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public string? CustomerName { get; set; }   // From GarmentOrder.Customer

        public int ManagerId { get; set; }
        public string? ManagerName { get; set; }    // From User.Username

        public int WorkerId { get; set; }
        public string? WorkerName { get; set; }     // From User.Username

        public string Status { get; set; } = "Assigned";

        public DateTime AssignedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        // Optional order info for dashboard display
        public string? GarmentType { get; set; }
        public string? CustomerInstructions { get; set; }

        public byte[]? ImageContent { get; set; } 
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }

        // List of available workers for assignment
        public List<User> WorkerList { get; set; } = new();
    }
}
