// using Domain.Interfaces;

// using Domain.Entities;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace Application.Services
// {public class RolePermissionService
// {
//     private readonly IRolePermissionRepository _repo;

//     public RolePermissionService(IRolePermissionRepository repo)
//     {
//         _repo = repo;
//     }

//     public async Task<List<RolePermission>> GetPermissionsByRoleAsync(string roleName)
//     {
//         return await _repo.GetByRoleAsync(roleName);
//     }

//     public async Task UpdatePermissionAsync(RolePermission permission)
//     {
//         await _repo.UpdateAsync(permission);
//     }
// }

// }
