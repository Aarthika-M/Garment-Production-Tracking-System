using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;


namespace Application.Services
{
    public class ManagerService
    {
        private readonly IManagerRepository _managerRepository;

       
        public ManagerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        //  Get all orders (assigned or not) for manager dashboard
        public async Task<List<OrderAssignmentDTO>> GetAllOrdersForDashboardAsync()
        {
            // DB la irundhu all orders & assignments fetch pannum
            var orders = await _managerRepository.GetAllOrdersAsync();
            var assignments = await _managerRepository.GetAllAssignmentsAsync();

            // Orders + Assignments combine pannitu DTO form la convert pannum
            var result = orders.Select(o =>
            {
                var assignment = assignments.FirstOrDefault(a => a.OrderId == o.Id);

                return new OrderAssignmentDTO
                {
                    Id = assignment?.Id ?? 0,
                    OrderId = o.Id,
                    CustomerName = o.Customer?.Username ?? "",
                    GarmentType = o.GarmentType,
                    ImageContent = o.ImageContent,
                    CustomerInstructions = o.CustomerInstructions ?? "No instructions",
                    Quantity = o.Quantity,
                    OrderDate = o.OrderDate,
                    DeliveryDate = o.DeliveryDate,
                    ManagerId = assignment?.ManagerId ?? 0,
                    ManagerName = assignment?.Manager?.Username ?? "",
                    WorkerId = assignment?.WorkerId ?? 0,
                    WorkerName = assignment?.Worker?.Username ?? "Not Assigned",
                    Status = assignment?.Status ?? "Not Assigned",
                    AssignedDate = assignment?.AssignedDate ?? default,
                    CompletedDate = assignment?.CompletedDate
                };
            }).ToList();

            return result;
        }

        // Assign or update worker for a specific order
        public async Task AssignWorkerToOrderAsync(int orderId, int workerId, int managerId)
        {
            // Check if already assignment exists for this order
            var assignment = await _managerRepository.GetAssignmentByOrderIdAsync(orderId);

            if (assignment == null)
            {
                //  Assignment illa – create new one
                assignment = new OrderAssignment
                {
                    OrderId = orderId,
                    WorkerId = workerId,
                    ManagerId = managerId,
                    Status = "Assigned",
                    AssignedDate = DateTime.UtcNow
                };

                await _managerRepository.AddAssignmentAsync(assignment);
            }
            else
            {
                //  Already exists – update existing one
                assignment.WorkerId = workerId;
                assignment.ManagerId = managerId;
                assignment.Status = "Assigned";
                assignment.AssignedDate = DateTime.UtcNow;

                await _managerRepository.UpdateAssignmentAsync(assignment);
            }

            // Update order table also to "Assigned"
            await _managerRepository.UpdateOrderStatusAsync(orderId, "Assigned");
        }

        // Update only assignment status (Pending → Completed etc.)
        public async Task UpdateAssignmentStatusAsync(int assignmentId, string status)
        {
            await _managerRepository.UpdateAssignmentStatusAsync(assignmentId, status);
        }

        //  Get one specific order details with assignment info
        public async Task<OrderAssignmentDTO?> GetOrderDetailsAsync(int orderId)
        {
            var order = await _managerRepository.GetOrderByIdAsync(orderId);
            if (order == null) return null;

            var assignment = await _managerRepository.GetAssignmentByOrderIdAsync(orderId);

            return new OrderAssignmentDTO
            {
                Id = assignment?.Id ?? 0,
                OrderId = order.Id,
                CustomerName = order.Customer?.Username ?? "",
                GarmentType = order.GarmentType,
                Quantity = order.Quantity,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                ManagerId = assignment?.ManagerId ?? 0,
                ManagerName = assignment?.Manager?.Username ?? "",
                WorkerId = assignment?.WorkerId ?? 0,
                WorkerName = assignment?.Worker?.Username ?? "Not Assigned",
                Status = assignment?.Status ?? "Not Assigned",
                AssignedDate = assignment?.AssignedDate ?? default,
                CompletedDate = assignment?.CompletedDate,
                ImageContent = order.ImageContent,
                CustomerInstructions = order.CustomerInstructions ?? "No instructions",
                WorkerList = await _managerRepository.GetAllWorkersAsync() // For dropdown
            };
        }

        //  Get assignment details using assignmentId
        public async Task<OrderAssignmentDTO?> GetAssignmentByIdAsync(int assignmentId)
        {
            var a = await _managerRepository.GetAssignmentByIdAsync(assignmentId);
            if (a == null) return null;

            return new OrderAssignmentDTO
            {
                Id = a.Id,
                OrderId = a.OrderId,
                CustomerName = a.Order?.Customer?.Username ?? "",
                GarmentType = a.Order?.GarmentType ?? "",
                Quantity = a.Order?.Quantity ?? 0,
                OrderDate = a.Order?.OrderDate ?? default,
                DeliveryDate = a.Order?.DeliveryDate ?? default,
                ManagerId = a.ManagerId,
                ManagerName = a.Manager?.Username ?? "",
                WorkerId = a.WorkerId,
                WorkerName = a.Worker?.Username ?? "Not Assigned",
                Status = a.Status,
                AssignedDate = a.AssignedDate,
                CompletedDate = a.CompletedDate,
                ImageContent = a.Order.ImageContent,
                CustomerInstructions = a.Order.CustomerInstructions ?? "No instructions"
            };
        }

        // Get all workers list (for manager assign dropdown)
        public async Task<List<User>> GetAllWorkersAsync()
        {
            return await _managerRepository.GetAllWorkersAsync();
        }

        //  Update order status (Assigned / InProgress / Completed)
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            await _managerRepository.UpdateOrderStatusAsync(orderId, status);
        }
        
    }
}
