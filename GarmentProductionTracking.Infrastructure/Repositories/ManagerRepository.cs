using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class ManagerRepository : IManagerRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor — connects to the database
        public ManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all assignments (with order, customer, manager & worker info)
        public async Task<List<OrderAssignment>> GetAllAssignmentsAsync()
        {
            return await _context.OrderAssignments
                .Include(a => a.Order).ThenInclude(o => o.Customer)
                .Include(a => a.Manager)
                .Include(a => a.Worker)
                .OrderBy(a => a.AssignedDate)
                .ToListAsync();
        }

        // Add a new assignment to database
        public async Task AddAssignmentAsync(OrderAssignment assignment)
        {
            _context.OrderAssignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        // Update only assignment status (like Pending → Completed)
        public async Task UpdateAssignmentStatusAsync(int assignmentId, string status)
        {
            var assignment = await _context.OrderAssignments.FindAsync(assignmentId);
            if (assignment != null)
            {
                assignment.Status = status;

                // If completed, record the completion date
                if (status == "Completed")
                    assignment.CompletedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
        }

        // Get one assignment using its ID
        public async Task<OrderAssignment?> GetAssignmentByIdAsync(int assignmentId)
        {
            return await _context.OrderAssignments
                .Include(a => a.Order).ThenInclude(o => o.Customer)
                .Include(a => a.Manager)
                .Include(a => a.Worker)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);
        }

        // Get assignment using order ID (1 order → 1 assignment)
        public async Task<OrderAssignment?> GetAssignmentByOrderIdAsync(int orderId)
        {
            return await _context.OrderAssignments
                .Include(a => a.Manager)
                .Include(a => a.Worker)
                .FirstOrDefaultAsync(a => a.OrderId == orderId);
        }

        // Update full assignment details (worker, manager, date, status)
        public async Task UpdateAssignmentAsync(OrderAssignment assignment)
        {
            var existing = await _context.OrderAssignments.FindAsync(assignment.Id);
            if (existing != null)
            {
                existing.WorkerId = assignment.WorkerId;
                existing.ManagerId = assignment.ManagerId;
                existing.Status = assignment.Status;
                existing.AssignedDate = assignment.AssignedDate;

                _context.OrderAssignments.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        // Get all assignments assigned to one specific worker
        public async Task<List<OrderAssignment>> GetAssignmentsByWorkerIdAsync(int workerId)
        {
            return await _context.OrderAssignments
                .Where(a => a.WorkerId == workerId)
                .Include(a => a.Order).ThenInclude(o => o.Customer)
                .Include(a => a.Manager)
                .Include(a => a.Worker)
                .OrderBy(a => a.AssignedDate)
                .ToListAsync();
        }

        // Get all orders from DB (with customer info)
        public async Task<List<GarmentOrder>> GetAllOrdersAsync()
        {
            return await _context.GarmentOrders
                .Include(o => o.Customer)
                .OrderBy(o => o.OrderDate)
                .ToListAsync();
        }

        // Get one order by ID
        public async Task<GarmentOrder?> GetOrderByIdAsync(int orderId)
        {
            return await _context.GarmentOrders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        // Update order and assignment status together
       public async Task UpdateOrderStatusAsync(int orderId, string status)
{
    // Update in GarmentOrders table (for customer)
    var order = await _context.GarmentOrders.FindAsync(orderId);
    if (order != null)
    {
        order.Status = status;
        _context.GarmentOrders.Update(order);
    }

    // Update in OrderAssignments table (for manager/worker)
    var assignment = await _context.OrderAssignments
        .FirstOrDefaultAsync(a => a.OrderId == orderId);
    if (assignment != null)
    {
        assignment.Status = status;

        //  Update date fields based on status
        if (status == "In Progress")
        {
            assignment.AssignedDate = DateTime.UtcNow; // mark when work started
        }
        else if (status == "Completed")
        {
            assignment.CompletedDate = DateTime.UtcNow;
        }

        _context.OrderAssignments.Update(assignment);
    }

    await _context.SaveChangesAsync();
}

       

        // Get all users who have Role = Worker
        public async Task<List<User>> GetAllWorkersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == Role.Worker)
                .ToListAsync();
        }
        
    }
}
