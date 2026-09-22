using Domain.Entities;


namespace Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task CreateOrderAsync(GarmentOrder order);
        Task<List<GarmentOrder>> GetOrdersByCustomerAsync(int customerId); 
        Task<GarmentOrder?> GetOrderByIdAsync(int id);
        Task<List<GarmentOrder>> GetOrdersByCustomerIdAsync(int customerId);//manager see the specificc customers
        Task<List<GarmentOrder>> GetAllOrdersAsync();
    }
    }
