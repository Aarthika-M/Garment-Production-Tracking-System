using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorization;


namespace WebUI.Controllers
{
    [CustomAuthorize]
    public class AccessControlController : Controller
    {
        private readonly IRolePermissionRepository _repo;

        public AccessControlController(IRolePermissionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string role = "Manager")
        {  
            try
            {
            
            ViewBag.Role = role;

            var allActions = new Dictionary<string, List<string>>
            {
                { "Customer", new List<string>{ "MyOrders", "CreateOrder", "OrderStatus" } },
                { "Manager", new List<string>{ "Home", "Dashboard", "AssignOrder", "UpdateStatus", "UpdateOrderStatus", "OrderDetails" } },
                { "Worker", new List<string>{ "Dashboard", "Status", "UpdateStatus" } }
            };

            ViewBag.Controllers = allActions;

            var existing = await _repo.GetPermissionsByRoleAsync(role);
            ViewBag.Permissions = existing;

            return View();
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Something went wrong while loading your AccessControl.";
                Console.WriteLine("Error in AccessControl: " + ex.Message);
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> SaveAllPermissions([FromBody] List<RolePermission> list)
        {   
            try{
            if (list == null || !list.Any())
                return Json(new { message = "No data received" });
           
            foreach (var p in list)
            {
                await _repo.InsertOrUpdatePermissionAsync(
                    p.Role,
                    p.Controller,
                    p.Action,
                    p.Enabled
                );
            }

            return Json(new { message = "Permissions saved successfully!" });
        }
        
      catch(Exception ex)
        {
            Console.WriteLine("Error Occurred Saving the permission:"+ex.Message);
            return Json(new{message="Error Occurred while Saving the permission"});
        }
    }
}
}
