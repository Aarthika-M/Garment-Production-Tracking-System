using Domain.Entities;


namespace Domain.Interfaces
{
    public interface IManagerRepository
    {
        // Assignments
        Task<List<OrderAssignment>> GetAllAssignmentsAsync();
        Task<OrderAssignment?> GetAssignmentByIdAsync(int assignmentId);
        Task<OrderAssignment?> GetAssignmentByOrderIdAsync(int orderId);
        Task AddAssignmentAsync(OrderAssignment assignment);
        Task UpdateAssignmentStatusAsync(int assignmentId, string status);

        // Orders
        Task<List<GarmentOrder>> GetAllOrdersAsync();
        Task<GarmentOrder?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, string status);

        // Workers
        Task<List<User>> GetAllWorkersAsync(); 
        Task<List<OrderAssignment>> GetAssignmentsByWorkerIdAsync(int workerId);// Keep only one method
        // Task<List<OrderAssignment>> GetAssignmentsForSpecificWorkerAsync(int workerId);

          Task UpdateAssignmentAsync(OrderAssignment assignment);
    }
}
