using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUI.Authorization;


namespace WebUI.Controllers
{
    [CustomAuthorize]
    public class ManagerController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly WorkerService _workerService;
        private readonly ManagerService _managerService;

        // Constructor — inject services
        public ManagerController(CustomerService customerService, WorkerService workerService, ManagerService managerService)
        {
            _customerService = customerService;
            _workerService = workerService;
            _managerService = managerService;
        }

        // ---------- Manager Dashboard Home ----------
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            try
            {
                var allOrders = await _managerService.GetAllOrdersForDashboardAsync();
                var allWorkers = await _workerService.GetAllWorkersAsync();

                ViewBag.TotalOrders = allOrders.Count;
                ViewBag.TotalWorkers = allWorkers.Count;
                ViewBag.CompletedOrders = allOrders.Count(o => o.Status == "Completed");
                ViewBag.InProgressOrders = allOrders.Count(o => o.Status == "InProgress");
                ViewBag.PendingOrders = allOrders.Count(o => o.Status == "Pending");

                ViewBag.WorkersAssigned = allOrders
                    .Where(o => !string.IsNullOrEmpty(o.WorkerName) && o.Status == "InProgress")
                    .Select(o => o.WorkerName)
                    .Distinct()
                    .Count();

                return View(allOrders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading dashboard data.";
                Console.WriteLine("Error in Manager/Home: " + ex.Message);
                return View();
            }
        }

        // ---------- View All Orders ----------
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                var orders = await _managerService.GetAllOrdersForDashboardAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading dashboard.";
                Console.WriteLine("Error in Dashboard: " + ex.Message);
                return View();
            }
        }

        // ---------- Assign Order (GET) ----------
        [HttpGet]
        public async Task<IActionResult> AssignOrder(int? orderId)
        {
            try
            {
                if (orderId == null)
                {
                    var orders = await _managerService.GetAllOrdersForDashboardAsync();
                    return View("AssignOrderList", orders);
                }

                var orderDetails = await _managerService.GetOrderDetailsAsync(orderId.Value);
                if (orderDetails == null)
                {
                    TempData["Error"] = $"Order with ID {orderId} not found.";
                    return RedirectToAction("Dashboard");
                }

                orderDetails.WorkerList = await _managerService.GetAllWorkersAsync();
                return View(orderDetails);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading order assignment.";
                Console.WriteLine("Error in AssignOrder: " + ex.Message);
                return View();
            }
        }

        // ---------- Assign Order (POST) ----------
        [HttpPost]
        public async Task<IActionResult> AssignOrder(int orderId, int workerId)
        {
            try
            {
                var managerIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(managerIdValue))
                    return Unauthorized("Manager ID not found in claims.");

                int managerId = int.Parse(managerIdValue);

                await _managerService.AssignWorkerToOrderAsync(orderId, workerId, managerId);
                TempData["SuccessMessage"] = "Worker assigned successfully!";
                return RedirectToAction("Dashboard");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to assign worker to the order.";
                Console.WriteLine("Error in AssignOrder (POST): " + ex.Message);
                return RedirectToAction("Dashboard");
            }
        }

        // ---------- Update Order Status (GET) ----------
        [HttpGet]
        public async Task<IActionResult> UpdateStatus()
        {
            try
            {
                var orders = await _managerService.GetAllOrdersForDashboardAsync();
                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading order list for status update.";
                Console.WriteLine("Error in UpdateStatus: " + ex.Message);
                return View();
            }
        }

        // ---------- Update Order Status (POST) ----------
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                await _managerService.UpdateOrderStatusAsync(orderId, status);
                TempData["Message"] = "Order status updated successfully!";
                return RedirectToAction("UpdateStatus");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update order status.";
                Console.WriteLine("Error in UpdateOrderStatus: " + ex.Message);
                return RedirectToAction("UpdateStatus");
            }
        }

        // ---------- View Order Details ----------
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int orderId)
        {
            try
            {
                var orderDetails = await _managerService.GetOrderDetailsAsync(orderId);
                if (orderDetails == null)
                    return NotFound();

                return View(orderDetails);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading order details.";
                Console.WriteLine("Error in OrderDetails: " + ex.Message);
                return View();
            }
        }
    }
}
