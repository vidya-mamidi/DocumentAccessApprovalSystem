using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAccessApprovalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessRequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public AccessRequestController(IRequestService requestService)        
        {
            _requestService = requestService;
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateAccessRequest(CreateAccessRequestDto accessRequestDto)
        {
            await _requestService.CreateAccessRequest(accessRequestDto);
            return Ok("Request submitted.");
        }

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRequestsByUser(int userId)
        {
            var requests = await _requestService.GetRequestsByUser(userId);
            if (requests == null || !requests.Any())
                return NotFound($"No access Requests Found for the User With Id {userId}");


            return Ok(requests.Select(x=> new
            {
                x.Id,
                x.UserId,
                UserName = x.User.Name,
                x.DocumentId,
                DocumentTitle = x.Document.Title,
                x.RequestedAccess,
                x.Status,
                x.Reason,
                x.CreatedAt
            }));
        }

    }
}
