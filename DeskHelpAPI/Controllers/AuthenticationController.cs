using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeskHelpAPI.Data;
using Microsoft.AspNetCore.Authorization;
using DeskHelpAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace DeskHelpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly SqlDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticationController(SqlDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register")] // api/authentication/register
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            try
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email
                };

                user.GeneratePassword(model.Password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return new BadRequestObjectResult(ex.Message); }

            return new OkResult();
        }


        [AllowAnonymous]
        [HttpPost("login")] // api/authentication/login
        public async Task<IActionResult> LogIn([FromBody] Register model)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == model.Email);

                if (user != null)
                {
                    if (user.ValidatePassword(model.Password))
                    {
                        var th = new JwtSecurityTokenHandler();
                        var expiresDate = DateTime.Now.AddDays(1);

                        var td = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim("UserId", user.Id.ToString()),
                                new Claim("Expires", expiresDate.ToString())
                            }),
                            Expires = expiresDate,
                            SigningCredentials = new SigningCredentials(
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(_configuration.GetSection("SecretKey").Value)),
                                    SecurityAlgorithms.HmacSha512Signature
                                )
                        };

                        var _accessToken = th.WriteToken(th.CreateToken(td));

                        return new OkObjectResult(_accessToken);
                    }
                }
            }
            catch (Exception ex) { return new BadRequestObjectResult(ex.Message); }

            return new BadRequestResult();
        }
    }
}
