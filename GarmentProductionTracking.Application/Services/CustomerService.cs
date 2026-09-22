using Domain.Interfaces;
using Application.DTOs;
using Domain.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IManagerRepository _managerRepository;

        // Constructor — connects this service with the repositories
        public CustomerService(ICustomerRepository customerRepository, IManagerRepository managerRepository)
        {
            _customerRepository = customerRepository;
            _managerRepository = managerRepository;
        }

      
      
       
        public async Task CreateOrderAsync(CustomerOrderDTO dto)
        {
            // Convert the DTO (data transfer object) into the Entity model
            var order = new GarmentOrder
            {
                CustomerId = dto.CustomerId,
                GarmentType = dto.GarmentType,
                Quantity = dto.Quantity,
                OrderDate = dto.OrderDate,
                DeliveryDate = dto.DeliveryDate,
                ImageContent = dto.ImageContent,
                CustomerInstructions = dto.CustomerInstructions
            };

            // Save to database using the repository
            await _customerRepository.CreateOrderAsync(order);
        }

        
        //   GET ALL ORDERS BY A CUSTOMER
       
        public async Task<List<CustomerOrderDTO>> GetOrdersByCustomerAsync(int customerId)
        {
            // Get all orders for the given customer from repository
            var orders = await _customerRepository.GetOrdersByCustomerAsync(customerId);

            // Convert Entity -> DTO
            return orders.Select(o => new CustomerOrderDTO
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                GarmentType = o.GarmentType,
                Quantity = o.Quantity,
                OrderDate = o.OrderDate,
                DeliveryDate = o.DeliveryDate,
                ImageContent = o.ImageContent,
                CustomerInstructions = o.CustomerInstructions
            }).ToList();
        }

       
        //   GET A SINGLE ORDER BY ID
        
        public async Task<CustomerOrderDTO?> GetOrderByIdAsync(int id)
        {
            // Find order by its ID
            var order = await _customerRepository.GetOrderByIdAsync(id);
            if (order == null)
                return null; // If not found, return null

            // Convert Entity -> DTO
            return new CustomerOrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                GarmentType = order.GarmentType,
                Quantity = order.Quantity,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status
            };
        }

        
        //  GET ORDERS BY CUSTOMER ID (with Customer info)
       
        public async Task<List<GarmentOrder>> GetOrdersByCustomerIdAsync(int customerId)
        {
            return await _customerRepository.GetOrdersByCustomerIdAsync(customerId);
        }

      
        //   GET ALL ORDERS (for Admin/Manager)
       
        public async Task<List<GarmentOrder>> GetAllOrdersAsync()
        {
            return await _customerRepository.GetAllOrdersAsync();
        }
    }
}
