using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<RolePermission?> GetPermissionAsync(string role, string controller, string action);
        Task<List<RolePermission>> GetPermissionsByRoleAsync(string role);
        Task InsertOrUpdatePermissionAsync(string role, string controller, string action, bool enabled);
        Task SaveAllAsync(List<RolePermission> permissions);
    }
}

