using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Application.Models;
using DocumentAccessApprovalSystem.Application.Services;
using DocumentAccessApprovalSystem.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAccessApprovalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecisionController : ControllerBase
    {
        private readonly IDecisionService _decisionService;

        public DecisionController(IDecisionService decisionService) => _decisionService = decisionService;

        [Authorize(Roles = "Approver")]
        [HttpPost]
        public async Task<IActionResult> MakeDecision(DecisionDto decisionDto)
        {
            await _decisionService.MakeDecision(decisionDto);
            return Ok("Decision recorded.");
        }

        [Authorize(Roles = "Approver")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests()
        {

            var pending = await _decisionService.GetPendingRequests();
            if (pending == null || !pending.Any())
                return NotFound($"No Pending  Requests Found for Approvals");
            return Ok(pending.Select(x => new
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
        [Authorize(Roles = "Approver")]
        [HttpGet("Requests")]
        public async Task<IActionResult> GetRequests()
        {

            var requests = await _decisionService.GetRequests();
            if (requests == null || !requests.Any())
                return NotFound($"No  Requests Found ");
            return Ok(requests.Select(x => new
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
