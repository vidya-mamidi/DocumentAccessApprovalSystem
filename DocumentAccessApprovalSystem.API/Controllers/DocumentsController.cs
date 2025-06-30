using DocumentAccessApprovalSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentAccessApprovalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public DocumentsController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var documents = await _appDbContext.Documents
                .Select(d => new
                {
                    d.Id,
                    d.Title
                })
                .ToListAsync();

            return Ok(documents);
        }
    }
}
