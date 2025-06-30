using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Application.Models;
using DocumentAccessApprovalSystem.Domain.Entities;

namespace DocumentAccessApprovalSystem.Application.Interfaces
{
    public interface IDecisionService
    {
        Task MakeDecision(DecisionDto decisionDto);
        Task<IEnumerable<AccessRequest>> GetPendingRequests();

        Task<IEnumerable<AccessRequest>> GetRequests();
    }
}
