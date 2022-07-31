using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using tiki_shop.Models;
using tiki_shop.Models.Common;
using tiki_shop.Models.DTO;
using tiki_shop.Models.Entity;
using tiki_shop.Models.Request;
using Bcrypt = BCrypt.Net.BCrypt;


namespace tiki_shop.Services
{
    public class UserServices : IUserService
    {
        private readonly TikiDbContext _context;
        private readonly IConfiguration _configuration;
        public UserServices(TikiDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<Result<bool>> AddUser(UserRequest userRequest)
        {
            try
            {
                var salt = Bcrypt.GenerateSalt(10);
                var user = new User()
                {
                    PhoneNumber = userRequest.PhoneNumber,
                    Email = userRequest.Email,
                    Address = userRequest.Address,
                    Password = Bcrypt.HashPassword(userRequest.Password, salt),
                    RoleId = 2,
                    Id = Guid.NewGuid().ToString(),
                    Status = true
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return new Result<bool> { Success = true, Data = true, Message = ""};
            }
            catch (Exception)
            {

                return new Result<bool> { Success = false, Data = false, Message = "Server error" };
            }
        }

        public async Task<ResultList<UserDTO>> GetAllUsers()
        {
            try
            {
                var query = from user in _context.Users
                            join role in _context.Roles on user.RoleId equals role.Id
                            select new UserDTO
                            {
                                Address = user.Address, Balance = user.Balance, Commission = user.Commission, Email = user.Email, Id = user.Id,
                                Password = user.Password, PhoneNumber = user.PhoneNumber, Status = user.Status, Role = role.Name, Fullname = user.Fullname 
                            };
                var listUser = await query.ToListAsync();
                //var json = JsonSerializer.Serialize(listUser);
                return new ResultList<UserDTO> { Data = listUser, Message = "", Success = true};
            }
            catch (Exception)
            {

                return new ResultList<UserDTO> { Message = "Server error", Success = false };
            }
        }

        public async Task<Result<string>> Login(string phoneNumber, string password)
        {
            try
            {
                var query = from user in _context.Users
                            join role in _context.Roles on user.RoleId equals role.Id
                            where user.PhoneNumber == phoneNumber && user.Status
                            select new UserDTO
                            {
                                Address = user.Address,
                                Balance = user.Balance,
                                Commission = user.Commission,
                                Email = user.Email,
                                Id = user.Id,
                                Password = user.Password,
                                PhoneNumber = user.PhoneNumber,
                                Status = user.Status,
                                Role = role.Name,
                                Fullname = user.Fullname
                            };
                var userDTO = await query.FirstOrDefaultAsync();
                if(userDTO == null)
                {
                    return new Result<string> { Message = "User is not exist", Success = false };
                }
                else
                {
                    if(Bcrypt.Verify(password, userDTO.Password))
                    { 
                        
                        return new Result<string> { Message = "Login success", Success = true, Data = CreateToken(userDTO) };
                    }
                    else
                    {
                        return new Result<string> { Message = "Password not correct", Success = false};
                    }    
                }    
            }
            catch (Exception)
            {

                throw;
            }
        }
        private string CreateToken(UserDTO user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Role, user.Role),
            };
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred,
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
