using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        //  Constructor: connects repository with the database context
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //  Create a new order
        public async Task CreateOrderAsync(GarmentOrder order)
        {
            _context.GarmentOrders.Add(order);
            await _context.SaveChangesAsync();
        }

        //  Get all orders of a specific customer (basic)
        public async Task<List<GarmentOrder>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _context.GarmentOrders
                                 .Where(o => o.CustomerId == customerId)
                                 .ToListAsync();
        }

        //  Get one order by its ID
        public async Task<GarmentOrder?> GetOrderByIdAsync(int id)
        {
            return await _context.GarmentOrders
                                 .FirstOrDefaultAsync(o => o.Id == id);
        }

        //  Get all orders of a customer (with customer details)
        public async Task<List<GarmentOrder>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _context.GarmentOrders
                                 .Include(o => o.Customer)
                                 .Where(o => o.CustomerId == customerId)
                                 .ToListAsync();
        }

        //  Get all orders from all customers (for admin/manager)
        public async Task<List<GarmentOrder>> GetAllOrdersAsync()
        {
            return await _context.GarmentOrders
                                 .Include(o => o.Customer)
                                 .ToListAsync();
        }
    }
}
