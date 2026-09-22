using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUI.Authorization;



namespace WebUI.Controllers
{
    [CustomAuthorize]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

    
        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // Get logged-in customer’s ID
        private int GetCustomerId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

    
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            try
            {
                int customerId = GetCustomerId();
                var orders = await _customerService.GetOrdersByCustomerAsync(customerId);
                return View(orders);
            }
            catch (Exception ex)
            {
               
                ViewBag.Error = "Something went wrong while loading your orders.";
                Console.WriteLine("Error in MyOrders: " + ex.Message);
                return View();
            }
        }

        // ---------- Create Order (Form) ----------
        [HttpGet]
        public IActionResult CreateOrder()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading Create Order page.";
                Console.WriteLine("Error in CreateOrder (GET): " + ex.Message);
                return View();
            }
        }

        // ---------- Create Order (Submit) ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(CustomerOrderDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                // Assign customer ID and current date
                model.CustomerId = GetCustomerId();
                model.OrderDate = DateTime.UtcNow;

                // Ensure DeliveryDate is stored properly
                model.DeliveryDate = DateTime.SpecifyKind(model.DeliveryDate, DateTimeKind.Utc);

                // Handle image upload
                var file = HttpContext.Request.Form.Files.FirstOrDefault(f => f.Name == "ImageUpload");
                if (file != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        model.ImageContent = ms.ToArray();
                    }
                }

                // Save order
                await _customerService.CreateOrderAsync(model);

                TempData["Success"] = "Order placed successfully!";
                return RedirectToAction(nameof(MyOrders));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error occurred while creating your order.";
                Console.WriteLine("Error in CreateOrder (POST): " + ex.Message);
                return View(model);
            }
        }

        // ---------- View Order Status ----------
        [HttpGet]
        public async Task<IActionResult> OrderStatus()
        {
            try
            {
                int customerId = GetCustomerId();
                var orders = await _customerService.GetOrdersByCustomerIdAsync(customerId);

                //  Calculate summary card counts
                ViewBag.TotalOrders = orders.Count();
                ViewBag.CompletedOrders = orders.Count(o => o.Status == "Completed");
                ViewBag.InProgressOrders = orders.Count(o => o.Status == "InProgress");
                ViewBag.PendingOrders = orders.Count(o => o.Status == "Pending");

                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to load order status right now.";
                Console.WriteLine("Error in OrderStatus: " + ex.Message);
                return View();
            }
        }
    }
}
