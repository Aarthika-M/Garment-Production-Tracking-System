using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public RolePermissionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<RolePermission?> GetPermissionAsync(string role, string controller, string action)
        {
            try
            {
                return await _db.RolePermissions
                    .FirstOrDefaultAsync(x => x.Role == role && x.Controller == controller && x.Action == action);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPermission: {ex.Message}");
                return null;
            }
        }

        public async Task<List<RolePermission>> GetPermissionsByRoleAsync(string role)
        {
            try
            {
                return await _db.RolePermissions
                    .Where(x => x.Role == role)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPermissionsByRoleAsync: {ex.Message}");
                return new List<RolePermission>();
            }
        }

        public async Task InsertOrUpdatePermissionAsync(string role, string controller, string action, bool enabled)
        {
            try
            {
                var existing = await _db.RolePermissions
                    .FirstOrDefaultAsync(x => x.Role == role && x.Controller == controller && x.Action == action);

                if (existing != null)
                {
                    existing.Enabled = enabled;
                    _db.RolePermissions.Update(existing);
                }
                else
                {
                    var newPermission = new RolePermission
                    {
                        Role = role,
                        Controller = controller,
                        Action = action,
                        Enabled = enabled
                    };
                    await _db.RolePermissions.AddAsync(newPermission);
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InsertOrUpdatePermissionAsync: {ex.Message}");
            }
        }

        public async Task SaveAllAsync(List<RolePermission> permissions)
        {
            try
            {
                foreach (var p in permissions)
                {
                    await InsertOrUpdatePermissionAsync(p.Role, p.Controller, p.Action, p.Enabled);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SaveAllAsync: {ex.Message}");
            }
        }
    }
}
