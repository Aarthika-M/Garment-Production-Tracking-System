using Application.DTOs;     
using Domain.Entities;       
using Domain.Interfaces;     
using System.Security.Cryptography;//used for hash password
using System.Text;

namespace Application.Services
{
    //  AuthService → Handles Login & Role Checking
    public class AuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User?> LoginAsync(UserLoginDTO dto)
        {
         
            var user = await _userRepository.GetByUsernameAsync(dto.Username);

         
            if (user == null)
                return null;

            var enteredPasswordHash = ComputeHash(dto.Password);

            //  Compare entered password hash with stored password hash
            if (user.PasswordHash == enteredPasswordHash)
                return user; //  Login success → return user

            return null; //  Password mismatch → login fail
        }
  
        public async Task<bool> ValidateRoleAsync(string username, string role)
        {
            
            var user = await _userRepository.GetByUsernameAsync(username);

            //  If user doesn't exist → false
            if (user == null)
                return false;

            //  Compare user's role with given role string
            return user.Role.ToString() == role;
        }
  //convert plain text into hashed password
        private string ComputeHash(string input)
        {
            //  Create SHA256 object
            using var sha = SHA256.Create();

            //  Convert string to bytes
            var bytes = Encoding.UTF8.GetBytes(input);

            //  Compute hash → get byte array
            var hashBytes = sha.ComputeHash(bytes);

            //  Convert hash bytes → Base64 string (for easy DB storage)
            return Convert.ToBase64String(hashBytes);
        }
    }
}
