using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebUI.Authorization;



namespace WebUI.Controllers
{
    [CustomAuthorize]
    public class WorkerController : Controller
    {
        private readonly WorkerService _workerService;
        private readonly ManagerService _managerService;

        public WorkerController(WorkerService workerService, ManagerService managerService)
        {
            _workerService = workerService;
            _managerService = managerService;
        }

       // Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard(int? workerId)
        {
            try
            {
                int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                string userRole = User.FindFirstValue(ClaimTypes.Role);

                int targetWorkerId;

                if (userRole == "Manager")
                {
                    if (workerId.HasValue)
                        targetWorkerId = workerId.Value;
                    else
                    {
                        var allWorkers = await _workerService.GetAllWorkersAsync();
                        targetWorkerId = allWorkers.FirstOrDefault()?.Id ?? 0;

                        if (targetWorkerId == 0)
                        {
                            ViewBag.Error = "No workers found to display.";
                            return View(new List<OrderAssignmentDTO>());
                        }
                    }
                }
                else
                {
                    targetWorkerId = loggedInUserId;
                }

                //  Fetch worker orders
                var assignments = await _workerService.GetOrdersByWorkerAsync(targetWorkerId);

                //  Calculate order summary
                var totalOrders = assignments.Count();
                var completedCount = assignments.Count(a => a.Status?.ToLower() == "completed");
                var inProgressCount = assignments.Count(a => a.Status?.ToLower() == "inprogress");
                var pendingCount = assignments.Count(a => a.Status?.ToLower() == "assigned" || a.Status?.ToLower() == "pending");

                //  Upcoming deadlines
                var upcomingDeadlines = assignments
                    .Where(o => o.DeliveryDate >= DateTime.Today)
                    .OrderBy(o => o.DeliveryDate)
                    .Take(5)
                    .ToList();

                // Pass to View
                ViewBag.TotalOrders = totalOrders;
                ViewBag.Completed = completedCount;
                ViewBag.InProgress = inProgressCount;
                ViewBag.Pending = pendingCount;
                ViewBag.WorkerId = targetWorkerId;
                ViewBag.IsManager = (userRole == "Manager");
                ViewBag.UpcomingDeadlines = upcomingDeadlines;

                return View(assignments);
            }
            catch (Exception ex)
            {
                //  Log or display error
                ViewBag.Error = "Something went wrong while loading the dashboard: " + ex.Message;
                return View(new List<OrderAssignmentDTO>());
            }
        }

        //  Status Page
        [HttpGet]
        public async Task<IActionResult> Status(int? workerId)
        {
            try
            {
                int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                string userRole = User.FindFirstValue(ClaimTypes.Role);

                int targetWorkerId;

                if (userRole == "Manager")
                {
                    if (workerId.HasValue)
                    {
                        targetWorkerId = workerId.Value;
                    }
                    else
                    {
                        var allWorkers = await _workerService.GetAllWorkersAsync();
                        targetWorkerId = allWorkers.FirstOrDefault()?.Id ?? 0;

                        if (targetWorkerId == 0)
                        {
                            ViewBag.Error = "No workers found to display.";
                            return View(new List<OrderAssignmentDTO>());
                        }
                    }
                }
                else
                {
                    targetWorkerId = loggedInUserId;
                }

                var orders = await _workerService.GetOrdersByWorkerAsync(targetWorkerId);
                return View(orders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Unable to load worker status: " + ex.Message;
                return View(new List<OrderAssignmentDTO>());
            }
        }

        //  Update Status (GET)
        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int assignmentId)
        {
            try
            {
                int loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var orders = await _workerService.GetOrdersByWorkerAsync(loggedInUserId);
                var assignment = orders.FirstOrDefault(a => a.Id == assignmentId);

                if (assignment == null)
                    return NotFound();

                return View(assignment);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error loading status update page: " + ex.Message;
                return View();
            }
        }

        //  Update Status (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int assignmentId, string status)
        {
            try
            {
                int workerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                await _workerService.UpdateAssignmentStatusAsync(assignmentId, status, workerId);

                TempData["SuccessMessage"] = "Order status updated successfully!";
                return RedirectToAction(nameof(Status));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating order status: " + ex.Message;
                return RedirectToAction(nameof(Status));
            }
        }
    }
}
