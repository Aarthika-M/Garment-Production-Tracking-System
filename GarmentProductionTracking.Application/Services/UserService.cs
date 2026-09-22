using Application.DTOs;       // For UserRegisterDTO
using Domain.Entities;        // For User
using Domain.Interfaces;      // For IUserRepository
using System.Security.Cryptography;
using System.Text;
using Domain.Enums;           

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // Register a new user
        public async Task<bool> RegisterAsync(UserRegisterDTO dto)
        {
            // Check if user already exists
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null) 
                return false;

            // Create new user entity
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = ComputeHash(dto.Password),
                Role = Enum.Parse<Role>(dto.Role) // Convert string ("Admin", "Manager", etc.) to enum
            };

            await _userRepository.AddAsync(user);
            return true;
        }

        // Validate user login
        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null) 
                return null;

            var passwordHash = ComputeHash(password);
            if (user.PasswordHash != passwordHash) 
                return null;

            return user; // Valid user
        }

        // Get user by username
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        // Helper method: Compute SHA256 hash for passwords
       private string ComputeHash(string input)
{
    using var sha = SHA256.Create();
    var bytes = Encoding.UTF8.GetBytes(input.Trim());
    var hashBytes = sha.ComputeHash(bytes);
    return Convert.ToBase64String(hashBytes);
}

    }
}
