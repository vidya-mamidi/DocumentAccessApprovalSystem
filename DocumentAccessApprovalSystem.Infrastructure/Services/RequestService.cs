using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Application.Interfaces;
using DocumentAccessApprovalSystem.Application.Models;
using DocumentAccessApprovalSystem.Domain.Entities;
using DocumentAccessApprovalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DocumentAccessApprovalSystem.Application.Services
{
    public class RequestService : IRequestService
    {
        private readonly AppDbContext _appDbContext;
        public RequestService (AppDbContext appDbContext) => _appDbContext = appDbContext;
        public async Task CreateAccessRequest(CreateAccessRequestDto accessRequestDto)
        {

            var duplicateExists = await _appDbContext.AccessRequests.AnyAsync(r =>
           r.UserId == accessRequestDto.UserId &&
           r.DocumentId == accessRequestDto.DocumentId);

            if (duplicateExists)
                throw new InvalidOperationException("You have already submitted a  request for this document.");

            var request = new AccessRequest
            {               
                UserId = accessRequestDto.UserId,
                DocumentId = accessRequestDto.DocumentId,
                RequestedAccess = accessRequestDto.RequestedAccess,
                Reason = accessRequestDto.Reason
            };
            _appDbContext.AccessRequests.Add(request);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AccessRequest>> GetRequestsByUser(int userId)
        {
            return await _appDbContext.AccessRequests.Where(x=>x.UserId == userId)
                .Include(x=>x.User)
                .Include(x=>x.Document)
                .ToListAsync();
        }

      
    }
}
