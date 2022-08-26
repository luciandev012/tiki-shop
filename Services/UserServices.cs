using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
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
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Role> _roleCollection;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserServices(IOptions<TikiDbSettings> tikiDb, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            var mongoClient = new MongoClient(tikiDb.Value.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(tikiDb.Value.DatabaseName);
            _userCollection = mongoDb.GetCollection<User>("users");
            _roleCollection = mongoDb.GetCollection<Role>("roles");
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
                    Role = "User",
                    Status = true,
                    Fullname = userRequest.FullName
                };
                await _userCollection.InsertOneAsync(user);
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
                var foundUser = await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
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
                    var update = Builders<User>.Update.Set(s => s.Password, Bcrypt.HashPassword(newPassowrd, salt));
                    var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(id));
                    await _userCollection.UpdateOneAsync(filter, update);
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
                var users = await _userCollection.Find(_ => true).ToListAsync();
                var listUser = new List<UserDTO>();
                foreach (var user in users)
                {
                    listUser.Add(new UserDTO {
                        Id = user.Id, Address = user.Address, Balance = user.Balance, Email = user.Email,
                        Fullname = user.Fullname, PhoneNumber = user.PhoneNumber, Role = user.Role,
                        Status = user.Status
                    });
                }
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
                var reqId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var foundUser = await _userCollection.Find(x => x.Id == reqId).FirstOrDefaultAsync();
                if (foundUser != null)
                {
                    var user = new UserDTO
                    {
                        Address = foundUser.Address,
                        Balance = foundUser.Balance,
                        Email = foundUser.Email,
                        Id = foundUser.Id,
                        Fullname = foundUser.Fullname,
                        PhoneNumber = foundUser.PhoneNumber,
                        Status = foundUser.Status,
                        Role = foundUser.Role
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

                return new Result<UserDTO> { Message = "Server error", Success = false };
            }
        }

        public async Task<Result<UserDTO>> GetUserById(string reqId)
        {
            try
            {
                var foundUser = await _userCollection.Find(x => x.Id == reqId).FirstOrDefaultAsync();
                if (foundUser != null)
                {
                    var user = new UserDTO
                    {
                        Address = foundUser.Address,
                        Balance = foundUser.Balance,
                        Email = foundUser.Email,
                        Id = foundUser.Id,
                        Fullname = foundUser.Fullname,
                        PhoneNumber = foundUser.PhoneNumber,
                        Status = foundUser.Status,
                        Role = foundUser.Role
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

                return new Result<UserDTO> { Message = "User is not exist", Success = false };
            }
        }

        public async Task<Result<string>> Login(string phoneNumber, string password)
        {
            try
            {
                var foundUser = await _userCollection.Find(x => x.PhoneNumber == phoneNumber && x.Status).FirstOrDefaultAsync();
                if (foundUser == null)
                {
                    return new Result<string> { Message = "User is not exist", Success = false };
                }
                else
                {
                    if(Bcrypt.Verify(password, foundUser.Password))
                    {
                        var userDTO = new UserDTO
                        {
                            Address = foundUser.Address,
                            Balance = foundUser.Balance,
                            Email = foundUser.Email,
                            Id = foundUser.Id,
                            Fullname = foundUser.Fullname,
                            PhoneNumber = foundUser.PhoneNumber,
                            Status = foundUser.Status,
                            Role = foundUser.Role
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
        public async Task<ResultList<Role>> GetRoles()
        {
            try
            {
                var roles = await _roleCollection.Find(_ => true).ToListAsync();
                return new ResultList<Role> { Data = roles, Success = true };
            }
            catch (Exception)
            {

                return new ResultList<Role> { Success = false, Message = "Server error" };
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
