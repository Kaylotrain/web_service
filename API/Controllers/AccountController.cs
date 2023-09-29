using System.Security.Cryptography;
using System.Text;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_service.Data;
using web_service.Entities;

namespace API.Controllers
{
     public class AccountController : BaseApiController
     {
          private readonly DataContext _context;

          private readonly ITokenService _tokenService;

          private const string USER_PASSWORD_ERROR_MESSAGE = "Usuario o contraseña incorrectos";

          public AccountController(DataContext context, ITokenService tokenService)
          {
               _context = context;
               _tokenService = tokenService;
          }

          [HttpPost("register")]
          public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
          {
               if (await UserExist(registerDTO.Username))
               {
                    return BadRequest("Ya existe un usuario con ese nombre");
               }
               using var hmac = new HMACSHA512();

               var user = new AppUser
               {
                    UserName = registerDTO.Username,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                    PasswordSalt = hmac.Key
               };

               _context.Users.Add(user);
               await _context.SaveChangesAsync();

               return new UserDTO
               {
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
               };
          }

          [HttpPost("login")]
          public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
          {
               var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == loginDTO.Username.ToLower());

               if (user == null)
               {
                    return Unauthorized(USER_PASSWORD_ERROR_MESSAGE);
               }

               using var hmac = new HMACSHA512(user.PasswordSalt);

               var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

               for (int i = 0; i < computedHash.Length; i++)
               {
                    if (computedHash[i] != user.PasswordHash[i])
                    {
                         return Unauthorized(USER_PASSWORD_ERROR_MESSAGE);
                    }
               }

               return new UserDTO
               {
                    Username = user.UserName,
                    Token = _tokenService.CreateToken(user)
               };
          }

          private async Task<bool> UserExist(string username)
          {
               return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
          }
     }
}