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
        private readonly IHttpContextAccessor _contextAccessor;
        public UserServices(TikiDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _contextAccessor = httpContextAccessor;
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
                    Status = true,
                    Fullname = userRequest.FullName
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

        public async Task<Result<string>> ChangePassword(string phoneNumber, string oldPassword, string newPassowrd)
        {
            try
            {
                ClaimsPrincipal User = _contextAccessor.HttpContext.User;
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var foundUser = await _context.Users.FindAsync(id);
                if(foundUser.PhoneNumber != phoneNumber)
                {
                    return new Result<string> { Message = "Phone number not match", Success = false };
                }
                else
                {
                    if(!Bcrypt.Verify(oldPassword, foundUser.Password))
                    {
                        return new Result<string> { Message = "Password not correct", Success = false };
                    }
                    var salt = Bcrypt.GenerateSalt(10);
                    foundUser.Password = Bcrypt.HashPassword(newPassowrd, salt);
                    await _context.SaveChangesAsync();
                    return new Result<string> { Success = true };
                }    
            }
            catch (Exception)
            {

                return new Result<string> { Message = "Server error", Success = false };
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
                                PhoneNumber = user.PhoneNumber, Status = user.Status, Role = role.Name, Fullname = user.Fullname 
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

        public async Task<Result<UserDTO>> GetUser()
        {
            try
            {
                ClaimsPrincipal User = _contextAccessor.HttpContext.User;
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var foundUser = await _context.Users.FindAsync(id);
                if(foundUser != null)
                {
                    var user = new UserDTO
                    {
                        Address = foundUser.Address,
                        Balance = foundUser.Balance,
                        Commission = foundUser.Commission,
                        Email = foundUser.Email,
                        Id = foundUser.Id,
                        Fullname = foundUser.Fullname,
                        PhoneNumber = foundUser.PhoneNumber,
                        Status = foundUser.Status,
                        Role = (await _context.Roles.FindAsync(foundUser.RoleId)).Name
                    };
                    return new Result<UserDTO> { Data = user, Success = true };
                }
                else
                {
                    return new Result<UserDTO> { Message = "User is not exist", Success = false };
                }    
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Result<string>> Login(string phoneNumber, string password)
        {
            try
            {
                //var query = from user in _context.Users
                //            join role in _context.Roles on user.RoleId equals role.Id
                //            where user.PhoneNumber == phoneNumber && user.Status
                //            select new UserDTO
                //            {
                //                Address = user.Address,
                //                Balance = user.Balance,
                //                Commission = user.Commission,
                //                Email = user.Email,
                //                Id = user.Id,
                //                PhoneNumber = user.PhoneNumber,
                //                Status = user.Status,
                //                Role = role.Name,
                //                Fullname = user.Fullname
                //            };
                //var userDTO = await query.FirstOrDefaultAsync();
                var user = await _context.Users.Where(x => x.PhoneNumber == phoneNumber && x.Status).FirstOrDefaultAsync();
                if (user == null)
                {
                    return new Result<string> { Message = "User is not exist", Success = false };
                }
                else
                {
                    if(Bcrypt.Verify(password, user.Password))
                    {
                        var userDTO = new UserDTO
                        {
                            Address = user.Address,
                            Balance = user.Balance,
                            Commission = user.Commission,
                            Email = user.Email,
                            Id = user.Id,
                            PhoneNumber = user.PhoneNumber,
                            Status = user.Status,
                            Role = (await _context.Roles.FindAsync(user.RoleId)).Name,
                            Fullname = user.Fullname
                        };
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
