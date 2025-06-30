using DocumentAccessApprovalSystem.API.Services;
using DocumentAccessApprovalSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using DocumentAccessApprovalSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentAccessApprovalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext appDbContext, TokenService tokenService)
        {
            _appDbContext = appDbContext;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login  request)
        {
            var user = _appDbContext.Users.FirstOrDefault(u => u.Name == request.Username);

            if (user == null) return Unauthorized();

            var token = _tokenService.GenerateToken(user.Id, user.Name, user.Role.ToString());
            return Ok(new { token });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _appDbContext.Users
                .Select(d => new
                {
                    d.Id,
                    d.Name,
                    d.Role
                })
                .ToListAsync();

            return Ok(users);
        }
    }
}
