using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class WorkerService
    {
        private readonly IManagerRepository _managerRepository;

        public WorkerService(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository;
        }

        //   Get all orders assigned to a specific worker (DTO format)
       public async Task<List<OrderAssignmentDTO>> GetOrdersByWorkerAsync(int workerId)
{
    var assignments = await _managerRepository.GetAssignmentsByWorkerIdAsync(workerId);

    if (assignments == null || !assignments.Any())
        return new List<OrderAssignmentDTO>();

    return assignments.Select(a => new OrderAssignmentDTO //convert entity into dto
    {
        Id = a.Id,
        OrderId = a.OrderId,
        CustomerName = a.Order?.Customer?.Username ?? "Unknown",
        ManagerId = a.ManagerId,
        ManagerName = a.Manager?.Username ?? "Unassigned",
        WorkerId = a.WorkerId,
        WorkerName = a.Worker?.Username ?? "Unknown Worker",
        Status = a.Status ?? "Pending",
        AssignedDate = a.AssignedDate,
        CompletedDate = a.CompletedDate,
        GarmentType = a.Order?.GarmentType ?? "N/A",
        Quantity = a.Order?.Quantity ?? 0,
        OrderDate = a.Order?.OrderDate ?? DateTime.MinValue,
        DeliveryDate = a.Order?.DeliveryDate ?? DateTime.MinValue,
        ImageContent = a.Order?.ImageContent,
        CustomerInstructions = a.Order?.CustomerInstructions ?? "No instructions"
    }).ToList();
}


        // Get all orders assigned to a worker (Entity format)
        public async Task<List<OrderAssignment>> GetOrdersByWorkerEntitiesAsync(int workerId)
        {
            var assignments = await _managerRepository.GetAllAssignmentsAsync();
            return assignments.Where(a => a.WorkerId == workerId).ToList();
        }

        // Update assignment status (e.g., InProgress → Completed)
        public async Task UpdateAssignmentStatusAsync(int assignmentId, string status, int workerId)
        {
            var assignment = await _managerRepository.GetAssignmentByIdAsync(assignmentId);

            if (assignment != null && assignment.WorkerId == workerId)
            {
                assignment.Status = status;

                if (status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                {
                    assignment.CompletedDate = DateTime.UtcNow;
                }

                await _managerRepository.UpdateAssignmentStatusAsync(assignmentId, status);
            }
        }

        //  Get all workers (for dropdowns, dashboard, etc.)
        public async Task<List<User>> GetAllWorkersAsync()
        {
            return await _managerRepository.GetAllWorkersAsync();
        }

    
    }
}
